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
        public static List<BE.GuestRequest> _guestRequestsList { get; set; }
        public static List<BE.BankBranch> _bankBranchList { get; set; }
        public static List<BE.HostingUnit> _hostingUnitsList { get; set; }
        public static List<BE.Order> _ordersList { get; set; }
        public static List<BE.Host> _hostsList { get; set; }

        public static void Init()
        {
            _guestRequestsList = new List<BE.GuestRequest>()
                {
                    new BE.GuestRequest()
                    {
                        GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                        PrivateName = "name1",
                        FamilyName = "last_name1",
                        Email = "1@gmail.com",
                        Stat = BE.Enums.Status.NotYetApproved,
                        RegistrationDate = new DateTime(2020,1,10),
                        EntryDate =  new DateTime(2020,4,5),
                        ReleaseDate = new DateTime(2020,4,15),
                        Area = BE.Enums.Area.Jerusalem,
                        type = BE.Enums.UnitType.Hotel,
                        Adults = 10,
                        Children = 0,
                        Pool = false,
                        Jecuzzi = false,
                        Garden = true,
                        ChildrenAttractions = false,
                    },
                    new BE.GuestRequest()
                    {
                        GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                        PrivateName = "name2",
                        FamilyName = "last_name2",
                        Email = "2@gmail.com",
                        Stat = BE.Enums.Status.Approved,
                        RegistrationDate = new DateTime(2020,1,10),
                        EntryDate =  new DateTime(2020,5,4),
                        ReleaseDate = new DateTime(2020,5,5),
                        Area = BE.Enums.Area.Center,
                        type = BE.Enums.UnitType.Camping,
                        Adults = 1,
                        Children = 2,
                        Pool = false,
                        Jecuzzi = true,
                        Garden = true,
                        ChildrenAttractions = false,
                    },
                    new BE.GuestRequest()
                    {
                        GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                        PrivateName = "name3",
                        FamilyName = "last_name3",
                        Email = "3@gmail.com",
                        Stat = BE.Enums.Status.MailSent,
                        RegistrationDate = new DateTime(2020,1,10),
                        EntryDate =  new DateTime(2020,4,15),
                        ReleaseDate = new DateTime(2020,4,17),
                        Area = BE.Enums.Area.North,
                        type = BE.Enums.UnitType.RentingRoom,
                        Adults = 3,
                        Children = 2,
                        Pool = true,
                        Jecuzzi = true,
                        Garden = true,
                        ChildrenAttractions = true,
                    }
                };
            _bankBranchList = new List<BE.BankBranch>()
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
                    }
                };
            _hostsList = new List<BE.Host>()
                {
                    new BE.Host()
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
                    },
                    new BE.Host()
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
                    },
                    new BE.Host()
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
            };
            _hostingUnitsList = new List<BE.HostingUnit>()
                {
                    new BE.HostingUnit()
                    {
                        HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                        HostingUnitName = "HostingUnitName1",
                        Diary = new bool[12, 31],
                        Area = _guestRequestsList[0].Area,
                        type = _guestRequestsList[0].type,
                        Owner = _hostsList[0]
                    },
                    new BE.HostingUnit()
                    {
                        HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                        HostingUnitName = "HostingUnitName2",
                        Diary = new bool[12, 31],
                        Area = _guestRequestsList[1].Area,
                        type = _guestRequestsList[1].type,
                        Owner = _hostsList[0]
                    },
                    new BE.HostingUnit()
                    {
                        HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                        HostingUnitName = "HostingUnitName3",
                        Diary = new bool[12, 31],
                        Area = _guestRequestsList[2].Area,
                        type = _guestRequestsList[2].type,
                        Owner = _hostsList[1]
                    },
                    new BE.HostingUnit()
                    {
                        HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                        HostingUnitName = "HostingUnitName4",
                        Diary = new bool[12, 31],
                        Area = BE.Enums.Area.South,
                        type = BE.Enums.UnitType.Zimmer,
                        Owner = _hostsList[2]
                    },
                };
            _ordersList = new List<BE.Order>()
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
}
