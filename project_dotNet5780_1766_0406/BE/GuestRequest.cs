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

        private string _privateName { get; }
        private string _familyName { get; }
        private string _email { get; }
        private Enums.Status _stat { get; }

        // all the dates are in this form: dd.mm.yyyy
        private string _registrationDate { get; }
        private string _entyDate { get; }
        private string _releaseDate { get; }

        private Enums.Area _area { get; }
        private Enums.SubArea _subArea { get; } // optional
        private Enums.UnitType _type { get; }

        private int _adults { get; }
        private int _children { get; }

        private bool _pool { get; }
        private bool _jecuzzi { get; }
        private bool _garden { get; }
        private bool _childrenAttractions { get; }


        public override string ToString() => base.ToString();

    }
}
