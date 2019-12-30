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
        public readonly double _hostKey;

        private string _privateName { get; set; }
        public string PrivateName
        {
            set { _privateName = value; }
        }
        private string _familyName { get; set; }
        public string FamilyName
        {
            set { _familyName = value; }
        }
        private string _phoneNumber { get; set; }
        public string PhoneNumber
        {
            set { _phoneNumber = value; }
        }
        private string _email { get; set; }
        public string Email
        {
            set { _email = value; }
        }
        private BankBranch _bankBranchDetails { get; set; }
        public BankBranch BankBranchDetails
        {
            set { _bankBranchDetails = value; }
        }
        private double _bankAccountNumber { get; set; }
        public double BankAccountNumber
        {
            set { _bankAccountNumber = value; }
        }
        private bool _collectionClearance { get; set; }
        public bool CollectionClearance
        {
            set { _collectionClearance = value; }
        }
        

        public override string ToString()
        {
            return "Private Name:\t" + _privateName +
                "\nFamily Name:\t" + _familyName +
                "\nPhone Number:\t" + _phoneNumber +
                "\nEmail:\t" + _email +
                "\nBank Branch Details:\t" + _bankBranchDetails.ToString() +
                "\nBank Account Number:\t" + _bankAccountNumber +
                "\nCollection Clearance:\t" + _collectionClearance;
        }
    }
}