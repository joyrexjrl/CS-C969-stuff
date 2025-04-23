using C969.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;

namespace C969
{
    public partial class AddAppointment : Form
    {
        HelperFunctions _helperFunctions;
        readonly int _customerId;

        DateTime _openTime = DateTime.Parse("08:00");
        DateTime _closedTime = DateTime.Parse("17:00");

        public event EventHandler AppointmentAdded;

        public AddAppointment(int customerId, DataTable selectedCustomer, DataTable customerAppointments)
        {
            InitializeComponent();
            _customerId = customerId;
            _helperFunctions = new HelperFunctions();
            _helperFunctions.DataGridLayout(appointmentCustomerDatagrid);
            _helperFunctions.DataGridLayout(appointmentInfoDataGrid);
            _helperFunctions.ConfigureAppointmentColumns(appointmentInfoDataGrid);

            appointmentCustomerDatagrid.DataSource = selectedCustomer;            
            appointmentInfoDataGrid.DataSource = customerAppointments;

            Shown += (s, e) =>
            {
                appointmentCustomerDatagrid.ClearSelection();
                appointmentInfoDataGrid.ClearSelection();
            };
        }

        void appointmentBackButton_Click(object sender, EventArgs e) => Close();

        void appointmentClearButton_Click(object sender, EventArgs e)
        {
            TextBox[] _textBoxes = { appointmentTitleTextBox, appointmentLocationTextBox, appointmentTypeTextBox, appointmentContactTextBox,
                appointmentURLTextBox, appointmentStartTextBox, appointmentEndTextBox, appointmentDescriptionTextBox };
            foreach (TextBox textBox in _textBoxes)
            {
                textBox.Text = "";
            }
        }

        void appointmentStartTextBox_TextChanged(object sender, EventArgs e) => ValidateTimeInput(appointmentStartTextBox);

        void appointmentEndTextBox_TextChanged(object sender, EventArgs e) => ValidateTimeInput(appointmentEndTextBox);

        void ValidateTimeInput(TextBox textBox)
        {
            string input = textBox.Text;
            if (input.Any(c => !char.IsDigit(c) && c != ':'))
            {
                textBox.Text = string.Concat(input.Where(c => char.IsDigit(c) || c == ':'));
                textBox.SelectionStart = textBox.Text.Length;
            }

            if (input.Length == 4 && input.All(char.IsDigit))
            {
                input = input.Insert(2, ":");
                textBox.Text = input;
                textBox.SelectionStart = textBox.Text.Length;
            }

            if (input.Length != 5 || !DateTime.TryParseExact(input, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
            {
                textBox.Clear();
                MessageBox.Show("Please enter a valid time in the 24-hour format (HH:mm), e.g., 14:30.", "Invalid Time Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox.Text = input;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        bool TryParseNormalizedTime(string input, out DateTime result)
        {
            result = default;
            input = input.Trim();

            if (input.Length == 4 && input.All(char.IsDigit)) input = input.Insert(2, ":");
            return DateTime.TryParseExact(input, "HH:mm", null, System.Globalization.DateTimeStyles.None, out result);
        }

        void appointmentAddButton_Click(object sender, EventArgs e)
        {
            int userId = DBConnection.UserId;
            string createdBy = "system";
            DateTime now = DateTime.UtcNow;

            string title = appointmentTitleTextBox.Text.Trim();
            string location = appointmentLocationTextBox.Text.Trim();
            string type = appointmentTypeTextBox.Text.Trim();
            string contact = appointmentContactTextBox.Text.Trim();
            string url = appointmentURLTextBox.Text.Trim();
            string description = appointmentDescriptionTextBox.Text.Trim();

            if (!TryParseNormalizedTime(appointmentStartTextBox.Text, out DateTime start) ||
                !TryParseNormalizedTime(appointmentEndTextBox.Text, out DateTime end))
            {
                MessageBox.Show("Invalid date/time format for start or end.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (start < _openTime || start > _closedTime || end < _openTime || end > _closedTime)
            {
                MessageBox.Show($"Please enter times within the operating hours: {_openTime:HH:mm} - {_closedTime:HH:mm}.",
                                "Time Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                appointmentStartTextBox.Clear();
                appointmentEndTextBox.Clear();
                return;
            }

            if (HasTimeOverlap(start, end))
            {
                MessageBox.Show("This appointment overlaps with an existing one. Please choose a different time.", "Time Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                appointmentStartTextBox.Clear();
                appointmentEndTextBox.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(contact) ||
                string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string insertQuery = $@"
                INSERT INTO appointment 
                (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES (
                    {_customerId}, {userId}, '{Escape(title)}', '{Escape(description)}', '{Escape(location)}', '{Escape(contact)}', 
                    '{Escape(type)}', '{Escape(url)}', '{start:yyyy-MM-dd HH:mm:ss}', '{end:yyyy-MM-dd HH:mm:ss}', '{now:yyyy-MM-dd HH:mm:ss}', 
                    '{Escape(createdBy)}', '{now:yyyy-MM-dd HH:mm:ss}', '{Escape(createdBy)}')";

            try
            {
                if (DBConnection.IsOffline())
                {
                    using (var cmd = new SQLiteCommand(insertQuery, DBConnection.OfflineConn))
                        cmd.ExecuteNonQuery();
                }
                else
                {
                    using (var cmd = new MySqlCommand(insertQuery, DBConnection.Conn))
                        cmd.ExecuteNonQuery();
                }

                AppointmentAdded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Appointment added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding appointment: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string Escape(string input) => input.Replace("'", "''");
        }

        bool HasTimeOverlap(DateTime newStart, DateTime newEnd)
        {
            foreach (DataGridViewRow row in appointmentInfoDataGrid.Rows)
            {
                if (row.DataBoundItem is DataRowView dataRow)
                {
                    DateTime existingStart = Convert.ToDateTime(dataRow["start"]);
                    DateTime existingEnd = Convert.ToDateTime(dataRow["end"]);

                    if (newStart < existingEnd && newEnd > existingStart) return true;
                }
            }
            return false;
        }
    }
}
