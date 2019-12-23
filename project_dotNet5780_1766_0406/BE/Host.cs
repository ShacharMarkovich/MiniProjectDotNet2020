using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class Host
    {
        public readonly double _hostKey;

        private string _privateName { get; set; }
        private string _familyName { get; set; }
        private string _phoneNumber { get; set; }
        private string _email { get; set; }
        private BankBranch _bankBranchDetails { get; set; }
        private double _bankAccountNumber { get; set; }
        private bool _collectionClearance { get; set; }

        Host()
        {
            _hostKey = ++Configuration.HostKey;
        }

        public override string ToString() => base.ToString();
    }
}
