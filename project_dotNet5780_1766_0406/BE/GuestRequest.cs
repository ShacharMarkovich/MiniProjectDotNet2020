using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        public readonly double _guestRequestKey;

        private string _privateName { get; set; }
        public string PrivateName
        {
            set
            {
                _privateName = value;
            }
        }
        private string _familyName { get; set; }
        public string FamilyName
        {
            set
            {
                _familyName = value;
            }
        }

        private string _email { get; set; }
        public string Email
        {
            set
            {
                _email = value;
            }
        }

        private Enums.Status _stat { get; set; }
        public Enums.Status Stat
        {
            set
            {
                _stat = value;
            }
        }

        // all the dates are in this form: dd.mm.yyyy
        private string _registrationDate { get; set; }
        public string RegistrationDate
        {
            set
            {
                _registrationDate = value;
            }
        }
        private string _entyDate { get; set; }
        public string EntyDate
        {
            set
            {
                _entyDate = value;
            }
        }
        private string _releaseDate { get; set; }
        public string ReleaseDate
        {
            set
            {
                _releaseDate = value;
            }
        }

        private Enums.Area _area { get; set; }
        public Enums.Area Area
        {
            set
            {
                _area = value;
            }
        }
        private Enums.SubArea _subArea { get; set; } // optional
        public Enums.SubArea SubArea
        {
            set
            {
                _subArea = value;
            }
        }
        private Enums.UnitType _type { get; set; }
        public Enums.UnitType type
        {
            set
            {
                _type = value;
            }
        }

        private int _adults { get; set; }
        public int Adults
        {
            set
            {
                _adults = value;
            }
        }
        private int _children { get; set; }
        public int Children
        {
            set
            {
                _children = value;
            }
        }

        private bool _pool { get; set; }
        public bool Pool
        {
            set
            {
                _pool = value;
            }
        }
        private bool _jecuzzi { get; set; }
        public bool Jecuzzi
        {
            set
            {
                _jecuzzi = value;
            }
        }
        private bool _garden { get; set; }
        public bool Garden
        {
            set
            {
                _garden = value;
            }
        }
        private bool _childrenAttractions { get; set; }
        public bool ChildrenAttractions
        {
            set
            {
                _childrenAttractions = value;
            }
        }

        public override string ToString()
        {
            return "\nRegistration Date:\t" + _registrationDate +
                "\nEnty Date:\t" + _entyDate +
                "\nRelease Date:\t" + _releaseDate +
                "\nAarea:\t" + _area +
                "\nSubArea:\t" + _subArea +
                "\nType:\t" + _type +
                "\nAdults:\t" + _adults +
                "\nChildren:\t" + _children +
                "\nPool:\t" + _pool +
                "\nJecuzzi:\t" + _jecuzzi +
                "\nGarden:\t" + _garden +
                "\nChildren Attractions:\t" + _childrenAttractions;
        }

    }
}
