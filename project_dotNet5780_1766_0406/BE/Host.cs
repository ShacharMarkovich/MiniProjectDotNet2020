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

        private string _privateName { get; }
        private string _familyName { get; }
        private string _phoneNumber { get; }
        private string _email { get; }
        private BankAccount _bankAccount { get; }
        private bool _collectionClearance { get; }


        public override string ToString() => base.ToString();
    }
}
