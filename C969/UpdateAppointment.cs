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
        string _user;

        public UpdateAppointment(int appointmentId, int customerId)
        {
            InitializeComponent();
            _helperFunctions = new HelperFunctions();
            _appointmentId = appointmentId;
            _user = DBConnection.UserName;
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
            int userId = DBConnection.UserId;
            string createdBy = "system";
            DateTime utcNow = TimeHelper.ToUtc(TimeHelper.GetNowTime());

            string title = updateTitle.Text.Trim();
            string location = updateLocation.Text.Trim();
            string type = updateType.Text.Trim();
            string contact = updateContact.Text.Trim();
            string url = updateUrl.Text.Trim();
            string description = updateDescription.Text.Trim();

            if (!TimeHelper.TryParseNormalizedTime(updateStart.Text, out DateTime startTime) ||
                !TimeHelper.TryParseNormalizedTime(updateEnd.Text, out DateTime endTime))
            {
                MessageBox.Show("Invalid time format for start or end.", "Time Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime selectedDate = updateDatePicker.Value.Date;
            DateTime localStart = selectedDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
            DateTime localEnd = selectedDate.AddHours(endTime.Hour).AddMinutes(endTime.Minute);

            if (localStart.TimeOfDay < TimeHelper.openTime.TimeOfDay || localStart.TimeOfDay > TimeHelper.closedTime.TimeOfDay ||
                localEnd.TimeOfDay < TimeHelper.openTime.TimeOfDay || localEnd.TimeOfDay > TimeHelper.closedTime.TimeOfDay)
            {
                MessageBox.Show($"Please enter times within operating hours: {TimeHelper.openTime:HH:mm} - {TimeHelper.closedTime:HH:mm}.", "Time Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                updateStart.Clear();
                updateEnd.Clear();
                return;
            }

            DateTime utcStart = TimeHelper.ToUtc(localStart);
            DateTime utcEnd = TimeHelper.ToUtc(localEnd);

            if (TimeHelper.HasTimeOverlap(utcStart, utcEnd, updateAppointmentList))
            {
                MessageBox.Show("Appointment overlaps with an existing one.", "Time Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                updateStart.Clear();
                updateEnd.Clear();
                return;
            }

            string updateQuery = $@"
                UPDATE appointment
                SET title = '{Escape(title)}', location = '{Escape(location)}', type = '{Escape(type)}', contact = '{Escape(contact)}', url = '{Escape(url)}',
                    description = '{Escape(description)}', start = '{utcStart:yyyy-MM-dd HH:mm:ss}', end = '{utcEnd:yyyy-MM-dd HH:mm:ss}',
                    lastUpdate = '{utcNow:yyyy-MM-dd HH:mm:ss}', lastUpdateBy = '{Escape(createdBy)}'
                WHERE appointmentId = {_appointmentId};";

            try
            {
                if (DBConnection.IsOffline())
                {
                    using (var cmd = new SQLiteCommand(updateQuery, DBConnection.OfflineConn)) cmd.ExecuteNonQuery();
                    Logger.LogAppointmentChange("Updated", "Appointment updated to offline database.", _user, DBConnection.IsOffline());
                }
                else
                {
                    using (var cmd = new MySqlCommand(updateQuery, DBConnection.Conn)) cmd.ExecuteNonQuery();
                    Logger.LogAppointmentChange("Updated", "Appointment updated to online database.", _user, DBConnection.IsOffline());
                }
                AppointmentUpdated?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Appointment updated successfully.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string Escape(string input) => input.Replace("'", "''");
        }
    }
}
