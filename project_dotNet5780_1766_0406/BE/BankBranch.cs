using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankBranch
    {
        // the following bool property is in order to make the next key
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        private bool _bankNumber_setAlready = false;
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

        private string _bankName;
        public string BankName
        {
            set => _bankName = value;
            get => _bankName;
        }

        private int _branchNumber;
        public int BranchNumber
        {
            set => _branchNumber = value;
            get => _branchNumber;
        }

        private string _branchAddress;
        public string BranchAddress
        {
            set => _branchAddress = value;
            get => _branchAddress;
        }

        private string _branchCity;
        public double bankNumber;

        public string BranchCity
        {
            set => _branchCity = value;
            get => _branchCity;
        }
    }
}
