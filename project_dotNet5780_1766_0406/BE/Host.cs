using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    /// in the next part of the project we will make this class singleton
    public class Host
    {
        // the following bool property is in order to make the next key
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        [XmlIgnore]
        private bool _hostKey_setAlready = false;
        [XmlIgnore]
        private double _hostKey;
        public double HostKey
        {
            get => _hostKey;
            set
            {
                if (!_hostKey_setAlready)
                {
                    _hostKey = value;
                    _hostKey_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.Host._hostKey property can only once change!");
            }
        }

        public string PrivateName { get; set; }

        public string FamilyName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public BankBranch BankBranchDetails { get; set; }

        public double BankAccountNumber { get; set; }

        public double Balance { get; set; }

        public bool CollectionClearance { get; set; }
    }
}