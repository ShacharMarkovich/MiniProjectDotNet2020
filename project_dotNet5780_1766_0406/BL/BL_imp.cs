using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class BL_imp : IBL
    {
        private DAL.IDal _dal;

        private static IBL _instance = null;

        public static IBL Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BL_imp();
                return _instance;
            }
        }

        private BL_imp() { _dal = DAL.FactoryDAL.Instance; }

        public bool Ischronological(DateTime entryDate, DateTime releaseDate)
        {
            return (releaseDate - entryDate).Days >= 1;
        }

        public bool IsCollectionClearance(BE.Host host)
        {
            return host.CollectionClearance;
        }

        public bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            if (!Ischronological(entryDate, releaseDate))
                return false;

            int count = (releaseDate - entryDate).Days;
            return IsDateArmor(hostingUnit, entryDate, count);
        }

        public bool IsOrderClose(BE.Order order)
        {
            return order.Status >= BE.Enums.Status.CloseByClient;
        }

        public void TakeFee(BE.Order order)
        {
            // find match GuestRequest to given order
            List<BE.GuestRequest> GuestRequestsList = _dal.GetAllRequests();
            IEnumerable<BE.GuestRequest> req = from gReq in GuestRequestsList
                                               where gReq.GuestRequestKey == order.GuestRequestKey
                                               select gReq;
            int days = 0;
            try
            {
                // get number of vacation days 
                BE.GuestRequest gReq = req.Single();
                days = (gReq.ReleaseDate - gReq.EntryDate).Days;
            }
            // handle exceptions
            catch (ArgumentNullException exc)
            {
                Console.WriteLine("guestsRequest is null");
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("guestsRequest has more than one element");
            }


            List<BE.HostingUnit> HostingUnitsList = _dal.GetAllHostingUnits();
            IEnumerable<BE.HostingUnit> units = from unit in HostingUnitsList
                                                where unit.HostingUnitKey == order.HostingUnitKey
                                                select unit;
            try
            {
                // take fee
                BE.HostingUnit unit = units.Single();
                unit.Owner.Balance -= days * BE.Configuration.Fee;
                _dal.UpdateHostingUnit(unit); // update DS
            }
            // handle exceptions
            catch (ArgumentNullException exc)
            {
                Console.WriteLine("guestsRequest is null");
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("guestsRequest has more than one element");
            }
        }

        public void UpdateCalendar(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            if (IsDateArmor(hostingUnit, entryDate, releaseDate))
            {
                int count = (releaseDate - entryDate).Days,
                    month = entryDate.Month,
                    day = entryDate.Day;
                bool[,] diary = hostingUnit.Diary;

                for (int i = 0; i < count; i++, day++)
                {
                    if (day == BE.Configuration._days) // check if we in end of month
                    {
                        // past to next month
                        day = 0;
                        month++;
                        if (month == BE.Configuration._month) // check if we in end of year
                            month = 0;
                    }
                    diary[month, day] = true;
                }
                hostingUnit.Diary = diary; // update the data structer
            }
            else
                throw new Exception("Dates already taken in this hosting unit");
        }

        public void SelectInvitation(BE.Order order)
        {
            List<BE.GuestRequest> allReq = _dal.GetAllRequests();

            BE.GuestRequest request = null;
            try
            {
                // find the much guest request to the given order...
                request = (from gReq in allReq
                           where gReq.GuestRequestKey == order.GuestRequestKey
                           select gReq).Single();
                // /// and update his status
                _dal.UpdateGuestRequest(request, BE.Enums.Status.CloseByApp);
            }
            catch (ArgumentNullException exc)
            {
                Console.WriteLine("guestsRequest is null");
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("guestsRequest has more than one element");
            }

            // find the rest of the orders which mach to the request...
            IEnumerable<BE.Order> orders = from matchOrder in _dal.GetAllOrders()
                                           where matchOrder.GuestRequestKey == request.GuestRequestKey
                                           select matchOrder;
            // ...and close them too
            foreach (BE.Order matchOrder in orders)
                _dal.UpdateOrder(matchOrder, BE.Enums.Status.CloseByApp);
        }

        public bool IsPossibleToDelete(BE.HostingUnit hostingUnit)
        {
            List<BE.Order> orders = _dal.GetAllOrders();

            IEnumerable<BE.Order> belongsTo = from order in orders
                                              where (order.HostingUnitKey == hostingUnit.HostingUnitKey && !IsOrderClose(order))
                                              select order;

            return belongsTo.Count() == 0;
        }

        public bool IsCanCancalCollectionClearance(BE.Host host)
        {
            List<BE.HostingUnit> units = _dal.GetAllHostingUnits();
            // get all HostingUnit that host is there owner
            List<BE.HostingUnit> hostingUnit = (from unit in units
                                                where unit.Owner.HostKey == host.HostKey
                                                select unit).ToList();

            // check if for all units there is no open order
            foreach (BE.HostingUnit unit in units)
                if (IsPossibleToDelete(unit) == false)
                    return false;

            return true;
        }

        public void SendEmail(BE.Host host/*FROM*/, string gReqEmail/*TO*/)
        {
            // TODO: realy send the mail
            if (IsCollectionClearance(host))
                Console.WriteLine("{0} is sending email to {1}....", host.Email, gReqEmail);
            else
                throw new Exception("host not has a CollectionClearance!");
        }

        //////////////////////////////////////////////////////////

        public List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int daysNumber)
        {
            List<BE.HostingUnit> free = (from unit in _dal.GetAllHostingUnits()
                                         where IsDateArmor(unit, entryDate, daysNumber)
                                         select unit).ToList();
            return free;
        }

        public int DaysNumber(params DateTime[] ArrDate)
        {
            if (ArrDate.Length == 1)
                return (ArrDate[0] - DateTime.Now).Days;
            else if (ArrDate.Length == 2)
                return (ArrDate[1] - ArrDate[0]).Days;
            else
                throw new ArgumentException("ArrDate.Length > 2 || ArrDate.Length < 1");
        }

        public List<BE.Order> AtLeastnDays(int n)
        {
            IEnumerable<BE.Order> enumerator = from order in _dal.GetAllOrders()
                                               where ((DateTime.Now - order.CreateDate).Days >= n || (DateTime.Now - order.OrderDate).Days >= n)
                                               select order;
            return enumerator.ToList();
        }

        public List<BE.GuestRequest> AccordingTo(BE.Configuration.Term term)
        {
            IEnumerable<BE.GuestRequest> gReqs = from gReq in _dal.GetAllRequests()
                                                 where term(gReq)
                                                 select gReq;

            return gReqs.ToList();
        }

        public int OrderCount(BE.GuestRequest gReq)
        {
            IEnumerable<BE.Order> orders = from order in _dal.GetAllOrders()
                                           where order.GuestRequestKey == gReq.GuestRequestKey
                                           select order;

            return orders.Count();
        }

        public int ApprovedCount(BE.HostingUnit hostingUnit)
        {
            IEnumerable<BE.Order> orders = from order in _dal.GetAllOrders()
                                           where order.GuestRequestKey == hostingUnit.HostingUnitKey &&
                                           (order.Status == BE.Enums.Status.Approved || order.Status == BE.Enums.Status.MailSent)
                                           select order;

            return orders.Count();
        }


        public List<IGrouping<BE.Enums.Area, BE.GuestRequest>> GroupGuestRequestByArea()
        {
            IEnumerable<IGrouping<BE.Enums.Area, BE.GuestRequest>> group = from gReq in _dal.GetAllRequests()
                                                                           group gReq by gReq.Area;
            return group.ToList();
        }

        public List<IGrouping<int, BE.GuestRequest>> GroupGuestRequestByPeopleCount()
        {
            IEnumerable<IGrouping<int, BE.GuestRequest>> group = from gReq in _dal.GetAllRequests()
                                                                 group gReq by (gReq.Children + gReq.Adults);
            return group.ToList();
        }

        public List<IGrouping<int, BE.Host>> GroupHostByfHostingUnitCount()
        {
            IEnumerable<IGrouping<BE.Host, BE.HostingUnit>> host2unit = from hostingUnit in _dal.GetAllHostingUnits()
                                                                        group hostingUnit by hostingUnit.Owner;

            IEnumerable<IGrouping<int, BE.Host>> lst = from IGrouping<BE.Host, BE.HostingUnit> g in host2unit
                                                       let count = g.Count()
                                                       group g.Key by count;

            return lst.ToList();
        }

        public List<IGrouping<BE.Enums.Area, BE.HostingUnit>> GroupHostingUnitByArea()
        {
            IEnumerable<IGrouping<BE.Enums.Area, BE.HostingUnit>> Hosting_unit = from Hunit in _dal.GetAllHostingUnits()
                                                                                 group Hunit by Hunit.Area;
            return Hosting_unit.ToList();
        }

        //////////////////////////////////////////////////////////
        // our additional functions:
        
        public bool InCalendar(DateTime time)
        {
            DateTime now = DateTime.Now;
            return (time - now).Days > ((BE.Configuration._month - 1) * BE.Configuration._days);
        }

        /// <summary>
        /// check if dates available to order
        /// </summary>
        private bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, int count)
        {
            int month = entryDate.Month, day = entryDate.Day;
            bool[,] diary = hostingUnit.Diary;

            if (diary[month, day] == false || (diary[month, day] == true && diary[month, day + 1] == false))
            {
                for (int i = 0; i < count; i++, day++)
                {
                    if (day == BE.Configuration._days) // check if we in end of month
                    {
                        // past to next month
                        day = 0;
                        month++;
                        if (month == BE.Configuration._month) // check if we in end of year
                            month = 0;
                    }
                    // check for exists busy day 
                    if (diary[month, day] == true && (i != 0 || i == count - 1))
                        return false;
                    else
                        diary[month, day] = true;
                }
                return true;
            }
            return false;
        }
    }
}
