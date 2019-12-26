using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Dal_imp : IDal
    {
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
            DS.DataSource._guestRequestsList.ForEach(delegate (BE.GuestRequest gReq) {
                if (gReq._guestRequestKey.Equals(gRequest._guestRequestKey))
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
            DS.DataSource._ordersList.ForEach(delegate (BE.Order innerOrder) {
                if (innerOrder._orderKey.Equals(order._guestRequestKey))
                    innerOrder.Status = newStat;
            });
        }


        public List<BE.GuestRequest> GetAllRequests() => DS.DataSource._guestRequestsList;
        public List<BE.HostingUnit> GetAllHostingUnits() => DS.DataSource._hostingUnitsList;
        public List<BE.Order> GetAllOrders() => DS.DataSource._ordersList;
        public List<BE.BankBranch> GetAllBranches() => DS.DataSource._bankBranchList;
    }
}
