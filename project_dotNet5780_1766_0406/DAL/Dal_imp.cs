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

        private Dal_imp() { DS.DataSource.Init(); }
        internal static IDal Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Dal_imp();
                return _instance;
            }
        }

        public void AddGuestRequest(BE.GuestRequest gRequest)
        {
            try
            {
                DS.DataSource._guestRequestsList.Add(gRequest);
            }
            catch (Exception exp)
            {
                var str = exp.Message;
            }
        }

        public void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat)
        {
            DS.DataSource._guestRequestsList.ForEach(delegate (BE.GuestRequest gReq)
            {
                if (gReq.GuestRequestKey == gRequest.GuestRequestKey)
                    gReq.Stat = newStat;
            });
        }

        public void AddHostingUnit(BE.HostingUnit newHostingUnit)
        {
            DS.DataSource._hostingUnitsList.Add(newHostingUnit);
        }

        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            if (DS.DataSource._hostingUnitsList.Remove(hostingUnit) == false)
                throw new ArgumentException("delete from DS.DataSource._hostingUnitsList not succeed!", "hostingUnit");
        }

        public void UpdateHostingUnit(BE.HostingUnit hostingUnit)
        {
            DS.DataSource._hostingUnitsList[DS.DataSource._hostingUnitsList.FindIndex(
                key => key.HostingUnitKey == hostingUnit.HostingUnitKey)] = hostingUnit;
        }

        public void AddOrder(BE.Order newOrder)
        {
            DS.DataSource._ordersList.Add(newOrder);
        }

        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {
            DS.DataSource._ordersList.ForEach(delegate (BE.Order innerOrder)
            {
                if (innerOrder.OrderKey == order.OrderKey)
                    innerOrder.Status = newStat;
            });
        }

        public void AddHost(BE.Host newHost)
        {
            DS.DataSource._hostsList.Add(newHost);
        }

        public void UpdateHost(BE.Host host)
        {
            DS.DataSource._hostsList[DS.DataSource._hostsList.FindIndex(
                key => key.HostKey == host.HostKey)] = host;
        }


        public List<BE.GuestRequest> GetAllRequests()
        {
            IEnumerable<BE.GuestRequest> newList = from gReq in DS.DataSource._guestRequestsList
                                                   orderby gReq.PrivateName
                                                   select gReq.clone();
            return newList.ToList();
        }

        public List<BE.HostingUnit> GetAllHostingUnits()
        {
            IEnumerable<BE.HostingUnit> newList = from hostingUnit in DS.DataSource._hostingUnitsList
                                                  select hostingUnit.clone();
            return newList.ToList();
        }

        public List<BE.Order> GetAllOrders()
        {
            IEnumerable<BE.Order> newList = from order in DS.DataSource._ordersList
                                            select order.clone();
            return newList.ToList();
        }

        public List<BE.BankBranch> GetAllBranches()
        {

            IEnumerable<BE.BankBranch> newList = from bankBranch in DS.DataSource._bankBranchList
                                                 select bankBranch.clone();
            return newList.ToList();
        }

        public List<BE.Host> GetAllHosts()
        {
            return (from host in DS.DataSource._hostsList
                    select host.clone()).ToList();
        }

    }
}
