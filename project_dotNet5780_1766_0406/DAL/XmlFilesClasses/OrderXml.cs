using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    class OrderXml
    {
        // class vars
        private XElement _ordersRoot;
        private const string _ordersPath = @"OrderXml.xml";

        #region Singleton
        private static OrderXml _instance = null;
        internal static OrderXml Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderXml();
                return _instance;
            }
        }

        private OrderXml()
        {
            if (!File.Exists(_ordersPath))
                CreateFile();
            else
                LoadData();
        }
        #endregion

        /// <summary>
        /// create GuestRequest xml file
        /// </summary>
        private void CreateFile()
        {
            _ordersRoot = new XElement("Orders");
            _ordersRoot.Save(_ordersPath);
        }

        /// <summary>
        /// load xml file
        /// </summary>
        private void LoadData()
        {
            try
            {
                _ordersRoot = XElement.Load(_ordersPath);
            }
            catch
            {
                throw new ArgumentException("Order Xml File upload problem");
            }
        }


        public void AddOrder(BE.Order order)
        {
            XElement OrderKey = new XElement("OrderKey", order.OrderKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", order.HostingUnitKey);
            XElement GuestRequestKey = new XElement("GuestRequestKey", order.GuestRequestKey);
            XElement CreateDate = new XElement("CreateDate", order.CreateDate);
            XElement OrderDate = new XElement("OrderDate", order.OrderDate);
            XElement Status = new XElement("Status", order.Status);

            _ordersRoot.Add(new XElement("Order", OrderKey, HostingUnitKey,
                GuestRequestKey, CreateDate, OrderDate, Status));
            _ordersRoot.Save(_ordersPath);
        }

        public void UpdateOrder(BE.Order order, BE.Enums.Status newStat)
        {
            XElement orderElement = (from or in _ordersRoot.Elements()
                                    where double.Parse(or.Element("OrderKey").Value) == order.OrderKey
                                    select or).FirstOrDefault();

            orderElement.Element("Status").Value = newStat.ToString();

            //if (newStat == BE.Enums.Status.MailSent)
            //    orderElement.Element("OrderDate").Value = DateTime.Now.ToString();

            _ordersRoot.Save(_ordersPath);
        }

        public List<BE.Order> GetAllOrders()
        {
            List<BE.Order> orders;
            try
            {
                orders = (from order in _ordersRoot.Elements()
                         select new BE.Order()
                         {
                             OrderKey = double.Parse(order.Element("OrderKey").Value),
                             GuestRequestKey = double.Parse(order.Element("GuestRequestKey").Value),
                             HostingUnitKey = double.Parse(order.Element("HostingUnitKey").Value),
#pragma warning disable CS0618 // Type or member is obsolete
                             CreateDate = XmlConvert.ToDateTime(order.Element("CreateDate").Value),
                             OrderDate = XmlConvert.ToDateTime(order.Element("OrderDate").Value),
#pragma warning restore CS0618 // Type or member is obsolete
                             Status = (BE.Enums.Status)Enum.Parse(typeof(BE.Enums.Status),order.Element("Status").Value)
                         }
                         ).ToList();
            }
            catch
            {
                orders = null;
            }
            return orders;
        }
    }
}
