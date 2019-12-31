using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// in the next part of the project we will make this class singleton
    public class Host
    {
        private bool _hostKey_setAlready = false;
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

        private string _privateName;
        public string PrivateName
        {
            set => _privateName = value;
            get => _privateName;
        }

        private string _familyName;
        public string FamilyName
        {
            set => _familyName = value;
            get => _familyName;
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            set => _phoneNumber = value;
            get => _phoneNumber;
        }

        private string _email;
        public string Email
        {
            set => _email = value;
            get => _email;
        }

        private BankBranch _bankBranchDetails;
        public BankBranch BankBranchDetails
        {
            set => _bankBranchDetails = value;
            get => _bankBranchDetails;
        }

        private double _bankAccountNumber;
        public double BankAccountNumber
        {
            set => _bankAccountNumber = value;
            get => _bankAccountNumber;
        }

        private double _balance;
        public double Balance
        {
            set => _balance = value;
            get => _balance;
        }

        private bool _collectionClearance;
        public bool CollectionClearance
        {
            set => _collectionClearance = value;
            get => _collectionClearance;
        }

        public override string ToString()
        {
            return "\nHost Key:\t" + _hostKey +
                "\nPrivate Name:\t" + _privateName +
                "\nFamily Name:\t" + _familyName +
                "\nPhone Number:\t" + _phoneNumber +
                "\nEmail:\t" + _email +
                "\nBank Branch Details:\n" + _bankBranchDetails.ToString() +
                "\nBank Account Number:\t" + _bankAccountNumber +
                "\nHas Collection Clearance: " + _collectionClearance;
        }
    }
}