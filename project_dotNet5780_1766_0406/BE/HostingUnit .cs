using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// in the next part of the project we will make this class singleton
    public class HostingUnit
    {
        public readonly double _hostingUnitKey;

        private Host _owner;
        public Host Owner
        {
            set => _owner = value;
            get => _owner;
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
            string str = "Key: " + _hostingUnitKey;
            str += "\nOwner: " + _owner.ToString();
            str += "\nHosting unit name: " + _hostingUnitName;
            str += "\nDiary busy ranges:\n" + _diary.ToString();
            return str;
        }
    }
}