using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    // singleton class
    public class Dal_imp : IDal
    {
        private static IDal _instance = null;
        private Dal_imp() { }
        public static IDal Instance()
        {
            if (_instance == null)
                _instance = new Dal_imp();
            return _instance;
        }

        /// <summary>
        /// this function add the new BE.GuestRequest to data source
        /// </summary>
        public void AddGuestRequest(BE.GuestRequest gRequest)
        {
            DS.DataSource._guestRequestsList.Add(gRequest);
        }

        /// <summary>
        /// update the given BE.GuestRequest to the new BE.Enums.Status
        /// </summary>
        /// <param name="gRequest">exists BE.GuestRequest to update </param>
        /// <param name="newStat">new BE.Enums.Status</param>
        public void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat)
        {
            DS.DataSource._guestRequestsList.ForEach(delegate (BE.GuestRequest gReq)
            {
                if (gReq._guestRequestKey == gRequest._guestRequestKey)
                    gReq.Stat = newStat;
            });
        }


        /// <summary>
        /// this function add the new BE.HostingUnit to data source
        /// </summary>
        public void AddHostingUnit(BE.HostingUnit newHostingUnit)
        {
            DS.DataSource._hostingUnitsList.Add(newHostingUnit);
        }

        /// <summary>
        /// delete the given BE.HostingUnit from the data source
        /// </summary>
        /// <param name="hostingUnit">BE.HostingUnit to remove</param>
        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            if (DS.DataSource._hostingUnitsList.Remove(hostingUnit) == false)
                throw new ArgumentException("delete from DS.DataSource._hostingUnitsList not succeed!", "hostingUnit");
        }

        /// <summary>
        /// TODO: what we need to update??
        /// </summary>
        // <param name="hostingUnit"></param>
        // <param name="update"></param>
        public void UpdateHostingUnit(BE.HostingUnit hostingUnit, object update)
        {
            // temp implementation
            throw new NotImplementedException("Dal_imp.UpdateHostingUnit has not yet implemented");
        }


        /// <summary>
        /// this function add the new BE.Order to data source
        /// </summary>
        public void AddOrder(BE.Order newOrder)
        {
            DS.DataSource._ordersList.Add(newOrder);
        }

        /// <summary>
        /// update the given BE.Order to the new BE.Enums.Status
        /// </summary>
        /// <param name="order">exists BE.Order to update </param>
        /// <param name="newStat">new BE.Enums.Status</param>
        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {
            //var a = from order_ in DS.DataSource._ordersList
            //        where order_._orderKey == order._orderKey
            //        select order_;
            //foreach(BE.Order b in a)
            //    b.Status = newStat;
            DS.DataSource._ordersList.ForEach(delegate (BE.Order innerOrder) {
                if (innerOrder._orderKey == order._guestRequestKey)
                    innerOrder.Status = newStat;
            });
        }

        /// <summary>
        /// return all BE.GuestRequest. using Linq
        /// </summary>
        public List<BE.GuestRequest> GetAllRequests()
        {
            IEnumerable<BE.GuestRequest> newList = from gReq in DS.DataSource._guestRequestsList
                                                   orderby gReq.PrivateName
                                                   select gReq;
            return newList.ToList();
        }

        /// <summary>
        /// return all BE.HostingUnit. using Linq
        /// </summary>
        public List<BE.HostingUnit> GetAllHostingUnits()
        {
            var newList = from hostingUnit in DS.DataSource._hostingUnitsList
                          select hostingUnit;
            return newList.ToList();
        }

        /// <summary>
        /// return all BE.Order. using Linq
        /// </summary>
        public List<BE.Order> GetAllOrders()
        {
            var newList = from order in DS.DataSource._ordersList
                          select order;
            return newList.ToList();
        }

        /// <summary>
        /// return all BE.BankBranch. using Linq
        /// </summary>
        public List<BE.BankBranch> GetAllBranches()
        {
            var newList = from bankBranch in DS.DataSource._bankBranchList
                          select bankBranch;
            return newList.ToList();
        }
    }
}
