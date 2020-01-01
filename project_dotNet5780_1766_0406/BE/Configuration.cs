using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
        public static string toString(this DateTime time)
        {
            return time.ToString("d", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR").DateTimeFormat);
        }

        public static string toString(this bool[,] diary)
        {
            string toString = "";
            bool count = false;
            for (int month = 0; month < _month; month++) //run through months
            {
                for (int day = 0; day < _days; day++)   //run through days
                {
                    if (diary[month, day] == true && count == false) //if yesterday not Busy and today Busy
                    {
                        count = true;
                        toString += (day + 1) + "." + (month + 1) + " - ";
                    }

                    if (diary[month, day] == false && count == true) //if today not Busy and yesterday Busy 
                    {
                        count = false;
                        if (day == 0) // Hosting end on the last day of month
                            toString += _days + "." + month + "\n";
                        else
                            toString += _days + "." + (month + 1) + "\n";
                    }

                    if (month + 1 == _month && day + 1 == _days && diary[month, day] == true)// last day of year
                    {
                        count = false;
                        toString += _days + "." + _month + "\n";
                    }
                }
            }
            return (toString == "" ? "None" : toString);
        }

        /// <summary>
        /// delegate to a term that work on BE.GuestRequest and return true
        /// if gReq meets the condition
        /// </summary>
        /// <param name="gReq">BE.GuestRequest to check</param>
        /// <returns>if gReq meets the condition</returns>
        public delegate bool Term(BE.GuestRequest gReq);

        public const int _days = 31;
        public const int _month = 12;
            
        public const double Fee = 10;

        private const double _firstKey = 9999999;
        public static double GuestRequestKey = _firstKey;
        public static double BankNumber = _firstKey;
        public static double HostKey = _firstKey;
        public static double HostingUnitKey = _firstKey;
        public static double OrderKey = _firstKey;
    }
}
