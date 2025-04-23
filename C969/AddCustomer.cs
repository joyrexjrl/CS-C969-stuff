using C969.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace C969
{
    public partial class AddCustomer : Form
    {
        HelperFunctions _helperFunctions;
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
        string _user;

        public event EventHandler CustomerSaved;

        public AddCustomer()
        {
            InitializeComponent();
            _helperFunctions = new HelperFunctions();
            _helperFunctions.DataGridLayout(addCustomerDataGrid);
            _user = DBConnection.UserName;
            SetActiveStatus();
            Load += AddCustomer_Load;
        }

        void AddCustomer_Load(object sender, EventArgs e) => _helperFunctions.LoadDataGridData(_customerQuery, addCustomerDataGrid);

        void CustomerAddButton_Click(object sender, EventArgs e)
        {
            if (customerInfoName.Text == "" || customerPhoneNumber.Text == "" || customerAddressOne.Text == "" || 
                customerAddressCity.Text == "" || customerAddressCountry.Text == "" || customerPostCode.Text == "")
            {
                MessageBox.Show("All fields must be filled out.");
                return;
            }

            string name = customerInfoName.Text;
            string address = customerAddressOne.Text;
            string phone = customerPhoneNumber.Text;
            string postal = customerPostCode.Text;
            string city = customerAddressCity.Text;
            string country = customerAddressCountry.Text;
            int active = customerActiveStatus.SelectedIndex;

            if (DBConnection.IsOffline())
            {
                try
                {
                    var conn = DBConnection.OfflineConn;

                    // Insert Country
                    string addCountry = $"INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                        $"VALUES ('{country}', datetime('now'), '{_user}', datetime('now'), '{_user}')";
                    var countryCmd = new SQLiteCommand(addCountry, conn);
                    countryCmd.ExecuteNonQuery();
                    long countryId = conn.LastInsertRowId;

                    // Insert City
                    string addCity = $"INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                     $"VALUES ('{city}', {countryId}, datetime('now'), '{_user}', datetime('now'), '{_user}')";
                    var cityCmd = new SQLiteCommand(addCity, conn);
                    cityCmd.ExecuteNonQuery();
                    long cityId = conn.LastInsertRowId;

                    // Insert Address
                    string addAddress = $"INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                        $"VALUES ('{address}', '', {cityId}, '{postal}', '{phone}', datetime('now'), '{_user}', datetime('now'), '{_user}')";
                    var addrCmd = new SQLiteCommand(addAddress, conn);
                    addrCmd.ExecuteNonQuery();
                    long addressId = conn.LastInsertRowId;

                    // Insert Customer
                    string addCustomer = $"INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                         $"VALUES ('{name}', {addressId}, {active}, datetime('now'), '{_user}', datetime('now'), '{_user}')";
                    var custCmd = new SQLiteCommand(addCustomer, conn);
                    custCmd.ExecuteNonQuery();

                    MessageBox.Show("Customer added to offline database.");

                    addCustomerDataGrid.DataSource = null;
                    _helperFunctions.LoadDataGridData(_customerQuery, addCustomerDataGrid);
                    ClearCustomerFields();
                    CustomerSaved?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting into offline database:\n" + ex.Message);
                }
            }
            else
            {
                try
                {
                    var conn = DBConnection.Conn;

                    string addCountry = $"INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                        $"VALUES ('{country}', now(), '{_user}', now(), '{_user}')";
                    var countryCmd = new MySqlCommand(addCountry, conn);
                    countryCmd.ExecuteNonQuery();
                    long countryId = countryCmd.LastInsertedId;

                    string addCity = $"INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                     $"VALUES ('{city}', {countryId}, now(), '{_user}', now(), '{_user}')";
                    var cityCmd = new MySqlCommand(addCity, conn);
                    cityCmd.ExecuteNonQuery();
                    long cityId = cityCmd.LastInsertedId;

                    string addAddress = $"INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                        $"VALUES ('{address}', '', {cityId}, '{postal}', '{phone}', now(), '{_user}', now(), '{_user}')";
                    var addrCmd = new MySqlCommand(addAddress, conn);
                    addrCmd.ExecuteNonQuery();
                    long addressId = addrCmd.LastInsertedId;

                    string addCustomer = $"INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                         $"VALUES ('{name}', {addressId}, {active}, now(), '{_user}', now(), '{_user}')";
                    var custCmd = new MySqlCommand(addCustomer, conn);
                    custCmd.ExecuteNonQuery();

                    MessageBox.Show("Customer added to online database.");

                    addCustomerDataGrid.DataSource = null;
                    _helperFunctions.LoadDataGridData(_customerQuery, addCustomerDataGrid);
                    ClearCustomerFields();
                    CustomerSaved?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting into online database:\n" + ex.Message);
                }
            }            
        }

        void CustomerClearButton_Click(object sender, EventArgs e) => ClearCustomerFields();

        void CustomerBackButton_Click(object sender, EventArgs e) => Hide();

        void ClearCustomerFields()
        {
            TextBox[] _textBoxes = {
                customerInfoName,
                customerAddressOne,
                customerAddressTwo,
                customerAddressCity,
                customerPostCode,
                customerPhoneNumber,
                customerAddressCountry
            };

            foreach (TextBox textBox in _textBoxes) textBox.Text = "";
            customerActiveStatus.SelectedIndex = 0;
        }

        void SetActiveStatus()
        {
            customerActiveStatus.Items.Add("Active");
            customerActiveStatus.Items.Add("Not Active");
            customerActiveStatus.SelectedIndex = 0;
        }
    }
}
