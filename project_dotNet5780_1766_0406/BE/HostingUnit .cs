using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    using System.Windows.Controls;
    public class HostingUnit
    {
        // the following bool property is in order to make the next key
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        [XmlIgnore]
        private bool _hostingUnitKey_setAlready = false;
        [XmlIgnore]
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

        public Host Owner { get; set; }

        public Enums.Area Area { get; set; }

        public Enums.UnitType type { get; set; }

        public string HostingUnitName { get; set; }

        public List<CalendarDateRange> DatesRange { get; set; }

        /*public string XmlDiary
        {
            get
            {
                if (Diary == null)
                    return "";

                int sizeA = Diary.GetLength(0);
                int sizeB = Diary.GetLength(1);

                string result = sizeA + "," + sizeB;
                for (int i = 0; i < sizeA; i++)
                    for (int j = 0; j < sizeB; j++)
                        result += "," + Diary[i, j].ToString();

                return result;
            }
            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');
                    int sizeA = int.Parse(values[0]);
                    int sizeB = int.Parse(values[1]);
                    Diary = new bool[sizeA, sizeB];
                    int index = 2;
                    for (int i = 0; i < sizeA; i++)
                        for (int j = 0; j < sizeB; j++)
                            Diary[i, j] = bool.Parse(values[index++]);
                }
            }
        }
        [XmlIgnore]
        public bool[,] Diary { get; set; }*/

    }
}