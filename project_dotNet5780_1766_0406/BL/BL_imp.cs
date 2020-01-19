using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            List<GuestRequest> units = AccordingTo(delegate (BE.GuestRequest unit) { return unit.GuestRequestKey == gRequest.GuestRequestKey; });
            if (units.Count() != 0)
                throw new ArgumentException("BE.HostingUnit.HostingUnitKey already exists");

            if (!(InCalendar(gRequest.RegistrationDate) && InCalendar(gRequest.EntryDate) &&
                InCalendar(gRequest.ReleaseDate) && Ischronological(gRequest.EntryDate, gRequest.ReleaseDate)))
                throw new ArgumentException("dates are not in this year / not chronological! try again!");

            if (gRequest.PrivateName == null || gRequest.FamilyName == null)
                throw new ArgumentException("please enter data in all fields");

            if (gRequest.PrivateName.Length == 0 || gRequest.FamilyName.Length == 0)
                throw new ArgumentException("names illegal");

            try
            {
                // if this line not throw an Exception - gRequest.Email is a legal email
                new System.Net.Mail.MailAddress(gRequest.Email);
            }
            catch
            {
                throw new ArgumentException("email illegal");
            }

            if ((gRequest.Adults == 0 && gRequest.Children == 0) || (gRequest.Adults == 0 && gRequest.Children != 0))
                throw new ArgumentException("Must have at least one adult");


            _dal.AddGuestRequest(gRequest.clone());
        }

        public void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat)
        {
            if (!IsGuestRequestClose(gRequest))
                _dal.UpdateGuestRequest(gRequest.clone(), newStat);
            else
                throw new ArgumentOutOfRangeException("can't change close Guest Request!");
        }
        #endregion

        #region HostingUnit functions signature
        public void AddHostingUnit(BE.HostingUnit newHostingUnit)
        {
            if (newHostingUnit.HostingUnitName == null)
                throw new ArgumentException("less Hosting Unit Name");

            List<HostingUnit> units = AccordingTo(delegate (BE.HostingUnit unit) { return unit.HostingUnitKey == newHostingUnit.HostingUnitKey; });
            if (units.Count() != 0)
                throw new ArgumentException("BE.HostingUnit.HostingUnitKey already exists");
            // TODO in part 3: check if newHostingUnit.Owner.BankBranchDetails is exists in israel

            _dal.AddHostingUnit(newHostingUnit.clone());
        }

        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            if (IsPossibleToDelete(hostingUnit))
                _dal.DeleteHostingUnit(hostingUnit.clone());
            else
                throw new ArgumentException($"cannot delete hostingunit({hostingUnit.HostingUnitKey}/({hostingUnit.HostingUnitName})" +
                    $"needed to first close all the orders)");
        }

        public void UpdateHostingUnit(HostingUnit hostingUnit)
        {
            if (hostingUnit.HostingUnitName == null || hostingUnit.HostingUnitName.Length == 0)
                throw new ArgumentException("ilegal name");

            List<HostingUnit> units = AccordingTo(delegate (BE.HostingUnit unit) { return unit.HostingUnitKey == hostingUnit.HostingUnitKey; });
            if (units.Count() == 0)
                throw new ArgumentException("please select an hosting unit");
            _dal.UpdateHostingUnit(hostingUnit.clone());
        }
        #endregion

        #region Order functions signature
        public void AddOrder(BE.Order newOrder)
        {
            // check if there is allready order with this keys
            List<BE.Order> ordersList = AccordingTo(delegate (BE.Order order) {
                return order.GuestRequestKey == newOrder.GuestRequestKey &&
                order.HostingUnitKey == newOrder.HostingUnitKey;
            });

            if (ordersList.Count() != 0)
                throw new ArgumentException("order allready exists for this GuestRequest in this HostingUnit");

                List<BE.GuestRequest> guestRequestList = AccordingTo(delegate (BE.GuestRequest gReq) { return gReq.GuestRequestKey == newOrder.GuestRequestKey; });
            List<HostingUnit> hostingUnitList = AccordingTo(delegate (BE.HostingUnit unit) { return unit.HostingUnitKey == newOrder.HostingUnitKey; });

            int requestsCount = guestRequestList.Count;
            int unitsCount = hostingUnitList.Count;
            // == 0 means that there isn't exists unit/request with those keys
            if (requestsCount == 0 || unitsCount == 0)
                throw new ArgumentException("unfamiliar GuestRequest or HostingUnit with those keys!please try again!");

            BE.GuestRequest matchRG = guestRequestList.Single();
            BE.HostingUnit matchUnit = hostingUnitList.Single();
            if (IsGuestRequestClose(matchRG))
                throw new ArgumentException("can't make order to close GuestRequest!");

            // check if areas and type are match:
            if (matchRG.Area != matchUnit.Area)
                throw new ArgumentException("Areas does not match");
            if (matchRG.type != matchUnit.type)
                throw new ArgumentException("Types does not match");

            // check if hosting unit calendar is free between those dates
            if (!IsDateArmor(matchUnit, matchRG.EntryDate, matchRG.ReleaseDate))
                throw new ArgumentOutOfRangeException($"dates between {matchRG.EntryDate.toString()} to {matchRG.EntryDate.toString()} are not available to order");

            _dal.AddOrder(newOrder.clone());
        }

        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {
            if (!IsOrderClose(order.clone()))
                _dal.UpdateOrder(order.clone(), newStat);
            else
                throw new ArgumentOutOfRangeException("can't change close order!");
        }
        #endregion

        public void AddHost(BE.Host newHost)
        {
            CheckHostDetails(newHost);

            IEnumerable<Host> s = from host in _dal.GetAllHosts()
                                  where host.HostKey == newHost.HostKey
                                  select host;
            if (s.Count() != 0)
            {
                BE.Configuration.HostKey++;
                throw new ArgumentException("HostKey allready exists");
            }

            _dal.AddHost(newHost.clone());
        }

        public void UpdateHost(BE.Host host)
        {
            CheckHostDetails(host);

            // check if host in DataSource
            IEnumerable<Host> s = from hostDS in _dal.GetAllHosts()
                                  where hostDS.HostKey == host.HostKey
                                  select hostDS;
            if (s.Count() == 0)
            {
                ///BE.Configuration.HostKey++;
                throw new ArgumentException("HostKey not exists");
            }

            _dal.UpdateHost(host);
        }

        /// <summary>
        /// this function get BE.Host and check if all the details are legal
        /// </summary>
        private void CheckHostDetails(BE.Host newHost)
        {
            try
            {
                int.Parse(newHost.PhoneNumber);
            }
            catch
            {
                throw new ArgumentException("Phone number must be a number");
            }
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

            if (!IsCollectionClearance(newHost))
                throw new ArgumentException("Collection Clearance must be checked!");

            try
            {
                new System.Net.Mail.MailAddress(newHost.Email);
            }
            catch
            {
                throw new ArgumentException("email illegal");
            }
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
                for (int i = 0; i < count; i++, ++day)
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
                _dal.UpdateGuestRequest(request, BE.Enums.Status.Approved);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException("there is no such GuestRequestKey!");
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("more than one GuestRequest with this key!");
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

            List<Order> list = belongsTo.ToList();
            int v = list.Count();
            var s = v == 0;
            return s;
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

        public List<BE.Order> AccordingTo(BE.Configuration.Term<BE.Order> term)
        {
            IEnumerable<BE.Order> orders = from order in _dal.GetAllOrders()
                                                where term(order)
                                                select order;
            return orders.ToList();
        }

        public List<BE.HostingUnit> AccordingTo(BE.Configuration.Term<BE.HostingUnit> term)
        {
            IEnumerable<BE.HostingUnit> units = from unit in _dal.GetAllHostingUnits()
                                                where term(unit)
                                                select unit;

            return units.ToList();
        }

        public List<BE.GuestRequest> AccordingTo(BE.Configuration.Term<BE.GuestRequest> term)
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
        /// BUT in format That in every month there is 31 days
        /// </summary>
        static private int Count2Diary(DateTime entryDate, DateTime releaseDate)
        {
            // return (month*31 + day) - (month*31 + day)
            return (releaseDate.Month * BE.Configuration._days + releaseDate.Day) -
                (entryDate.Month * BE.Configuration._days + entryDate.Day);
        }
    }
}
