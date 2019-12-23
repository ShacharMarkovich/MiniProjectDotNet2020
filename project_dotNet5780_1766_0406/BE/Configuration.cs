using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public static class Configuration
    {
        private const double _firstKey = 9999999;
        public static double GuestRequestKey = _firstKey;
        public static double BankNumber = _firstKey;
        public static double HostKey = _firstKey;
        public static double HostingUnitKey = _firstKey;
        public static double OrderKey = _firstKey;

    }
}
