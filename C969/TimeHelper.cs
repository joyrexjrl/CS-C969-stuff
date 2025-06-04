using C969.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969
{
    public static class TimeHelper
    {
        public static TimeZoneInfo AppTimeZone { get; set; } = TimeZoneInfo.Local;
        public static DateTime GetNowTime() => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AppTimeZone);

        public static DateTime openTime = DateTime.Parse("08:00");
        public static DateTime closedTime = DateTime.Parse("17:00");

        public static DateTime ToUtc(DateTime localTime)
        {
            if (localTime.Kind == DateTimeKind.Utc) return localTime;
            DateTime unspecified = DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecified, AppTimeZone);
        }

        public static DateTime ToLocal(DateTime utcTime) => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcTime, DateTimeKind.Utc), AppTimeZone);

        public static void ConvertUtcColumnsToLocal(DataTable table, params string[] columns)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (string col in columns)
                {
                    if (row[col] is DateTime utcTime) row[col] = ToLocal(utcTime);
                }
            }
        }

        public static bool TryParseNormalizedTime(string input, out DateTime result)
        {
            result = default;
            input = input.Trim();

            if (input.Length == 4 && input.All(char.IsDigit)) input = input.Insert(2, ":");
            return DateTime.TryParseExact(input, "HH:mm", null, System.Globalization.DateTimeStyles.None, out result);
        }

        public static bool HasTimeOverlap(DateTime newStart, DateTime newEnd, DataGridView dataGrid)
        {
            DateTime newStartUtc = ToUtc(newStart);
            DateTime newEndUtc = ToUtc(newEnd);

            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (row.DataBoundItem is DataRowView dataRow)
                {
                    DateTime existingStartUtc = Convert.ToDateTime(dataRow["start"]);
                    DateTime existingEndUtc = Convert.ToDateTime(dataRow["end"]);

                    if (existingStartUtc == default || existingEndUtc == default) continue;
                    if (existingStartUtc.Date == newStartUtc.Date)
                    {
                        if (newStartUtc < existingEndUtc && newEndUtc > existingStartUtc) return true;
                    }
                }
            }
            return false;
        }
    }
}
