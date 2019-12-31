using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class DataSource
    {
        //manual entry of values to lists
        public static List<BE.GuestRequest> _guestRequestsList = new List<BE.GuestRequest>()
            {
                new BE.GuestRequest("name2", "last_name2", "2@gmail.com", BE.Enums.Status.Approved, 
                    new DateTime(2020,1,10), new DateTime(2020,5,4), new DateTime(2020,5,5), BE.Enums.Area.Center, BE.Enums.UnitType.Camping,
                    1, 2, false, true, false, true),
                new BE.GuestRequest("name1", "last_name1", "1@gmail.com", BE.Enums.Status.NotYetApproved, 
                    new DateTime(2020,1,10), new DateTime(2020,4,5), new DateTime(2020,4,15), BE.Enums.Area.Jerusalem, BE.Enums.UnitType.Hotel,
                    10, 0, false, false, false, false),
                new BE.GuestRequest("name3", "last_name3", "3@gmail.com", BE.Enums.Status.MailSent, 
                    new DateTime(2020,1,10), new DateTime(2020,4,15), new DateTime(2020,4,17), BE.Enums.Area.North, BE.Enums.UnitType.RentingRoom,
                    3, 4, true, true, true, true)
            };
        public static List<BE.BankBranch> _bankBranchList = new List<BE.BankBranch>()
            {
                new BE.BankBranch(){
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "bank1",
                    BranchNumber = 1,
                    BranchAddress = "address1",
                    BranchCity="city1"
                },
                new BE.BankBranch(){
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "bank2",
                    BranchNumber = 2,
                    BranchAddress = "address2",
                    BranchCity="city2"
                },
                new BE.BankBranch(){
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "bank3",
                    BranchNumber = 3,
                    BranchAddress = "address3",
                    BranchCity="city3"
                },
                new BE.BankBranch(){
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "bank4",
                    BranchNumber = 4,
                    BranchAddress = "address4",
                    BranchCity="city4"
                },
                new BE.BankBranch(){
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "bank5",
                    BranchNumber = 5,
                    BranchAddress = "address5",
                    BranchCity="city5"
                },

            };
        public static List<BE.HostingUnit> _hostingUnitsList = new List<BE.HostingUnit>()
            {
                new BE.HostingUnit()
                {
                    HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                    HostingUnitName = "HostingUnitName1",
                    Diary = new bool[12, 31],
                    Area = _guestRequestsList[0].Area,
                    type = _guestRequestsList[0].type,
                    Owner = new BE.Host()
                    {
                        HostKey = ++BE.Configuration.HostKey,
                        Balance = 1000,
                        PrivateName = "name1",
                        FamilyName = "pname1",
                        PhoneNumber = "phone1",
                        Email = "smarkovi@g.jct.ac.il",
                        BankAccountNumber = 120159,
                        CollectionClearance = false,
                        BankBranchDetails = _bankBranchList[0]
                    }
                },
                new BE.HostingUnit()
                {
                    HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                    HostingUnitName = "HostingUnitName2",
                    Diary = new bool[12, 31],
                    Area = _guestRequestsList[1].Area,
                    type = _guestRequestsList[1].type,
                    Owner = new BE.Host()
                    {
                        // same owner as before
                        HostKey = BE.Configuration.HostKey,
                        Balance = 1000,
                        PrivateName = "name1",
                        FamilyName = "pname1",
                        PhoneNumber = "phone1",
                        Email = "smarkovi@g.jct.ac.il",
                        BankAccountNumber =120159,
                        CollectionClearance = false,
                        BankBranchDetails = _bankBranchList[0]
                    }
                },
                new BE.HostingUnit()
                {
                    HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                    HostingUnitName = "HostingUnitName3",
                    Diary = new bool[12, 31],
                    Area = _guestRequestsList[2].Area,
                    type = _guestRequestsList[2].type,
                    Owner = new BE.Host()
                    {
                        HostKey = ++BE.Configuration.HostKey,
                        Balance = 1000,
                        PrivateName = "name3",
                        FamilyName = "pname3",
                        PhoneNumber = "phone3",
                        Email = "shachar.markovich@gmail.com",
                        BankAccountNumber = 10151,
                        CollectionClearance = false,
                        BankBranchDetails = _bankBranchList[3]
                    }
                },
                new BE.HostingUnit()
                {
                    HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                    HostingUnitName = "HostingUnitName4",
                    Diary = new bool[12, 31],
                    Area = BE.Enums.Area.South,
                    type = BE.Enums.UnitType.Zimmer,
                    Owner = new BE.Host()
                    {
                        HostKey = ++BE.Configuration.HostKey,
                        Balance = 1000,
                        PrivateName = "name4",
                        FamilyName = "pname4",
                        PhoneNumber = "phone4",
                        Email = "234shachar@gmail.com",
                        BankAccountNumber = 3,
                        CollectionClearance = true,
                        BankBranchDetails = _bankBranchList[4]
                    }
                },
            };
        public static List<BE.Order> _ordersList = new List<BE.Order>()
            {
                new BE.Order(){
                    GuestRequestKey=_guestRequestsList[0].GuestRequestKey,
                    HostingUnitKey=_hostingUnitsList[0].HostingUnitKey,
                    OrderKey=++BE.Configuration.OrderKey,
                    Status = _guestRequestsList[0].Stat,
                    CreateDate = new DateTime(2020,1,10),
                    OrderDate = new DateTime(2020,1,12)
                },
                new BE.Order(){
                    GuestRequestKey=_guestRequestsList[1].GuestRequestKey,
                    HostingUnitKey=_hostingUnitsList[1].HostingUnitKey,
                    OrderKey=++BE.Configuration.OrderKey,
                    Status = _guestRequestsList[1].Stat,
                    CreateDate = new DateTime(2020,1,10),
                    OrderDate = new DateTime(2020,1,10)
                },
                new BE.Order(){
                    GuestRequestKey=_guestRequestsList[2].GuestRequestKey,
                    HostingUnitKey=_hostingUnitsList[2].HostingUnitKey,
                    OrderKey=++BE.Configuration.OrderKey,
                    Status = _guestRequestsList[2].Stat,
                    CreateDate = new DateTime(2020,1,10),
                    OrderDate = new DateTime(2020,2,10)
                },

            };
    }
}
