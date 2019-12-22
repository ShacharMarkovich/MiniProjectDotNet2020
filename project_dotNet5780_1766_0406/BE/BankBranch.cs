using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BankBranch
    {
        public readonly double _bankNumber;
        private string _bankName { get; }
        private int _branchNumber { get; }
        private string _branchAddress { get; }
        private string _branchCity { get; }

        ///TODO
        public override string ToString() => base.ToString();

    }
}
