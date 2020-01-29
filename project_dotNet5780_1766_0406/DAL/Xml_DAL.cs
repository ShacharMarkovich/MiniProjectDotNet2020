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

        private readonly List<BE.BankBranch> _banksDetails;

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
            _banksDetails = BankXml.GetAllBankBranch().Distinct().ToList();
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

        public void UpdateConfig() => _configXml.UpdateConfig();


        public void AddHost(BE.Host newHost) => _hostXml.AddHost(newHost.clone());

        public void UpdateHost(BE.Host host) => _hostXml.UpdateHost(host.clone());


        public void AddHostingUnit(BE.HostingUnit newHostingUnit) => _hostingUnitXml.AddHostingUnit(newHostingUnit.clone());

        public void UpdateHostingUnit(BE.HostingUnit hostingUnit) => _hostingUnitXml.UpdateHostingUnit(hostingUnit.clone());

        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            if (!_hostingUnitXml.DeleteHostingUnit(hostingUnit.clone()))
                throw new ArgumentException("delete hosting unit from Data Source didn't succeed!");
        }


        public void AddOrder(BE.Order newOrder) => _orderXml.AddOrder(newOrder.clone());

        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat) => _orderXml.UpdateOrder(order, newStat);


        public List<BE.HostingUnit> GetAllHostingUnits() => _hostingUnitXml.GetAllHostingUnit();

        public List<BE.Host> GetAllHosts() => _hostXml.GetAllHost();

        public List<BE.Order> GetAllOrders() => _orderXml.GetAllOrders();

        public List<BE.GuestRequest> GetAllRequests() => _guestRequestXml.LoadListFromXML();

        public List<BE.BankBranch> GetAllBranches() => _banksDetails;
    }
}
