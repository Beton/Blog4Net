using System;
using System.Configuration;

namespace Blog4Net.Web.Utility
{
    public static class Extensions
    {
        public static string ToConfigLocalTime(this DateTime utcDt)
        {
            var istTz = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);
            return String.Format("{0} ({1})", TimeZoneInfo.ConvertTimeFromUtc(utcDt, istTz).ToShortDateString(), ConfigurationManager.AppSettings["TimezoneAbbr"]);
        } 
    }
}