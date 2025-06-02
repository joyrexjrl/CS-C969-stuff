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

        static Logger()
        {
            try
            {
                Directory.CreateDirectory(LoggerPath);
                if(!File.Exists(LoggerFile)) using (File.Create(LoggerFile)) { }
            }
            catch(Exception ex) 
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
            catch (Exception err)
            {
                MessageBox.Show($"Logging error: {err.Message}");
            }
        }

        public static void LogLogin(string username, bool success, bool isOffline)
        {
            string status = success ? "logged in" : "failed login attempt";
            string mode = isOffline ? "[OFFLINE]" : "[ONLINE]";
            DateTime time = DBConnection.GetNowTime();
            Log($"{mode} User {username} {status} at {time}");
        }

        public static void LogCustomerChange(string action, string customerName)
        {
            Log($"[CUSTOMER] {action} - {customerName}");
        }

        public static void LogAppointmentChange(string action, string appointmentDetails)
        {
            Log($"[APPOINTMENT] {action} - {appointmentDetails}");
        }
    }
}
