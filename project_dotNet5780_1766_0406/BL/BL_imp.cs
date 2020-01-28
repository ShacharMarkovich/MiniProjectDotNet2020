using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Mail;

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

            CheckDates(gRequest.EntryDate, gRequest.ReleaseDate);

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
        public void ApprovedOrder(BE.Order order)
        {
            if (order == null)
                throw new ArgumentException("Please select an order first");
            if (order.Status != Enums.Status.MailSent)
                throw new ArgumentException("Enail must be sent first!");

            GuestRequest gR;
            HostingUnit unit;
            GetUnitNgRFromOrder(order, out gR, out unit);

            // try to add GuestRequest dates to HostingUnit calender and make it busy
            UpdateCalendar(unit, gR.EntryDate, gR.ReleaseDate);
            // update fit statuses
            SelectInvitation(order);
        }

        private void GetUnitNgRFromOrder(Order order, out GuestRequest gR, out HostingUnit unit)
        {
            // get matching BE.GuestRequest and BE.HostingUnit to order
            BE.Configuration.Term<BE.GuestRequest> gruestRequestTerm = gReq => gReq.GuestRequestKey == order.GuestRequestKey;
            BE.Configuration.Term<BE.HostingUnit> hostingUnitTerm = u => u.HostingUnitKey == order.HostingUnitKey;
            try
            {
                gR = AccordingTo(gruestRequestTerm).Single();
                unit = AccordingTo(hostingUnitTerm).Single();
            }
            catch
            {
                throw new ArgumentException("more than one GuestRequest or HostingUnit to this Order!!!");
            }
        }

        public void CreateOrder(BE.Order newOrder)
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
            if (order == null)
                throw new ArgumentException("please select an order first!");

            if (IsOrderClose(order.clone()))
                throw new ArgumentException("can't change close order!");
            
            _dal.UpdateOrder(order.clone(), newStat);
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
            return entryDate < releaseDate;
        }

        public bool IsCollectionClearance(BE.Host host)
        {
            return host.CollectionClearance;
        }

        public bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            try
            {
                CheckDates(entryDate, releaseDate);
            }
            catch
            {
                return false;
            }

            BE.HostingUnit newUnit = hostingUnit.clone();
            for (DateTime date = entryDate; date < releaseDate; date = date.AddDays(1))
                //month + 1, in the diary we Holding first month for backwards calendar
                if (newUnit.Diary[date.Month, date.Day - 1])
                    return false;

            return true;
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
            hostingUnit = UpdateDairy(hostingUnit.clone(), entryDate, releaseDate);

            // if succeeded update hosting unit
            UpdateHostingUnit(hostingUnit.clone());
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
            _dal.UpdateOrder(order, BE.Enums.Status.Approved);

            // update status of matching guest request to approved too
            _dal.UpdateGuestRequest(request.clone(), Enums.Status.Approved);
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

        public void SendEmail(Order order)
        {
            //   Order;GuestRequest gR;
            HostingUnit hosting_unit;
            GuestRequest guest_Req;
            GetUnitNgRFromOrder(order, out guest_Req, out hosting_unit);
            if (IsCollectionClearance(hosting_unit.Owner))
            {
                //MailMessage
               MailMessage mail = new MailMessage();
              mail.To.Add(guest_Req.Email);
                mail.From = new MailAddress(hosting_unit.Owner.Email);
              mail.Subject = "Booking a hosting unit";
              mail.Body = "Hello,/n We were pleased to see that you were interested in booking a accommodation unit./n We would love to be at your service";
              mail.IsBodyHtml = true;
              SmtpClient smtp = new SmtpClient();
              smtp.Host = "smtp.gmail.com";
              smtp.Port = 25;
              smtp.Credentials = new System.Net.NetworkCredential("dotnet2020.liorandshachar@gmail.com",
              "Ll123123");
              smtp.EnableSsl = true;
            }
            else
                throw new ArgumentException("host not has a CollectionClearance!");
        }

        //////////////////////////////////////////////////////////

        public List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int daysNumber)
        {
            DateTime releaseDate = entryDate.AddDays(daysNumber);
            List<BE.HostingUnit> free = (from unit in _dal.GetAllHostingUnits()
                                         where IsDateArmor(unit, entryDate, releaseDate)
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
            try
            {
                _dal.GetAllRequests();
            }
            catch
            {
                return new List<GuestRequest>();
            }

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

        public List<IGrouping<string, BE.BankBranch>> GetAllBranchesGroupByBank()
        {
            return (from atm in _dal.GetAllBranches()
                    group atm by atm.BankName).ToList();
        }

        public List<IGrouping<string, BE.BankBranch>> GetAllBranchesGroupByCity()
        {
            return (from atm in _dal.GetAllBranches()
                    group atm by atm.BranchCity).ToList();
        }

        public List<string> GetAllBankNames()
        {
            return (from atm in _dal.GetAllBranches()
                    select atm.BankName).Distinct().ToList();
        }

        public List<BE.BankBranch> GetAllBank(string bankName)
        {
            return (from atm in _dal.GetAllBranches()
                    where atm.BankName.CompareTo(bankName) == 0
                    select atm).Distinct().ToList();
        }

        public List<IGrouping<string, IGrouping<string, BE.BankBranch>>> GetAllBranchesGroupByBankAndCity()
        {
            List<IGrouping<string, IGrouping<string, BE.BankBranch>>> queryNestedGroups =
                (from atm in _dal.GetAllBranches().Distinct()
                 orderby atm.BankName, atm.BranchCity, atm.BankNumber// optional for sort list
                 group atm by atm.BankName into g  // group by bank name
                 from newGroup2 in (from atm2 in g   // foreach bank name grouping now group by city
                                    group atm2 by atm2.BranchCity)
                 group newGroup2 by g.Key).ToList();

            return queryNestedGroups;
        }



        //////////////////////////////////////////////////////////
        // our additional functions:

        /// <summary>
        /// return true if time in this year
        /// </summary>
        public bool InCalendar(DateTime time) => time < DateTime.Today.AddMonths(BE.Configuration._month - 1);

        /// <summary>
        /// try to make the diary of hostingUnit busy between entryDate to releaseDate
        /// </summary>
        /// <returns>return update the hostingUnit</returns>
        private BE.HostingUnit UpdateDairy(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            BE.HostingUnit newUnit = hostingUnit.clone();
            // check if dates free
            if (!IsDateArmor(newUnit, entryDate, releaseDate))
                throw new ArgumentException("Date already taken");

            // update diary
            for (DateTime date = entryDate; date < releaseDate; date = date.AddDays(1))
                newUnit.Diary[date.Month, date.Day - 1] = true;

            return newUnit;
        }

        /// <summary>
        /// check if guestRequest dates are legal
        /// </summary>
        private void CheckGuestRequestDates(BE.GuestRequest guestRequest) => CheckDates(guestRequest.EntryDate, guestRequest.ReleaseDate);

        /// <summary>
        /// check if given dates are legal
        /// if illegal - throw ArgumentException with fit message
        /// </summary>
        private void CheckDates(DateTime entryDate, DateTime releaseDate)
        {
            if (!InCalendar(entryDate) || !InCalendar(releaseDate))
                throw new ArgumentException("Release Date or Entry Date is not in this year!");

            if (!Ischronological(entryDate, releaseDate))
                throw new ArgumentException("Dates are not chronological!");

            if (entryDate < DateTime.Today || releaseDate < DateTime.Today)
                throw new ArgumentException("Release Date or Entry Date not valid, date already passed");
        }

        public List<BankBranch> GetAllBanks(string bankName)
        {
            return _dal.GetAllBranches().Where(bank => bank.BankName== bankName).ToList();
        }

        public List<IGrouping<string, BankBranch>> GetAllBankBranchGroupByBank()
        {
            throw new NotImplementedException();
        }

        public List<IGrouping<string, BankBranch>> GetAllBankBranchGroupByCity()
        {
            throw new NotImplementedException();
        }

        public List<IGrouping<string, IGrouping<string, BankBranch>>> GetAllBankBranchGroupByBankAndCity()
        {
            throw new NotImplementedException();
        }
    }
}
