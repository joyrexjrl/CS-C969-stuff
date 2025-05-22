using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System;

namespace C969.Database
{
    class DBConnection
    {
        public static ConnectionMode CurrentMode { get; private set; } = ConnectionMode.None;

        public static bool IsConnectionLive => Conn != null && Conn.State == ConnectionState.Open;

        public static MySqlConnection Conn { get; set; }
        public static SQLiteConnection OfflineConn => _offlineConn;
        public static SQLiteConnection GetOfflineConnection()
        {
            if (_offlineConn == null || _offlineConn.State == ConnectionState.Closed || _offlineConnDisposed) StartOfflineConnection();
            return _offlineConn;
        }

        public static int UserId { get; set; } = -1;
        public static string UserName { get; set; }
        public static bool IsConnected { get; set; }
        public static bool IsOffline() => CurrentMode == ConnectionMode.Offline;
        public static TimeZoneInfo timeZone = TimeZoneInfo.Local;

        static bool _offlineConnDisposed = false;
        static SQLiteConnection _offlineConn;

        public static bool LoginUser(string username, string password)
        {
            try
            {
                string query = $"SELECT userId FROM user WHERE userName = '{username}' AND password = '{password}'";

                using (var cmd = new MySqlCommand(query, Conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserId = reader.GetInt32("userId");
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void StartConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                Conn = new MySqlConnection(connectionString);
                Conn.Open();
                IsConnected = true;
                CurrentMode = ConnectionMode.Online;
                MessageBox.Show("Connected to online database.");
            }
            catch (MySqlException)
            {
                MessageBox.Show("Online connection failed. Switching to offline database...");
                StartOfflineConnection();
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if (Conn != null)
                {
                    Conn.Close();
                    Conn.Dispose();
                    Conn = null;
                }
                if (_offlineConn != null && !_offlineConnDisposed)
                {
                    try
                    {
                        _offlineConn.Close();
                        _offlineConn.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Already disposed, do nothing
                    }
                    finally
                    {
                        _offlineConnDisposed = true;
                        _offlineConn = null;
                    }
                }
                IsConnected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closing connections: " + ex.Message);
            }
        }

        public static void StartOfflineConnection()
        {
            string dbPath = Path.Combine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")), "Resources", "OfflineDatabase.db");

            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                MessageBox.Show("Offline database created.");
                CreateTablesForOfflineMode(dbPath);
            }

            if (_offlineConn == null || _offlineConn.State == ConnectionState.Closed || _offlineConnDisposed)
            {
                _offlineConn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                _offlineConn.Open();
                _offlineConnDisposed = false;
                CurrentMode = ConnectionMode.Offline;
                IsConnected = true;
                MessageBox.Show("Connected to offline server.");
            }
        }

        public static void CreateTablesForOfflineMode(string dbPath)
        {
            using (var sqliteConn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                sqliteConn.Open();
                using (var cmd = new SQLiteCommand(sqliteConn))
                {
                    string[] tableCommands = {
                    // Country
                    @"CREATE TABLE IF NOT EXISTS country (
                        countryId INTEGER PRIMARY KEY,
                        country TEXT,
                        createDate DATETIME,
                        createdBy TEXT,
                        lastUpdate TIMESTAMP,
                        lastUpdateBy TEXT
                    );",

                    // City
                    @"CREATE TABLE IF NOT EXISTS city (
                        cityId INTEGER PRIMARY KEY,
                        city TEXT,
                        countryId INTEGER,
                        createDate DATETIME,
                        createdBy TEXT,
                        lastUpdate TIMESTAMP,
                        lastUpdatedBy TEXT,
                        FOREIGN KEY(countryId) REFERENCES country(countryId)
                    );",

                    // Address
                    @"CREATE TABLE IF NOT EXISTS address (
                        addressId INTEGER PRIMARY KEY,
                        address TEXT,
                        address2 TEXT,
                        cityId INTEGER,
                        postalCode TEXT,
                        phone TEXT,
                        createDate DATETIME,
                        createdBy TEXT,
                        lastUpdate TIMESTAMP,
                        lastUpdateBy TEXT,
                        FOREIGN KEY(cityId) REFERENCES city(cityId)
                    );",

                    // Customer
                    @"CREATE TABLE IF NOT EXISTS customer (
                        customerId INTEGER PRIMARY KEY,
                        customerName TEXT,
                        addressId INTEGER,
                        active INTEGER,
                        createDate DATETIME,
                        createdBy TEXT,
                        lastUpdate TIMESTAMP,
                        lastUpdateBy TEXT,
                        FOREIGN KEY(addressId) REFERENCES address(addressId)
                    );",

                    // User
                    @"CREATE TABLE IF NOT EXISTS user (
                        userId INTEGER PRIMARY KEY,
                        userName TEXT,
                        password TEXT,
                        active INTEGER,
                        createDate DATETIME,
                        createdBy TEXT,
                        lastUpdate TIMESTAMP,
                        lastUpdateBy TEXT
                    );",

                    // Appointment
                    @"CREATE TABLE IF NOT EXISTS appointment (
                        appointmentId INTEGER PRIMARY KEY,
                        customerId INTEGER,
                        userId INTEGER,
                        title TEXT,
                        description TEXT,
                        location TEXT,
                        contact TEXT,
                        type TEXT,
                        url TEXT,
                        start DATETIME,
                        end DATETIME,
                        createDate DATETIME,
                        createdBy TEXT,
                        lastUpdate TIMESTAMP,
                        lastUpdateBy TEXT,
                        FOREIGN KEY(customerId) REFERENCES customer(customerId),
                        FOREIGN KEY(userId) REFERENCES user(userId)
                    );"
                };

                foreach (var query in tableCommands)
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
                MessageBox.Show("Tables created or already exist.");
            }
        }
    }

    public enum ConnectionMode
    {
        Offline,
        Online,
        None
    }
}
