using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        public readonly double _hostingUnitKey = ++BE.Configuration.HostingUnitKey;

        private Host _owner;
        public Host Owner
        {
            set => _owner = value;
            get => _owner;
        }

        private Enums.Area _area;
        public Enums.Area Area
        {
            set
            {
                _area = value;
            }
            get => _area;
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
        /// <summary>
        /// swap to string
        /// </summary>
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