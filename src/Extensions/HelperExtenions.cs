using AzureDevopsTracker.Data;
using System;
using System.Linq;

namespace AzureDevopsTracker.Extensions
{
    public static class HelperExtenions
    {
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text?.Trim());
        }

        public static string Truncate(this string value, int maxLength = 8000)
        {
            if (value.IsNullOrEmpty()) return value;
            return value.Length <= maxLength ? value : value[..maxLength];
        }

        public static string ExtractEmail(this string user)
        {
            if (user is null)
                return user;

            if (!user.Contains(" <") && !user.TrimEnd().Contains('>'))
                return user;

            return user.Split('<').LastOrDefault()?.Split('>')?.FirstOrDefault();
        }

        public static string ToTextTime(this TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
                return $@"{timeSpan:%d} Day(s) {timeSpan:%h} h e {timeSpan:%m} min e {timeSpan:%s} s";

            if (timeSpan.Hours > 0)
                return $@"{timeSpan:%h} h e {timeSpan:%m} min e {timeSpan:%s} s";

            if (timeSpan.Minutes > 0)
                return $@"{timeSpan:%m} min e {timeSpan:%s} s";

            if (timeSpan.Seconds > 0)
                return $@"{timeSpan:%s} s";

            return "-";
        }

        public static DateTime ToDateTimeFromTimeZoneInfo(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, DataBaseConfig.TimeZoneInfo);
        }
    }
}