using C969.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace C969
{
    public partial class RecordsForm : Form
    {
        HelperFunctions _helperFunctions;
        DateTime _localNow;

        int _selectedCustomerId;
        string _customerQuery = @"
            SELECT 
                customerId, 
                customerName, 
                addressId, 
                CASE 
                    WHEN active = 1 THEN 'Not Active' 
                    ELSE 'Active' 
                END AS active, 
                createDate, 
                createdBy, 
                lastUpdate, 
                lastUpdateBy 
            FROM customer";
        string _customerAppointmentQuery = "SELECT * FROM appointment WHERE customerId";

        public RecordsForm()
        {
            InitializeComponent();
            _helperFunctions = new HelperFunctions();
            _helperFunctions.DataGridLayout(customerDataGrid);
            _helperFunctions.DataGridLayout(appointmentDataGrid);
            _helperFunctions.ConfigureAppointmentColumns(appointmentDataGrid);

            updateCustomerButton.Enabled = false;
            deleteCustomerButton.Enabled = false;
            appointmentAddButton.Enabled = false;
            appointmentUpdateButton.Enabled = false;
            appointmentDeleteButton.Enabled = false;

            customerDataGrid.SelectionChanged += CustomerDataGrid_SelectionChanged;
            appointmentDataGrid.SelectionChanged += AppointmentDataGrid_SelectionChanged;

            _localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DBConnection.timeZone);

            Load += RecordsForm_Load;
            FormClosed += EndConnectionOnClose;
        }

        public void ReloadCustomerAppointmentData(int customerId)
        {
            string appointmentQuery = $"{_customerAppointmentQuery} = {customerId}";
            DataTable appointmentTable = _helperFunctions.LoadQueryToTable(appointmentQuery);
            appointmentDataGrid.DataSource = appointmentTable;
            appointmentDataGrid.ClearSelection();
        }

        void AppointmentTimeChecker()
        {
            try
            {
                var now = _localNow;
                var fifteenMinutesLater = now.AddMinutes(15);

                string query = @"
                    SELECT a.appointmentId, a.title, a.start, c.customerName
                    FROM appointment a
                    JOIN customer c ON a.customerId = c.customerId";

                DataTable appointments = _helperFunctions.LoadQueryToTable(query);

                List<string> upcomingAppointments = new List<string>();

                foreach (DataRow row in appointments.Rows)
                {
                    if (DateTime.TryParse(row["start"].ToString(), out DateTime startTimeUtc))
                    {
                        if (startTimeUtc >= now && startTimeUtc <= fifteenMinutesLater)
                        {
                            string title = row["title"].ToString();
                            string customerName = row["customerName"].ToString();
                            string message = $"{customerName}: '{title}' at {startTimeUtc.ToLocalTime():t}";
                            upcomingAppointments.Add(message);
                        }
                    }
                }

                if (upcomingAppointments.Count > 0)
                {
                    string alertMessage = string.Join("\n", upcomingAppointments);
                    MessageBox.Show(alertMessage, "Upcoming Appointments (Next 15 Minutes)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking upcoming appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ReloadCustomerData()
        {
            _helperFunctions.LoadDataGridData(_customerQuery, customerDataGrid);
            appointmentDataGrid.DataSource = null;
        }

        void RecordsForm_Load(object sender, EventArgs e)
        {
            AppointmentTimeChecker();
            ReloadCustomerData();
        }

        void addCustomerButton_Click(object sender, EventArgs e)
        {
            AddCustomer _addCustomer = new AddCustomer();
            _addCustomer.CustomerSaved += (s, args) => ReloadCustomerData();
            _addCustomer.Show();
        }

        void updateCustomerButton_Click(object sender, EventArgs e)
        {
            if (customerDataGrid.SelectedRows.Count == 0) return;
            DataRowView rowView = customerDataGrid.SelectedRows[0].DataBoundItem as DataRowView;
            if (rowView != null)
            {
                var _updateCustomer = new UpdateCustomer(rowView.Row);
                _updateCustomer.CustomerSaved += (s, args) => ReloadCustomerData();
                _updateCustomer.Show();
            }
        }

        void CustomerDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = customerDataGrid.SelectedRows.Count > 0;
            updateCustomerButton.Enabled = hasSelection;
            deleteCustomerButton.Enabled = hasSelection;
            appointmentAddButton.Enabled = hasSelection;
            appointmentUpdateButton.Enabled = false;

            if (!hasSelection)
            {
                appointmentDataGrid.DataSource = null;
                _selectedCustomerId = 0;
                return;
            }

            DataRowView rowView = customerDataGrid.SelectedRows[0].DataBoundItem as DataRowView;
            if (rowView != null)
            {
                _selectedCustomerId = Convert.ToInt32(rowView["customerId"]);
                ReloadCustomerAppointmentData(_selectedCustomerId);
            }
        }

        void AppointmentDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            appointmentUpdateButton.Enabled = appointmentDataGrid.SelectedRows.Count > 0;
            appointmentDeleteButton.Enabled = appointmentDataGrid.SelectedRows.Count > 0;
        }

        void deleteCustomerButton_Click(object sender, EventArgs e) => DeleteDataGridInfo(customerDataGrid);

        void appointmentAddButton_Click(object sender, EventArgs e)
        {
            if (!(customerDataGrid.SelectedRows[0].DataBoundItem is DataRowView rowView)) return;
            int customerId = Convert.ToInt32(rowView["customerId"]);

            DataTable customerTable = rowView.Row.Table.Clone();
            customerTable.ImportRow(rowView.Row);

            string appointmentQuery = $"SELECT * FROM appointment WHERE customerId = {customerId}";
            DataTable appointmentTable = _helperFunctions.LoadQueryToTable(appointmentQuery);
            var addAppointmentForm = new AddAppointment(customerId, customerTable, appointmentTable);
            addAppointmentForm.AppointmentAdded += (s, args) => { ReloadCustomerAppointmentData(customerId);};
            addAppointmentForm.Show();
        }

        void appointmentUpdateButton_Click(object sender, EventArgs e) 
        {
            int appointmentId = Convert.ToInt32(appointmentDataGrid.SelectedRows[0].Cells["appointmentId"].Value);
            int customerId = Convert.ToInt32(customerDataGrid.SelectedRows[0].Cells["customerId"].Value);

            UpdateAppointment _updateAppointment = new UpdateAppointment(appointmentId, customerId);
            _updateAppointment.AppointmentUpdated += (s, args) => { ReloadCustomerAppointmentData(customerId); };
            _updateAppointment.Show();
        }

        void appointmentDeleteButton_Click(object sender, EventArgs e) => DeleteDataGridInfo(appointmentDataGrid);

        void exitButton_Click(object sender, EventArgs e) => Application.Exit();

        void EndConnectionOnClose(object sender, FormClosedEventArgs e) => DBConnection.CloseConnection();

        void DeleteAttachedAppointment(int customerId, string deleteQuery)
        {
            try
            {
                if (DBConnection.IsOffline())
                {
                    using (var cmd = new SQLiteCommand(deleteQuery, DBConnection.OfflineConn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var cmd = new MySqlCommand(deleteQuery, DBConnection.Conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting appointments: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void DeleteDataGridInfo(DataGridView dataGridView)
        {
            string noSelectError = "", sqlTablePK = "", deleteMessage = "", sqlDeleteQuery = "", successMessage = "", reloadQuery = "", tableType = "";

            if (dataGridView == customerDataGrid)
            {
                noSelectError = "Please select a customer to delete.";
                sqlTablePK = "customerId";
                deleteMessage = "Are you sure you want to delete this customer? This action cannot be undone.";
                sqlDeleteQuery = "DELETE FROM customer WHERE customerId = @customerId";
                successMessage = "Customer deleted successfully.";
                reloadQuery = _customerQuery;
                tableType = "customer";
            }
            else if (dataGridView == appointmentDataGrid)
            {
                int currentCustomerId = Convert.ToInt32(customerDataGrid.SelectedRows[0].Cells["customerId"].Value);
                noSelectError = "Please select an appointment to delete.";
                sqlTablePK = "appointmentId";
                deleteMessage = "Are you sure you want to delete this appointment? This action cannot be undone.";
                sqlDeleteQuery = "DELETE FROM appointment WHERE appointmentId = @appointmentId";
                successMessage = "Appointment deleted successfully.";
                reloadQuery = $"SELECT * FROM appointment WHERE customerId = {currentCustomerId}";
                tableType = "appointment";
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show(noSelectError, "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[sqlTablePK].Value);
            if (MessageBox.Show(deleteMessage, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                if (tableType == "customer") DeleteAttachedAppointment(id, "DELETE FROM appointment WHERE customerId = @customerId");
                if (DBConnection.IsOffline())
                {
                    using (var cmd = new SQLiteCommand(sqlDeleteQuery, DBConnection.OfflineConn))
                    {
                        cmd.Parameters.AddWithValue($"@{sqlTablePK}", id);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _helperFunctions.LoadDataGridData(reloadQuery, dataGridView);
                        }
                        else MessageBox.Show("Deletion failed. It may not exist in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    using (var cmd = new MySqlCommand(sqlDeleteQuery, DBConnection.Conn))
                    {
                        cmd.Parameters.AddWithValue($"@{sqlTablePK}", id);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _helperFunctions.LoadDataGridData(reloadQuery, dataGridView);
                        }
                        else MessageBox.Show("Deletion failed. It may not exist in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting {tableType}: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
