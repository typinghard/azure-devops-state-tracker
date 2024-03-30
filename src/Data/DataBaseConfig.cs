using AzureDevopsTracker.Extensions;
using System;
using System.Runtime.InteropServices;

namespace AzureDevopsTracker.Data
{
    public class DataBaseConfig
    {
        public DataBaseConfig(string connectionsString, string schemaName = "dbo", TimeZoneInfo timeZoneInfo = null)
        {
            if (connectionsString.IsNullOrEmpty())
                throw new ArgumentException("The ConnectionsString is required");

            if (timeZoneInfo is null)
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                else
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Brazil/East");

            ConnectionsString = connectionsString;
            SchemaName = schemaName;
            TimeZoneInfo = timeZoneInfo;
        }

        public static string ConnectionsString { get; private set; }
        public static string SchemaName { get; private set; }
        public static TimeZoneInfo TimeZoneInfo { get; private set; }
    }
}