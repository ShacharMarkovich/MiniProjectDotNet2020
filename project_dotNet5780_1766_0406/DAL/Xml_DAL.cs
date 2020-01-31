using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Controls;

namespace DAL
{

    internal class Xml_DAL : IDal
    {
        private ConfigurationXml _configXml;
        private GuestRequestXml _guestRequestXml;
        private HostXml _hostXml;
        private HostingUnitXml _hostingUnitXml;
        private OrderXml _orderXml;

        private Thread _bankThread;
        public bool _isEnd { get; set; }
        private List<BE.BankBranch> _banksDetails;

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

            _isEnd = false;
            _bankThread = new Thread(GetBanks);
            _bankThread.Start();
        }

        public void Join() => _bankThread.Join();

        public bool IsEnd() => _isEnd;

        private void GetBanks()
        {
            _banksDetails = BankXml.GetAllBankBranch().Distinct().ToList();
            _isEnd = true;
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
            for (int i = 0; i < list.Count; ++i)
                if (list[i].GuestRequestKey == gRequest.GuestRequestKey)
                {
                    list[i] = gRequest.clone();
                    list[i].Stat = newStat;
                }

            _guestRequestXml.SaveListToXML(list);
        }

        public void UpdateConfig() => _configXml.UpdateConfig();


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
            list.ForEach(delegate(BE.HostingUnit unit)
            {
                if (unit.HostingUnitKey == hostingUnit.HostingUnitKey)
                {
                    unit.HostingUnitName = hostingUnit.HostingUnitName;
                    unit.DatesRange = hostingUnit.DatesRange;
                    unit.Owner.Balance = hostingUnit.Owner.Balance;
                }
            });
            _hostingUnitXml.SaveListToXML(list);
        }

        public void DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            List<BE.HostingUnit> list = _hostingUnitXml.LoadListFromXML();
            int count = list.RemoveAll(unit => unit.HostingUnitKey == hostingUnit.HostingUnitKey);
            _hostingUnitXml.SaveListToXML(list);
            if (count == 0)
                throw new ArgumentException("delete hosting unit from Data Source didn't succeed!");
        }


        public void AddOrder(BE.Order newOrder)
        {
            List<BE.Order> list =_orderXml.LoadListFromXML();
            list.Add(newOrder);
            _orderXml.SaveListToXML(list);
        }

        public void UpdateOrder(BE.Order order2Update, BE.Enums.Status newStat)
        {
            List<BE.Order> list = _orderXml.LoadListFromXML();
            list.ForEach(delegate (BE.Order order)
            {
                if (order.OrderKey == order2Update.OrderKey)
                {
                    order.Status = newStat;
                    if (newStat == BE.Enums.Status.MailSent)
                        order.OrderDate = DateTime.Today;
                }
            });
            _orderXml.SaveListToXML(list);
        }


        public List<BE.HostingUnit> GetAllHostingUnits() => _hostingUnitXml.LoadListFromXML();

        public List<BE.Host> GetAllHosts() => _hostXml.GetAllHost();

        public List<BE.Order> GetAllOrders() => _orderXml.LoadListFromXML();

        public List<BE.GuestRequest> GetAllRequests() => _guestRequestXml.LoadListFromXML();

        public List<BE.BankBranch> GetAllBranches() => _banksDetails;

    }
}
