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

        private string _privateName;
        public string PrivateName
        {
            set
            {
                _privateName = value;
            }
            get
            {
                return _privateName;
            }
        }

        private string _familyName;
        public string FamilyName
        {
            set
            {
                _familyName = value;
            }
            get
            {
                return _familyName;
            }
        }

        private string _email;
        public string Email
        {
            set
            {
                _email = value;
            } 
            get
            {
                return _email;
            }
        }

        private Enums.Status _stat;
        public Enums.Status Stat
        {
            set
            {
                _stat = value;
            }
            get
            {
                return _stat;
            }
        }

        private DateTime _registrationDate;
        public DateTime RegistrationDate
        {
            set =>_registrationDate = value;
            get => _registrationDate;
        }

        private DateTime _entryDate;
        public DateTime EntryDate
        {
            set=>_entryDate = value;
            get => _entryDate;
        }

        private DateTime _releaseDate;
        public DateTime ReleaseDate
        {
            set
            {
                _releaseDate = value;
            }
            get => _releaseDate;
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

        private Enums.UnitType _type { get; set; }
        public Enums.UnitType type
        {
            set
            {
                _type = value;
            }
            get => _type;
        }

        private int _adults { get; set; }
        public int Adults
        {
            set
            {
                _adults = value;
            }
            get => _adults;
        }

        private int _children { get; set; }
        public int Children
        {
            set
            {
                _children = value;
            }
            get => _children;
        }

        private bool _pool { get; set; }
        public bool Pool
        {
            set
            {
                _pool = value;
            }
            get => _pool;
        }

        private bool _jecuzzi { get; set; }
        public bool Jecuzzi
        {
            set
            {
                _jecuzzi = value;
            }
            get => _jecuzzi;
        }

        private bool _garden;
        public bool Garden
        {
            set
            {
                _garden = value;
            }
            get => _garden;
        }

        private bool _childrenAttractions;
        public bool ChildrenAttractions
        {
            set
            {
                _childrenAttractions = value;
            }
            get => _childrenAttractions;
        }


        /// <summary>
        /// for now the c'tor is public, in order to make 3 instances that needed in DS.DataSource in the next part of the project we will make this class singleton
        /// </summary>
        public GuestRequest(string privateName, string familyName, string email, Enums.Status stat,
                            DateTime registrationDate, DateTime entryDate, DateTime releaseDate,
                            Enums.Area area, Enums.UnitType type_, int adults, int children,
                            bool pool, bool jecuzzi, bool garden, bool childrenAttractions)
        {
            _guestRequestKey = ++Configuration.GuestRequestKey;
            PrivateName = privateName;
            FamilyName = familyName;
            Email = email;
            Stat = stat;
            RegistrationDate = registrationDate;
            EntryDate = entryDate;
            ReleaseDate = releaseDate;
            Area = area;
            type = type_;
            Adults = adults;
            Children = children;
            Pool = pool;
            Jecuzzi = jecuzzi;
            Garden = garden;
            ChildrenAttractions = childrenAttractions;
        }
        /// <summary>
        /// swap to string
        /// </summary>
        public override string ToString()
        {
            return "\nRegistration Date:\t" + _registrationDate +
                "\nEnty Date:\t" + _entryDate +
                "\nRelease Date:\t" + _releaseDate +
                "\nAarea:\t" + _area +
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