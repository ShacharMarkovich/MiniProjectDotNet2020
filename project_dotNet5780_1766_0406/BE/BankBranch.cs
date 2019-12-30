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
        private string _bankName { get; set; }
        public string BankName
        {
            set
            {
                _bankName = value;
            }
        }

        private int _branchNumber { get; set; }
        public int BranchNumber
        {
            set
            {
                _branchNumber = value;
            }
        }

        private string _branchAddress { get; set; }
        public string BranchAddress
        {
            set
            {
                _branchAddress = value;
            }
        }

        private string _branchCity { get; set; }
        public string BranchCity
        {
            set
            {
                _branchCity = value;
            }
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
