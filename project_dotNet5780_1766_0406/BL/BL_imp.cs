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

        /// <summary>
        /// singleton
        /// </summary>
        public static IBL Instance()
        {
            if (_instance == null)
                _instance = new BL_imp();
            return _instance;
        }

        private BL_imp() { _dal = DAL.FactoryDAL.Dal(); }

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
        /// <summary>
        /// Are dates available to order
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
        public bool IsOrderClose(BE.Order order)
        {
            return order.Status >= BE.Enums.Status.CloseByClient;
        }
        public void TakeFee(BE.Order order)
        {
            var a = _dal.GetAllRequests();
            var req = from gReq in a
                    where gReq._guestRequestKey == order._guestRequestKey
                    select gReq;
            int days = 0;
            try
            {
                BE.GuestRequest c = req.Single();
                days = (c.ReleaseDate - c.EntryDate).Days;

            }
            catch (ArgumentNullException exc)
            {
                Console.WriteLine("EROR IN Guest Request too much or less argument" );
            }
            var d = _dal.GetAllHostingUnits();
            var Hos = from gHos in d
                      where gHos._hostingUnitKey == order._hostingUnitKey
                      select gHos;
            try
            {
                BE.HostingUnit e = Hos.Single();
                e.Owner.Balanse = (e.Owner.Balanse - days * BE.Configuration.Fee);
            }
            catch (ArgumentNullException exc1)
            {
                Console.WriteLine("EROR IN SELECT HostingUnit" );
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
            _dal.UpdateOrder(order, BE.Enums.Status.CloseByClient);
            var a = _dal.GetAllRequests();
            var Req = from gReq in a
                      where gReq._guestRequestKey == order._guestRequestKey
                      select gReq;
            BE.GuestRequest c=null;
            try
            {
                 c = Req.Single();
                _dal.UpdateGuestRequest(c, BE.Enums.Status.CloseByApp);

            }
            catch (ArgumentNullException exc)
            {
                Console.WriteLine("EROR IN Guest Request too much or less argument");
            }
            var cc = from matchOrder in _dal.GetAllOrders()
                     where matchOrder._guestRequestKey == c._guestRequestKey
                     select matchOrder;

            foreach (var matchOrder in cc)
                _dal.UpdateOrder(matchOrder, BE.Enums.Status.CloseByApp);
        }
        public bool IsPossibleToDelete(BE.HostingUnit hostingUnit)
        {
            List<BE.Order> orders = _dal.GetAllOrders();

            var belongsTo = from order in orders
                            where (order._hostingUnitKey == hostingUnit._hostingUnitKey && !IsOrderClose(order))
                            select order;

            return belongsTo.Count() == 0;
        }
        public bool IsCanCancalCollectionClearance(BE.Host host)
        {
            List<BE.HostingUnit> units = _dal.GetAllHostingUnits();
            // get all HostingUnit that host is there owner
            List<BE.HostingUnit> hostingUnit = (from unit in units
                                                where unit.Owner._hostKey == host._hostKey
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

        public List<BE.Order> IsOrderBigFromNumber(int daysNumber)
        {
            var lst = from order in _dal.GetAllOrders()
                      where (DateTime.Now - order.CreateDate).Days >= daysNumber || (DateTime.Now - order.OrderDate).Days >= daysNumber
                      select order;
            return lst.ToList();
        }

        public List<BE.GuestRequest> AccordingTo(Term term)
        {
            var gReqs = from gReq in _dal.GetAllRequests()
                        where term(gReq)
                        select gReq;

            return gReqs.ToList();
        }

        public int OrderCount(BE.GuestRequest gReq)
        {
            var orders = from order in _dal.GetAllOrders()
                         where order._guestRequestKey == gReq._guestRequestKey
                         select order;

            return orders.Count();
        }

        public int ApprovedCount(BE.HostingUnit hostingUnit)
        {
            var orders = from order in _dal.GetAllOrders()
                         where order._guestRequestKey == hostingUnit._hostingUnitKey &&
                         (order.Status == BE.Enums.Status.Approved || order.Status == BE.Enums.Status.MailSent)
                         select order;

            return orders.Count();
        }

        public List<IGrouping<BE.Enums.Area, BE.GuestRequest>> GuestRequest_GroupbyArea()
        {
            IEnumerable<IGrouping<BE.Enums.Area, BE.GuestRequest>> group = from gReq in _dal.GetAllRequests()
                                                                            group gReq by gReq.Area;
            return group.ToList();
        }

        public List<IGrouping<int, BE.GuestRequest>> GuestRequest_GroupbyAmountOfPeople()
        {
            IEnumerable<IGrouping<int, BE.GuestRequest>> group = from gReq in _dal.GetAllRequests()
                                                                 group gReq by (gReq.Children + gReq.Adults);
            return group.ToList();
        }

        public List<IGrouping<int, BE.Host>> Host_GroupbyAmountOfHostingUnit()
        {
            IEnumerable<IGrouping<BE.Host, BE.HostingUnit>> host2unit = from hostingUnit in _dal.GetAllHostingUnits()
                                                                group hostingUnit by hostingUnit.Owner;

            IEnumerable<IGrouping<int, BE.Host>> lst = from IGrouping<BE.Host, BE.HostingUnit> g in host2unit
                                                       let count = g.Count()
                                                       group g.Key by count;

            return lst.ToList();
        }

        public List<IGrouping<BE.Enums.Area, BE.HostingUnit>> HostingUnit_GroupbyArea()
        {
            var Hosting_unit = from Hunit in _dal.GetAllHostingUnits()
                               group Hunit by Hunit.Area;
            return Hosting_unit.ToList();
        }

        //////////////////////////////////////////////////////////
        // our additional functions:

        /// <summary>
        /// return true if time is max 11 months ahead from now
        /// </summary>
        public bool InCalendar(DateTime time)
        {
            DateTime now = DateTime.Now;
            return (time - now).Days > ((BE.Configuration._month - 1) * BE.Configuration._days);
        }

        private bool DateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, int count)
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
