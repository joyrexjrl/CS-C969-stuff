using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969
{
    public partial class ViewLogs : Form
    {
        public ViewLogs()
        {
            InitializeComponent();
            LoadLogs();
        }

        void logsBackButton_Click(object sender, EventArgs e) => Close();

        void LoadLogs()
        {
            viewLogsTextbox.Clear();

            if (File.Exists(Logger.LoggerFile))
            {
                string logContent = File.ReadAllText(Logger.LoggerFile);
                viewLogsTextbox.Text = logContent;
            }
            else viewLogsTextbox.Text = "Log file not found.";
        }
    }
}
