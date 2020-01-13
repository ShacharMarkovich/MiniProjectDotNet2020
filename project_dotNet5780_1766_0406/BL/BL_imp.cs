using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    using DAL;
    using BE;
    class BL_imp : IBL
    {
        #region singleton
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
        #endregion

        #region GuestRequest functions signature
        public void AddGuestRequest(BE.GuestRequest gRequest)
        {
            if (!(InCalendar(gRequest.RegistrationDate) && InCalendar(gRequest.EntryDate) &&
                InCalendar(gRequest.ReleaseDate) && Ischronological(gRequest.EntryDate, gRequest.ReleaseDate)))
                throw new ArgumentException("dates are not in this year / not chronological! try again!");
            if (gRequest.PrivateName.Length == 0 || gRequest.FamilyName.Length == 0)
                throw new ArgumentException("names illegal");
            if ((gRequest.Adults != 0 && gRequest.Children == 0) || (gRequest.Adults == 0 && gRequest.Children != 0))
                throw new ArgumentException("Must have at least one child or adult");
            try
            {
                // if this line not throw an Exception - gRequest.Email is a legal email
                new System.Net.Mail.MailAddress(gRequest.Email);
            }
            catch
            {
                throw new ArgumentException("email illegal");
            }

            BE.GuestRequest gR = gRequest.Clone();
            _dal.AddGuestRequest(gR);
        }

        public void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat)
        {
            if (!IsGuestRequestClose(gRequest))
                _dal.UpdateGuestRequest(gRequest.Clone(), newStat);
            else
                throw new ArgumentOutOfRangeException("can't change close Guest Request!");
        }
        #endregion

        #region HostingUnit functions signature
        public void AddHostingUnit(BE.HostingUnit newHostingUnit)
        {
            // TODO in part 3: check if newHostingUnit.Owner.BankBranchDetails is exists in israel
            _dal.AddHostingUnit(newHostingUnit.Clone());
        }

        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            if (IsPossibleToDelete(hostingUnit))
                _dal.DeleteHostingUnit(hostingUnit.Clone());
            else
                throw new ArgumentException($"cannot delete hostingunit({hostingUnit.HostingUnitKey}/({hostingUnit.HostingUnitName})" +
                    $"needed to first close all the orders)");
        }

        public void UpdateHostingUnit(BE.HostingUnit hostingUnit) => _dal.UpdateHostingUnit(hostingUnit.Clone());
        #endregion

        #region Order functions signature
        public void AddOrder(BE.Order newOrder)
        {
            int requestsCount = AccordingTo(gReq => gReq.GuestRequestKey == newOrder.GuestRequestKey).Count;
            int unitsCount = AccordingTo(unit => unit.HostingUnitKey == newOrder.HostingUnitKey).Count;

            // == 0 means that there isn't exists unit/request with those keys
            if (requestsCount == 0 || unitsCount == 0)
                throw new ArgumentNullException("unfamiliar GuestRequest or HostingUnit with those keys!please try again!");

            _dal.AddOrder(newOrder.Clone());
        }

        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {
            if (!IsOrderClose(order.Clone()))
                _dal.UpdateOrder(order.Clone(), newStat);
            else
                throw new ArgumentOutOfRangeException("can't change close order!");
        }
        #endregion

        public void AddHost(BE.Host newHost)
        {
            try
            {
                if (newHost.BankBranchDetails == null || newHost.PrivateName == null || newHost.FamilyName == null || newHost.Email == null || newHost.PhoneNumber == null) ;
            }
            catch
            {
                throw new ArgumentNullException("Please enter data in all fields");
            }

            BE.BankBranch bank = newHost.BankBranchDetails;
            if (bank.BankName == null || bank.BranchAddress == null || bank.BranchCity == null) 
                throw new ArgumentNullException("Please enter data in all fields");

            if (bank.BranchNumber == 0 || newHost.BankAccountNumber == 0)
                throw new ArgumentException("Bank Branch number cannot be zero");

            if (IsCollectionClearance(newHost))
                throw new ArgumentException("Collection Clearance must be checked!");

            try
            {
                new System.Net.Mail.MailAddress(newHost.Email);
            }
            catch
            {
                throw new ArgumentException("email illegal");
            }

            _dal.AddHost(newHost.Clone());
        }

        public List<BE.Host> GetAllHosts()
        {
            return _dal.GetAllHosts();
        }

        public bool Ischronological(DateTime entryDate, DateTime releaseDate)
        {
            int days = (releaseDate - entryDate).Days;
            return days >= 1;
        }

        public bool IsCollectionClearance(BE.Host host)
        {
            return host.CollectionClearance;
        }

        public bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            if (!Ischronological(entryDate, releaseDate))
                return false;
            int count = Count2Diary(entryDate, releaseDate);
            return IsDateArmor(hostingUnit, entryDate, count);
           
        }

        public bool IsOrderClose(BE.Order order) => order.Status >= BE.Enums.Status.CloseByClient;

        public bool IsGuestRequestClose(BE.GuestRequest gReq) => gReq.Stat >= BE.Enums.Status.CloseByClient;

        public void TakeFee(BE.Order order)
        {
            // find matching GuestRequest to given order
            List<BE.GuestRequest> GuestRequestsList = _dal.GetAllRequests();
            IEnumerable<BE.GuestRequest> req = from guestRequest in GuestRequestsList
                                               where guestRequest.GuestRequestKey == order.GuestRequestKey
                                               select guestRequest;
            int days = 0;
            BE.GuestRequest gReq = null;
            try
            {
                gReq = req.Single();
            }
            // handle exceptions
            catch (ArgumentNullException exc)
            {
                Console.WriteLine(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.Message);
            }
            // get number of vacation days 
            days = (gReq.ReleaseDate - gReq.EntryDate).Days;


            // find matching HostingUnit to given order
            List<BE.HostingUnit> HostingUnitsList = _dal.GetAllHostingUnits();
            IEnumerable<BE.HostingUnit> units = from hostingUnit in HostingUnitsList
                                                where hostingUnit.HostingUnitKey == order.HostingUnitKey
                                                select hostingUnit;
            BE.HostingUnit unit = null;
            try
            {
                unit = units.Single();
            }
            // handle exceptions
            catch (ArgumentNullException exc)
            {
                Console.WriteLine(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.Message);
            }
            // take fee
            unit.Owner.Balance -= days * BE.Configuration.Fee;
            _dal.UpdateHostingUnit(unit); // update DS
        }

        public void UpdateCalendar(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {

            if (IsDateArmor(hostingUnit, entryDate, releaseDate))
            {
                int month = entryDate.Month - 1,
                    day = entryDate.Day - 1;
                bool[,] diary = hostingUnit.Diary;
                int count = Count2Diary(entryDate, releaseDate);
                for (int i = 0; i < count; i++,++day)
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
                _dal.UpdateHostingUnit(hostingUnit);
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
                Console.WriteLine(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.Message);
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
            // get all open orders who belong to the given hostingUnit
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
            switch (ArrDate.Length)
            {
                case 1:
                    return (ArrDate[0] - DateTime.Now).Days;
                case 2:
                    return (ArrDate[1] - ArrDate[0]).Days;
                default:
                    throw new ArgumentException("ArrDate.Length > 2 || ArrDate.Length < 1");
            }
        }

        public List<BE.Order> AtLeastnDays(int n)
        {
            IEnumerable<BE.Order> enumerator = from order in _dal.GetAllOrders()
                                               where ((DateTime.Now - order.CreateDate).Days >= n ||
                                               (DateTime.Now - order.OrderDate).Days >= n)
                                               select order;
            return enumerator.ToList();
        }

        public List<BE.GuestRequest> AccordingTo(BE.Configuration.Term<BE.GuestRequest> term)
        {
            IEnumerable<BE.GuestRequest> gReqs = from gReq in _dal.GetAllRequests()
                                                 where term(gReq)
                                                 select gReq;

            return gReqs.ToList();
        }

        public List<BE.HostingUnit> AccordingTo(BE.Configuration.Term<BE.HostingUnit> term)
        {
            IEnumerable<BE.HostingUnit> units = from unit in _dal.GetAllHostingUnits()
                                                 where term(unit)
                                                 select unit;

            return units.ToList();
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
            IEnumerable<IGrouping<BE.Enums.Area, BE.HostingUnit>> hostingUnits = from unit in _dal.GetAllHostingUnits()
                                                                                 group unit by unit.Area;
            return hostingUnits.ToList();
        }

        //////////////////////////////////////////////////////////
        // our additional functions:
        
        public bool InCalendar(DateTime time)
        {
            DateTime now = DateTime.Now;
            return (time - now).Days < BE.Configuration._daysInYear;
        }

        /// <summary>
        /// check if dates available to order
        /// </summary>
        private bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, int count)
        {
            // get day and month, value between 0 and 30.
            int month = entryDate.Month - 1, day = entryDate.Day - 1;
            bool[,] diary = hostingUnit.Diary;

            if (diary[month, day] == false)
            {
                // in release day - diary[release day] = false
                // don't even check release day 'cause it doesn't matter
                for (int i = 0; i < count; ++i, ++day)
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
                    if (diary[month, day] == true)
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// this function return the amount of days between the given dates,
        /// BUT in format That in every month the days number is 31
        /// </summary>
        static private int Count2Diary(DateTime entryDate, DateTime releaseDate)
        {
            // return (month*31 + day) - (month*31 + day)
            return (releaseDate.Month * BE.Configuration._days + releaseDate.Day) - 
                (entryDate.Month * BE.Configuration._days + entryDate.Day);
        }
    }
}
