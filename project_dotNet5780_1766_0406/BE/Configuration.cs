using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    using System.Reflection;
    public static class Configuration
    {
        /// <summary>
        /// return string in DD/MM/YYYY method
        /// </summary>
        /// <param name="time">DateTime to string</param>
        public static string toString(this DateTime time)
        {
            return time.ToString("d", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR").DateTimeFormat);
        }

        /// <summary>
        /// delegate to a term that work on BE.GuestRequest and return true
        /// if gReq meets the condition
        /// </summary>
        /// <param name="t">BE.GuestRequest to check</param>
        /// <returns>if gReq meets the condition</returns>
        public delegate bool Term<T>(T t) where T : new();

        public const int _days = 31;
        public const int _month = 12;
        public const int _daysInYear = 31 * 12;

        public const double Fee = 10;

        public const double _firstKey = 9999999;
        public static double GuestRequestKey;
        public static double HostKey;
        public static double HostingUnitKey;
        public static double OrderKey;
    }
}
