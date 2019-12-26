using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        public readonly double _hostingUnitKey;
        private Host _owner { get; set; }
        public Host Owner
        {
            set { _owner = value; }
        }

        private string _hostingUnitName { get; set; }
        public string HostingUnitName
        {
            set { _hostingUnitName = value; }
        }
        private Diary _diary { get; set; }

        public override string ToString()
        {
            string str = "Key: " + _hostingUnitKey;
            str += "\nOwner: " + _owner.ToString();
            str += "\nHosting unit name: " + _hostingUnitName;
            str += "\nDiary busy ranges:\n" + _diary.ToString();
            return str;
        }
    }
}