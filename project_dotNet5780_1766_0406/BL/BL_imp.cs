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
        public static IBL Instance()
        {
            if (_instance == null)
                _instance = new BL_imp();
            return _instance;
        }

        private BL_imp() { _dal = DAL.FactoryDAL.Dal(); }


        /// <summary>
        /// return true if entryDate is at least 1 day earlyer than releaseDate
        /// </summary>
        public bool Ischronological(DateTime entryDate, DateTime releaseDate)
        {
            if (entryDate.Year > releaseDate.Year)
                return false;
            else if (entryDate.Month > releaseDate.Month)
                return false;
            else if (entryDate.Day + 1 > releaseDate.Day)
                return false;
            else
                return true;
        }
        
        public bool IsCollectionClearance(BE.Host host)
        {
            return host.CollectionClearance;
        }
        
        public bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            throw new NotImplementedException("TODO: the check");
            throw new NotImplementedException("maybe targil1 will help");
            
            return false;
        }

        /// <summary>
        /// return true if order stauts is CloseByClient or CloseByApp or Approved
        /// else - return false
        /// </summary>
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
                throw new NotImplementedException("TODO: add");
                throw new NotImplementedException("maybe targil1 will help");
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

        /// <summary>
        /// check if hostingUnit has any open order.
        /// return true if thers isn't.
        /// </summary>
        public bool IsPossibleToDelete(BE.HostingUnit hostingUnit)
        {
            List<BE.Order> orders = _dal.GetAllOrders();
            
            var belongsTo = from order in orders
                            where (order._hostingUnitKey == hostingUnit._hostingUnitKey && !IsOrderClose(order))
                            select order;

            return belongsTo.Count() == 0;
        }

        /// <summary>
        /// check if host has any open order.
        /// return true if thers isn't.
        /// </summary>
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

        /// <summary>
        /// TODO: realy send the mail
        /// </summary>
        public void SendEmail(string HostEmail/*FROM*/, string gReqEmail/*TO*/)
        {
            // TODO: realy send the mail
            Console.WriteLine("{0} is sending email to {1}....", HostEmail, gReqEmail);
        }

        //////////////////////////////////////////////////////////
        public List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int NumberDay)
        {
            return null;
        }
        public int NumberDay(params DateTime[] ArrDate)
        {
            return -1;
        }
        public List<BE.Order> IsOrderBigFromNumber(int NumberDay)
        {
            return null;
        }
        public List<BE.GuestRequest> AccordingTo(Term foo)
        {
            return null;
        }
        public int OrderCount(BE.GuestRequest gReq) { return 0; }
        public int ApprovedCount(BE.HostingUnit hostingUnit) { return 0; }

        public IGrouping<BE.Enums.Area, BE.GuestRequest> GuestRequest_GroupbyArea()
        {
            return null;
        }
        public IGrouping<int, BE.GuestRequest> GuestRequest_GroupbyAmountOfPeople()
        {
            return null;
        }
        public IGrouping<int, BE.Host>Host_GroupbyAmountOfHostingUnit()
        {
            return null;
        }
        public List<IGrouping<BE.Enums.Area, BE.HostingUnit>> HostingUnit_GroupbyArea()
        {
            var Hosting_unit = from Hunit in _dal.GetAllHostingUnits()
                               group Hunit by Hunit.Area;
            return Hosting_unit.ToList();
        }

    }
}
