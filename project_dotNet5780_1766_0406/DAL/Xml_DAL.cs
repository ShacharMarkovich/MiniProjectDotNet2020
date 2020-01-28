using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DAL
{
    internal class Xml_DAL : IDal
    {
        private ConfigurationXml _configXml;
        private GuestRequestXml _guestRequestXml;
        private HostXml _hostXml;
        private HostingUnitXml _hostingUnitXml;
        private OrderXml _orderXml;

        #region singleton
        private static IDal _instance = null;

        internal static IDal Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Xml_DAL();
                return _instance;
            }
        }
        private Xml_DAL()
        {
            _configXml = ConfigurationXml.Instance;
            _guestRequestXml = GuestRequestXml.Instance;
            _hostXml = HostXml.Instance;
            _orderXml = OrderXml.Instance;
            _hostingUnitXml = HostingUnitXml.Instance;
        }
    #endregion

    public void AddGuestRequest(BE.GuestRequest guestRequest)
        {
            List<BE.GuestRequest> list = _guestRequestXml.LoadListFromXML();
            list.Add(guestRequest.clone());
            _guestRequestXml.SaveListToXML(list);
        }

        public void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat)
        {
            List<BE.GuestRequest> list = _guestRequestXml.LoadListFromXML();
            list.ForEach(delegate (BE.GuestRequest gReq)
            {
                if (gReq.GuestRequestKey == gRequest.GuestRequestKey)
                    gReq.Stat = newStat;
            });
            _guestRequestXml.SaveListToXML(list);
        }


        public void AddHost(BE.Host newHost) => _hostXml.AddHost(newHost.clone());

        public void UpdateHost(BE.Host host) => _hostXml.UpdateHost(host.clone());


        public void AddHostingUnit(BE.HostingUnit newHostingUnit)
        {
            List<BE.HostingUnit> list = _hostingUnitXml.LoadListFromXML();
            list.Add(newHostingUnit);
            _hostingUnitXml.SaveListToXML(list);
        }

        public void UpdateHostingUnit(BE.HostingUnit hostingUnit)
        {
            List<BE.HostingUnit> list = _hostingUnitXml.LoadListFromXML();
            list[list.FindIndex(key => key.HostingUnitKey == hostingUnit.HostingUnitKey)] = hostingUnit.clone();
            _hostingUnitXml.SaveListToXML(list);
        }

        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            List<BE.HostingUnit> list = _hostingUnitXml.LoadListFromXML();
            BE.HostingUnit unit2Del = list.Find(unit => unit.HostingUnitKey == hostingUnit.HostingUnitKey);
            if (list.Remove(unit2Del) == false)
                throw new ArgumentException("delete hosting unit from Data Source didn't succeed!");
            _hostingUnitXml.SaveListToXML(list);
        }


        public void AddOrder(BE.Order newOrder)
        {
            List<BE.Order> list = _orderXml.LoadListFromXML();
            list.Add(newOrder.clone());
            _orderXml.SaveListToXML(list);
        }

        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {
            List<BE.Order> list = _orderXml.LoadListFromXML();
            list.ForEach(delegate (BE.Order innerOrder)
            {
                if (innerOrder.OrderKey == order.OrderKey)
                    innerOrder.Status = newStat;
            });

            _orderXml.SaveListToXML(list);
        }


        public List<BE.HostingUnit> GetAllHostingUnits() => _hostingUnitXml.LoadListFromXML();

        public List<BE.Host> GetAllHosts() => _hostXml.GetAllHost();

        public List<BE.Order> GetAllOrders() => _orderXml.LoadListFromXML();

        public List<BE.GuestRequest> GetAllRequests() => _guestRequestXml.LoadListFromXML();

        public List<BE.BankBranch> GetAllBranches() => BankXml.GetAllBankBranch().ToList();
    }
}
