using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class DataSource
    {
        public static List<BE.GuestRequest> _guestRequestsList;
        public static List<BE.HostingUnit> _hostingUnitsList;
        public static List<BE.Order> _ordersList;
        public static List<BE.BankBranch> _bankBranchList;

        public DataSource()
        {
            // Manual initialization ONLY for this part of the project
            _guestRequestsList = new List<BE.GuestRequest>();
            _hostingUnitsList = new List<BE.HostingUnit>()
            {
                new BE.HostingUnit()
                {
                    HostingUnitName = "HostingUnitName1",
                    Diary = new bool[12, 31],
                    Owner = new BE.Host()
                    {
                        PrivateName = "name1",
                        FamilyName = "pname1",
                        PhoneNumber = "phone1",
                        Email = "email1@org.tld",
                        BankAccountNumber = 1,
                        CollectionClearance = false,
                        BankBranchDetails = new BE.BankBranch()
                        {
                            BankName = "bank1",
                            BranchNumber = 1,
                            BranchAddress = "address1",
                            BranchCity="city1"
                        }
                    }
                },
                new BE.HostingUnit()
                {
                    HostingUnitName = "HostingUnitName2",
                    Diary = new bool[12, 31],
                    Owner = new BE.Host()
                    {
                        PrivateName = "name2",
                        FamilyName = "pname2",
                        PhoneNumber = "phone2",
                        Email = "email2@org.tld",
                        BankAccountNumber = 2,
                        CollectionClearance = false,
                        BankBranchDetails = new BE.BankBranch()
                        {
                            BankName = "bank2",
                            BranchNumber = 2,
                            BranchAddress = "address2",
                            BranchCity="city2"
                        }
                    }
                },
                new BE.HostingUnit()
                {
                    HostingUnitName = "HostingUnitName3",
                    Diary = new bool[12, 31],
                    Owner = new BE.Host()
                    {
                        PrivateName = "name3",
                        FamilyName = "pname3",
                        PhoneNumber = "phone3",
                        Email = "email3@org.tld",
                        BankAccountNumber = 3,
                        CollectionClearance = true,
                        BankBranchDetails = new BE.BankBranch()
                        {
                            BankName = "bank3",
                            BranchNumber = 3,
                            BranchAddress = "address3",
                            BranchCity="city3"
                        }
                    }
                },
            };
            _ordersList = new List<BE.Order>()
            {
                new BE.Order(){
                    Status = (BE.Enums.Status) 0,
                    CreateDate = "1.1.2000",
                    OrderDate = "3.1.200"
                },
                new BE.Order(){
                    Status = (BE.Enums.Status) 1,
                    CreateDate = "2.2.2001",
                    OrderDate = "3.2.2001"
                },
                new BE.Order(){
                    Status = (BE.Enums.Status) 2,
                    CreateDate = "3.3.2002",
                    OrderDate = "4.2.2002"
                },

            };
            _bankBranchList = new List<BE.BankBranch>()
            {
                new BE.BankBranch(){
                    BankName = "bank1",
                    BranchNumber = 1,
                    BranchAddress = "address1",
                    BranchCity="city1"
                },
                new BE.BankBranch(){
                    BankName = "bank2",
                    BranchNumber = 2,
                    BranchAddress = "address2",
                    BranchCity="city2"
                },
                new BE.BankBranch(){
                    BankName = "bank3",
                    BranchNumber = 3,
                    BranchAddress = "address3",
                    BranchCity="city3"
                },
                new BE.BankBranch(){
                    BankName = "bank4",
                    BranchNumber = 4,
                    BranchAddress = "address4",
                    BranchCity="city4"
                },
                new BE.BankBranch(){
                    BankName = "bank5",
                    BranchNumber = 5,
                    BranchAddress = "address5",
                    BranchCity="city5"
                },

            };
        }
    }
}
