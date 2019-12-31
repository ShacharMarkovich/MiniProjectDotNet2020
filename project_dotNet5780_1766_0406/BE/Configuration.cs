using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
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
