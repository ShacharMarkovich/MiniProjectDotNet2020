using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class GuestRequest
    {
        // the following bool property is in order to make the next key
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        [XmlIgnore]
        private bool _guestRequestKey_setAlready = false;
        [XmlIgnore]
        private double _guestRequestKey;
        public double GuestRequestKey
        {
            get => _guestRequestKey;
            set
            {
                if (!_guestRequestKey_setAlready)
                {
                    _guestRequestKey = value;
                    _guestRequestKey_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.GuestRequest._guestRequestKey property can only once change!");
            }
        }

        public string PrivateName { get; set; }

        public string FamilyName { get; set; }

        public string Email { get; set; }

        public Enums.Status Stat { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Enums.Area Area { get; set; }

        public Enums.UnitType type { get; set; }

        public int Adults { get; set; }

        public int Children { get; set; }

        public bool Pool { get; set; }

        public bool Jecuzzi { get; set; }

        public bool Garden { get; set; }

        public bool ChildrenAttractions { get; set; }
    }
}