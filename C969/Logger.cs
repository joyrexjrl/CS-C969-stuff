using C969.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969
{
    public static class Logger
    {
        public static readonly string LoggerPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources"));
        public static readonly string LoggerFile = Path.Combine(LoggerPath, "userLog.txt");

        static Logger() => EnsureLogFileExists();

        public static void EnsureLogFileExists()
        {
            try
            {
                Directory.CreateDirectory(LoggerPath);
                if (!File.Exists(LoggerFile))
                    using (File.Create(LoggerFile)) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize log file: {ex.Message}");
            }
        }

        public static void Log(string message)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(LoggerFile, true))
                {
                    outputFile.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Logging error: {ex.Message}");
            }
        }

        public static void LogLogin(string username, bool success, bool isOffline)
        {
            string status = success ? "logged in" : "failed login attempt";
            string mode = isOffline ? "[OFFLINE]" : "[ONLINE]";
            DateTime time = TimeHelper.GetNowTime();
            Log($"{mode} User {username} {status} at {time}");
        }

        public static void LogCustomerChange(string action, string customerName, string username, bool isOffline)
        {
            string mode = isOffline ? "[OFFLINE]" : "[ONLINE]";
            string timestamp = TimeHelper.GetNowTime().ToString("u"); // ISO 8601 UTC format
            Log($"{mode} [CUSTOMER] {action} - {customerName} by {username} at {timestamp}");
        }

        public static void LogAppointmentChange(string action, string appointmentDetails, string username, bool isOffline)
        {
            string mode = isOffline ? "[OFFLINE]" : "[ONLINE]";
            string timestamp = TimeHelper.GetNowTime().ToString("u"); // ISO 8601 UTC format
            Log($"{mode} [APPOINTMENT] {action} - {appointmentDetails} by {username} at {timestamp}");
        }
    }
}
