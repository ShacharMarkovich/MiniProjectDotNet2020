using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    using BE;
    class Program
    {
        public static BL.IBL _bl = BL.FactoryBL.Instance;

        static void Clear()
        {
            Console.WriteLine("press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void Main(string[] args)
        {
            DateTime entryDate = new DateTime(2020, 4, 5),
                releaseDate = new DateTime(2020, 5, 12);

            BE.Host host = new BE.Host()
            {
                HostKey = ++BE.Configuration.HostKey,
                Balance = 105,
                PrivateName = "no",
                FamilyName = "money",
                PhoneNumber = "05050505",
                Email = "takemymoney@machon.lev",
                CollectionClearance = true,
                BankBranchDetails = new BE.BankBranch()
                {
                    BankNumber = ++BE.Configuration.BankNumber,
                    BankName = "take your money",
                    BranchAddress = "take your money city",
                    BranchCity = "no money city",
                    BranchNumber = 15
                },
                BankAccountNumber = 130559,
            };
            BE.GuestRequest guestRequest = new BE.GuestRequest
            {
                GuestRequestKey = ++BE.Configuration.GuestRequestKey,
                PrivateName = "shachar",
                FamilyName = "markovich",
                Email = "21com.bat21@gmail.com",
                Stat = BE.Enums.Status.NotYetApproved,
                RegistrationDate = DateTime.Now,
                EntryDate = new DateTime(2020, 8, 9),
                ReleaseDate = new DateTime(2020, 8, 12),
                Area = BE.Enums.Area.Center,
                type = BE.Enums.UnitType.Hotel,
                Adults = 2,
                Children = 3,
                Pool = true,
                Jecuzzi = true,
                Garden = true,
                ChildrenAttractions = true
            };
            BE.HostingUnit hostingUnit = new BE.HostingUnit()
            {
                HostingUnitKey = ++BE.Configuration.HostingUnitKey,
                HostingUnitName = "hotel california",
                Owner = host,
                type = BE.Enums.UnitType.Hotel,
                Area = BE.Enums.Area.Center,
                Diary = new bool[BE.Configuration._month, BE.Configuration._days],
            };
            BE.Order order = new BE.Order()
            {
                OrderKey = ++BE.Configuration.OrderKey,
                HostingUnitKey = hostingUnit.HostingUnitKey,
                GuestRequestKey = guestRequest.GuestRequestKey,
                Status = guestRequest.Stat,
                CreateDate = DateTime.Now,
                OrderDate = new DateTime(2020, 3, 3)
            };


            _bl.AddGuestRequest(guestRequest);
            _bl.AddHostingUnit(hostingUnit);
            _bl.AddOrder(order);

            Console.WriteLine("adding the next instance Successfully:\n");
            Console.WriteLine(host);
            Console.WriteLine();
            Console.WriteLine(guestRequest);
            Console.WriteLine();
            Console.WriteLine(order);
            Clear();


            if (_bl.IsDateArmor(hostingUnit, entryDate, releaseDate))
            {
                Console.WriteLine("the following hostingUnit has free rooms between " + entryDate.toString() + " to " + releaseDate.toString());
                Console.WriteLine(hostingUnit);
                Clear();
            }

            Console.WriteLine($"host.Balance before taking fee: {host.Balance}");
            _bl.TakeFee(order);
            List<BE.HostingUnit> unit2 = _bl.AccordingTo(unit => hostingUnit.Owner.HostKey == unit.Owner.HostKey);
            Console.WriteLine($"host.Balance after taking fee: {unit2.Single().Owner.Balance}");
            Clear();

            Console.WriteLine($"hostingUnit.diary before update:\n{hostingUnit.Diary.toString()}");
            _bl.UpdateCalendar(hostingUnit, entryDate, releaseDate);
            unit2 = _bl.AccordingTo(unit => hostingUnit.HostingUnitKey == unit.HostingUnitKey);
            Console.WriteLine($"hostingUnit.diary after update:\n{unit2.Single().Diary.toString()}");
            Clear();


            _bl.SendEmail(host/*FROM*/, "no.money@for.now"/*TO*/);
            int daysNumber = _bl.OrderCount(guestRequest);
            Console.WriteLine($"number of orders sent to {guestRequest.GuestRequestKey} is: {daysNumber}");
            daysNumber = _bl.ApprovedCount(hostingUnit);
            Console.WriteLine($"number of all order that have been closed for the hosting unit {hostingUnit.HostingUnitKey} is: {daysNumber}");
            Clear();

            List<BE.HostingUnit> c = _bl.ListOptionsFree(entryDate, daysNumber);
            foreach (var item in c)
                Console.WriteLine(item);

            Console.WriteLine("\n");
            List<BE.Order> d = _bl.AtLeastnDays(daysNumber);
            foreach (var item in c)
                Console.WriteLine(item);

            Clear();

            List<IGrouping<BE.Enums.Area, BE.GuestRequest>> f = _bl.GroupGuestRequestByArea();
            List<IGrouping<int, BE.GuestRequest>>           g = _bl.GroupGuestRequestByPeopleCount();
            List<IGrouping<int, BE.Host>>                   h= _bl.GroupHostByfHostingUnitCount();
            List<IGrouping<BE.Enums.Area, BE.HostingUnit>>  i = _bl.GroupHostingUnitByArea();
        }
    }
}
