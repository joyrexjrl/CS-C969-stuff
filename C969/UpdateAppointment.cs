using C969.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace C969
{
    public partial class UpdateAppointment : Form
    {
        public event EventHandler AppointmentUpdated;
        HelperFunctions _helperFunctions;
        int _appointmentId;
        int _customerId;

        public UpdateAppointment(int appointmentId, int customerId)
        {
            InitializeComponent();
            _helperFunctions = new HelperFunctions();
            _appointmentId = appointmentId;
            _customerId = customerId;
            _helperFunctions.DataGridLayout(updateAppointmentBeingChanged);
            _helperFunctions.DataGridLayout(updateAppointmentList);

            Shown += UpdateAppointment_Shown;
        }

        void UpdateAppointment_Shown(object sender, EventArgs e) => LoadAppointmentData();

        void LoadAppointmentData()
        {
            string selectedQuery = $"SELECT * FROM appointment WHERE appointmentId = {_appointmentId}";
            string othersQuery = $"SELECT * FROM appointment WHERE customerId = {_customerId} AND appointmentId != {_appointmentId}";

            _helperFunctions.LoadDataGridData(selectedQuery, updateAppointmentBeingChanged);
            _helperFunctions.LoadDataGridData(othersQuery, updateAppointmentList);

            if (updateAppointmentBeingChanged.Rows.Count > 0)
            {
                var row = updateAppointmentBeingChanged.Rows[0];

                updateTitle.Text = row.Cells["title"].Value?.ToString();
                updateLocation.Text = row.Cells["location"].Value?.ToString();
                updateType.Text = row.Cells["type"].Value?.ToString();
                updateContact.Text = row.Cells["contact"].Value?.ToString();
                updateUrl.Text = row.Cells["url"].Value?.ToString();

                updateStart.Text = Convert.ToDateTime(row.Cells["start"].Value).ToString("HH:mm");
                updateEnd.Text = Convert.ToDateTime(row.Cells["end"].Value).ToString("HH:mm");
                updateDatePicker.Value = Convert.ToDateTime(row.Cells["start"].Value).Date;

                updateDescription.Text = row.Cells["description"].Value?.ToString();
            }
        }

        void clearButton_Click(object sender, EventArgs e)
        {
            TextBox[] _textBoxes = { updateTitle, updateLocation, updateType, updateContact,
                updateUrl, updateStart, updateEnd, updateDescription };
            foreach (TextBox textBox in _textBoxes) textBox.Text = "";
        }

        void backButton_Click(object sender, EventArgs e) => Close();

        void updateButton_Click(object sender, EventArgs e)
        {
            string title = updateTitle.Text.Trim();
            string location = updateLocation.Text.Trim();
            string type = updateType.Text.Trim();
            string contact = updateContact.Text.Trim();
            string url = updateUrl.Text.Trim();
            string description = updateDescription.Text.Trim();

            if (!TimeSpan.TryParse(updateStart.Text.Trim(), out TimeSpan startTimeOnly) ||
                !TimeSpan.TryParse(updateEnd.Text.Trim(), out TimeSpan endTimeOnly))
            {
                MessageBox.Show("Invalid start or end time format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime startTime = updateDatePicker.Value.Date + startTimeOnly;
            DateTime endTime = updateDatePicker.Value.Date + endTimeOnly;

            if (startTime >= endTime)
            {
                MessageBox.Show("End time must be after start time.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (HasTimeOverlap(startTime, endTime))
            {
                MessageBox.Show("Appointment overlaps with an existing one.", "Time Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string updateQuery = @"UPDATE appointment 
                           SET title = @title, location = @location, type = @type, contact = @contact, url = @url, 
                               description = @description, start = @start, end = @end, lastUpdate = CURRENT_TIMESTAMP, 
                               lastUpdateBy = @user 
                           WHERE appointmentId = @appointmentId";

            try
            {
                if (DBConnection.IsOffline())
                {
                    using (var cmd = new SQLiteCommand(updateQuery, DBConnection.OfflineConn))
                    {
                        AddUpdateParameters(cmd);
                        ExecuteUpdate(cmd);
                    }
                }
                else
                {
                    using (var cmd = new MySqlCommand(updateQuery, DBConnection.Conn))
                    {
                        AddUpdateParameters(cmd);
                        ExecuteUpdate(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            void AddUpdateParameters(dynamic cmd)
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@contact", contact);
                cmd.Parameters.AddWithValue("@url", url);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@start", startTime);
                cmd.Parameters.AddWithValue("@end", endTime);
                cmd.Parameters.AddWithValue("@user", Environment.UserName);
                cmd.Parameters.AddWithValue("@appointmentId", _appointmentId);
            }

            void ExecuteUpdate(dynamic cmd)
            {
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Appointment updated successfully.");
                    AppointmentUpdated?.Invoke(this, EventArgs.Empty);
                    Close();
                }
                else MessageBox.Show("No changes were made to the appointment.", "No Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        bool HasTimeOverlap(DateTime newStart, DateTime newEnd)
        {
            foreach (DataGridViewRow row in updateAppointmentList.Rows)
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
