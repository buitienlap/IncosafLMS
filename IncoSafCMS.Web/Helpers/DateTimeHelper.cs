using System;

namespace IncosafCMS.Web.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo VietnamTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        /// <summary>
        /// Returns the current date and time in Vietnam timezone (Asia/Ho_Chi_Minh, UTC+7).
        /// </summary>
        public static DateTime VietnamNow
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VietnamTimeZone); }
        }

        /// <summary>
        /// Converts a UTC DateTime to Vietnam timezone.
        /// If the DateTime.Kind is Unspecified (e.g. from EF), it is assumed to be UTC.
        /// </summary>
        public static DateTime ToVietnamTime(this DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Local)
                utcDateTime = utcDateTime.ToUniversalTime();
            else if (utcDateTime.Kind == DateTimeKind.Unspecified)
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, VietnamTimeZone);
        }
    }
}
