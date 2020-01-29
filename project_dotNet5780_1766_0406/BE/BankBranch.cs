using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class BankBranch
    {
        // the following bool property is in order to make the next key
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        [XmlIgnore]
        private bool _bankNumber_setAlready = false;
        [XmlIgnore]
        private double _bankNumber;
        public double BankNumber
        {
            get => _bankNumber;
            set
            {
                if (!_bankNumber_setAlready)
                {
                    _bankNumber = value;
                    _bankNumber_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.BankBranch._bankNumber property can only once change!");
            }
        }

        public string BankName { get; set; }

        public int BranchNumber { get; set; }

        public string BranchAddress { get; set; }

        public string BranchCity { get; set; }

        public override bool Equals(object obj)
        {
            BankBranch other = obj as BankBranch;

            if (BankNumber != other.BankNumber)
                return false;
            if (BranchNumber != other.BranchNumber)
                return false;
            if (BankName != other.BankName)
                return false;
            if (BranchAddress != other.BranchAddress)
                return false;
            if (BranchCity != other.BranchCity)
                return false;
        
            return true;
        }
    }
}
