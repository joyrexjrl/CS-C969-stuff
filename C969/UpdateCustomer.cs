using C969.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace C969
{
    public partial class UpdateCustomer : Form
    {
        HelperFunctions _helperFunctions;
        DataRow _customerRow;

        public event EventHandler CustomerSaved;

        public UpdateCustomer(DataRow customerRow)
        {
            InitializeComponent();
            _customerRow = customerRow;
            _helperFunctions = new HelperFunctions();
            _helperFunctions.DataGridLayout(updateCustomerDataGrid);
            updateCustomerDataGrid.ClearSelection();
            SetActiveStatus();
            PopulateCustomerFields();
            updateCustomerPhoneNumber.KeyPress += CustomerPhoneNumber_KeyPress;
        }

        void customerUpdateBackButton_Click(object sender, EventArgs e) => Close();

        void customerUpdateClearButton_Click(object sender, EventArgs e)
        {
            TextBox[] _textBoxes = { updateCustomerName, updateCustomerAddressOne, updateCustomerAddressTwo, updateCustomerCity, updateCustomerPostCode, updateCustomerPhoneNumber, updateCustomerCountry };
            foreach (TextBox textBox in _textBoxes) textBox.Text = "";
            updateCustomerActiveStatus.SelectedIndex = 0;
        }

        void CustomerPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == '(' || e.KeyChar == ')' || e.KeyChar == '-') return;
            e.Handled = true;
        }

        string FormatPhoneNumber(string input)
        {
            var digits = new string(input.Where(char.IsDigit).ToArray());
            if (digits.Length != 10) throw new Exception("Phone number must be exactly 10 digits.");
            return $"({digits.Substring(0, 3)}){digits.Substring(3, 3)}-{digits.Substring(6, 4)}";
        }

        void customerUpdateButton_Click(object sender, EventArgs e)
        {
            if (updateCustomerName.Text == "" || updateCustomerPhoneNumber.Text == "" ||
                updateCustomerAddressOne.Text == "" || updateCustomerCity.Text == "" ||
                updateCustomerCountry.Text == "" || updateCustomerPostCode.Text == "")
            {
                MessageBox.Show("All fields must be filled out.");
                return;
            }            

            string name = updateCustomerName.Text;
            string address = updateCustomerAddressOne.Text;
            string address2 = updateCustomerAddressTwo.Text;
            string phone;
            try
            {
                phone = FormatPhoneNumber(updateCustomerPhoneNumber.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            string postal = updateCustomerPostCode.Text;
            string city = updateCustomerCity.Text;
            string country = updateCustomerCountry.Text;
            int active = updateCustomerActiveStatus.SelectedIndex;

            try
            {
                long customerId = Convert.ToInt64(_customerRow["customerId"]);
                string query = $@"
                    SELECT c.customerId, c.customerName, a.addressId, a.cityId, 
                    ci.countryId, a.address, a.address2, a.postalCode, a.phone, 
                    ci.city, co.country
                    FROM customer c
                    JOIN address a ON c.addressId = a.addressId
                    JOIN city ci ON a.cityId = ci.cityId
                    JOIN country co ON ci.countryId = co.countryId
                    WHERE c.customerId = {customerId}";

                if (DBConnection.IsOffline())
                {
                    if (DBConnection.OfflineConn == null || DBConnection.OfflineConn.State != ConnectionState.Open) DBConnection.StartOfflineConnection();

                    var connOffline = DBConnection.OfflineConn; // Using SQLite connection
                    using (var commandOffline = new SQLiteCommand(query, connOffline))
                    using (var readerOffline = commandOffline.ExecuteReader())
                    {
                        if (readerOffline.Read())
                        {
                            long addressId = Convert.ToInt64(readerOffline["addressId"]);
                            long cityId = Convert.ToInt64(readerOffline["cityId"]);
                            long countryId = Convert.ToInt64(readerOffline["countryId"]);

                            string updateCountry = $"UPDATE country SET country = '{country}', lastUpdate = datetime('now') WHERE countryId = {countryId}";
                            using (var command = new SQLiteCommand(updateCountry, connOffline))
                                command.ExecuteNonQuery();

                            string updateCity = $"UPDATE city SET city = '{city}', lastUpdate = datetime('now') WHERE cityId = {cityId}";
                            using (var command = new SQLiteCommand(updateCity, connOffline))
                                command.ExecuteNonQuery();

                            string updateAddress = $"UPDATE address SET address = '{address}', address2 = '{address2}', postalCode = '{postal}', phone = '{phone}', lastUpdate = datetime('now') WHERE addressId = {addressId}";
                            using (var command = new SQLiteCommand(updateAddress, connOffline))
                                command.ExecuteNonQuery();

                            string updateCustomer = $"UPDATE customer SET customerName = '{name}', active = {active}, lastUpdate = datetime('now') WHERE customerId = {customerId}";
                            using (var command = new SQLiteCommand(updateCustomer, connOffline))
                                command.ExecuteNonQuery();

                            MessageBox.Show("Customer updated in offline database.");
                            CustomerSaved?.Invoke(this, EventArgs.Empty);
                            Close();
                        }
                    }
                }
                else
                {
                    var connOnline = DBConnection.Conn; // Using MySQL connection
                    using (var commandOnline = new MySqlCommand(query, connOnline))
                    using (var readerOnline = commandOnline.ExecuteReader())
                    {
                        if (readerOnline.Read())
                        {
                            long addressId = Convert.ToInt64(readerOnline["addressId"]);
                            long cityId = Convert.ToInt64(readerOnline["cityId"]);
                            long countryId = Convert.ToInt64(readerOnline["countryId"]);

                            string updateCountry = $"UPDATE country SET country = '{country}', lastUpdate = now() WHERE countryId = {countryId}";
                            using (var command = new MySqlCommand(updateCountry, connOnline))
                                command.ExecuteNonQuery();

                            string updateCity = $"UPDATE city SET city = '{city}', lastUpdate = now() WHERE cityId = {cityId}";
                            using (var command = new MySqlCommand(updateCity, connOnline))
                                command.ExecuteNonQuery();

                            string updateAddress = $"UPDATE address SET address = '{address}', address2 = '{address2}', postalCode = '{postal}', phone = '{phone}', lastUpdate = now() WHERE addressId = {addressId}";
                            using (var command = new MySqlCommand(updateAddress, connOnline))
                                command.ExecuteNonQuery();

                            string updateCustomer = $"UPDATE customer SET customerName = '{name}', active = {active}, lastUpdate = now() WHERE customerId = {customerId}";
                            using (var command = new MySqlCommand(updateCustomer, connOnline))
                                command.ExecuteNonQuery();

                            MessageBox.Show("Customer updated in online database.");
                            CustomerSaved?.Invoke(this, EventArgs.Empty);
                            Close();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("MySQL Error: " + ex.Message);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("SQLite Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message);
            }            
        }

        void SetActiveStatus()
        {
            updateCustomerActiveStatus.Items.Add("Active");
            updateCustomerActiveStatus.Items.Add("Not Active");
            updateCustomerActiveStatus.SelectedIndex = 0;
        }

        void PopulateCustomerFields()
        {
            updateCustomerName.Text = _customerRow["customerName"].ToString();

            string activeStatus = _customerRow["active"].ToString();
            int activeValue = activeStatus == "Active" ? 0 : 1;
            updateCustomerActiveStatus.SelectedIndex = activeValue;

            int addressId = Convert.ToInt32(_customerRow["addressId"]);

            string query = $@"SELECT a.address, a.address2, a.postalCode, a.phone, 
                c.city, co.country 
                FROM address a
                JOIN city c ON a.cityId = c.cityId
                JOIN country co ON c.countryId = co.countryId
                WHERE a.addressId = {addressId}";

            if (DBConnection.IsOffline())
            {
                var offlineConnection = DBConnection.GetOfflineConnection();

                using (var cmd = new SQLiteCommand(query, offlineConnection))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        updateCustomerAddressOne.Text = reader["address"].ToString();
                        updateCustomerAddressTwo.Text = reader["address2"].ToString();
                        updateCustomerPostCode.Text = reader["postalCode"].ToString();
                        updateCustomerPhoneNumber.Text = reader["phone"].ToString();
                        updateCustomerCity.Text = reader["city"].ToString();
                        updateCustomerCountry.Text = reader["country"].ToString();
                    }
                }
            }
            else
            {
                if (!DBConnection.IsConnectionLive) DBConnection.StartConnection();

                using (var cmd = new MySqlCommand(query, DBConnection.Conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        updateCustomerAddressOne.Text = reader["address"].ToString();
                        updateCustomerAddressTwo.Text = reader["address2"].ToString();
                        updateCustomerPostCode.Text = reader["postalCode"].ToString();
                        updateCustomerPhoneNumber.Text = reader["phone"].ToString();
                        updateCustomerCity.Text = reader["city"].ToString();
                        updateCustomerCountry.Text = reader["country"].ToString();
                    }
                }
            }
        }
    }
}
