using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Dal_imp : IDal
    {
        public void AddGuestRequest(BE.GuestRequest gRequest)
        {

        }
        public void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat)
        {

        }

        public void AddHostingUnit(BE.HostingUnit newHostingUnit)
        {

        }
        public void DeleteHostingUnit(BE.HostingUnit newHostingUnit)
        {

        }
        public void UpdateHostingUnit(BE.HostingUnit newHostingUnit)
        {

        }

        public void AddOrder(BE.Order newOrder)
        {

        }
        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {

        }

        public List<BE.GuestRequest> GetAllRequests() => DS.DataSource._guestRequestsList;
        public List<BE.HostingUnit> GetAllHostingUnits() => DS.DataSource._hostingUnitsList;
        public List<BE.Order> GetAllOrders() => DS.DataSource._ordersList;
        public List<BE.BankBranch> GetAllBranches() => DS.DataSource._bankBranchList;
    }
}
