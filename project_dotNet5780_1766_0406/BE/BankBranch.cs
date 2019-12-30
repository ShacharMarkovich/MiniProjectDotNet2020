using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// in the next part of the project we will make this class singleton
    public class BankBranch
    {
        public readonly double _bankNumber;

        //defualt getter
        private string _bankName;
        public string BankName
        {
            set
            {
                _bankName = value;
            }
            get
            {
                return _bankName;
            }
        }

        private int _branchNumber;
        public int BranchNumber
        {
            set
            {
                _branchNumber = value;
            }
            get
            {
                return _branchNumber;
            }
        }

        private string _branchAddress;
        public string BranchAddress
        {
            set { _branchAddress = value; }
            get { return _branchAddress; }
        }

        private string _branchCity;
        public string BranchCity
        {
            set { _branchCity = value; }
            get { return _branchCity; }
        }

        
        public override string ToString()
        {
            return "Bank Number:\t" + _bankName +
                "\nBank Name:\t" + _bankName +
                "\nBranch Number:\t" + _branchNumber +
                "\nBranch Address:\t" + _branchAddress +
                "\nBranch City:\t" + _branchCity;
        }

    }
}
