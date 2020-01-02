using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        // the following bool property is in order to make the next key
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        private bool _hostingUnitKey_setAlready = false;
        private double _hostingUnitKey;
        public double HostingUnitKey
        {
            get => _hostingUnitKey;
            set
            {
                if (!_hostingUnitKey_setAlready)
                {
                    _hostingUnitKey = value;
                    _hostingUnitKey_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.HostingUnit._hostingUnitKey property can only once change!");
            }
        }

        private Host _owner;
        public Host Owner
        {
            set => _owner = value;
            get => _owner;
        }

        private Enums.Area _area;
        public Enums.Area Area
        {
            set => _area = value;
            get => _area;
        }

        private Enums.UnitType _type { get; set; }
        public Enums.UnitType type
        {
            set => _type = value;
            get => _type;
        }

        private string _hostingUnitName;
        public string HostingUnitName
        {
            set => _hostingUnitName = value;
            get => _hostingUnitName;
        }

        private bool[,] _diary;
        public bool[,] Diary
        {
            set => _diary = value;
            get => _diary;
        }

        public override string ToString()
        {
            string str = "Hosting Unit Key:\t" + _hostingUnitKey;
            str += "\nUnit name:\t" + _hostingUnitName;
            str += "\nOwner:\n" + _owner.ToString();
            str += "\nUnit Area:\t" + _area;
            str += "\nDiary busy ranges:\n" + _diary.toString();
            return str;
        }
    }
}