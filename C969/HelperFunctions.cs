using C969.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace C969
{
    class HelperFunctions
    {
        public void DataGridLayout(DataGridView dataGrid)
        {
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.MultiSelect = false;
            dataGrid.ReadOnly = true;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.RowHeadersVisible = false;
            dataGrid.TabStop = false;

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;            
        }

        public void ConfigureAppointmentColumns(DataGridView dataGrid)
        {
            var columnsToShow = new string[]
            {
            "title", "location", "type", "url", "contact", "start", "end", "description"
            };

            foreach (DataGridViewColumn column in dataGrid.Columns) column.Visible = false;
            foreach (var columnName in columnsToShow)
            {
                var column = dataGrid.Columns[columnName];
                if (column != null) column.Visible = true;
            }
        }

        public void LoadDataGridData(string query, DataGridView dataGrid)
        {
            if (DBConnection.CurrentMode == ConnectionMode.None)
            {
                MessageBox.Show("Not connected to any database.");
                return;
            }
            try
            {
                DataTable dataTable = new DataTable();

                switch (DBConnection.CurrentMode)
                {
                    case ConnectionMode.Offline:
                        using (var cmd = new SQLiteCommand(query, DBConnection.OfflineConn))
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                        break;
                    case ConnectionMode.Online:
                        using (var cmd = new MySqlCommand(query, DBConnection.Conn))
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                        break;
                }
                dataGrid.DataSource = dataTable;
                dataGrid.BeginInvoke(new Action(() =>
                {
                    dataGrid.ClearSelection();
                    dataGrid.CurrentCell = null;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        public DataTable LoadQueryToTable(string query)
        {
            var table = new DataTable();
            try
            {
                if (DBConnection.IsOffline())
                {
                    using (var cmd = new SQLiteCommand(query, DBConnection.OfflineConn))
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
                else
                {
                    using (var cmd = new MySqlCommand(query, DBConnection.Conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load table: {ex.Message}", "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return table;
        }
    }
}
