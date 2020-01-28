using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    class OrderXml
    {
        // class vars
        private XElement _ordersRoot;
        private string _ordersPath = @"OrderXml.xml";

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
            //init empty list
            SaveListToXML(new List<BE.Order>());
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
                throw new Exception("File upload problem");
            }
        }

        public void SaveListToXML(List<BE.Order> list)
        {
            FileStream file = new FileStream(_ordersPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }

        public List<BE.Order> LoadListFromXML()
        {
            FileStream file = new FileStream(_ordersPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BE.Order>));
            List<BE.Order> list = (List<BE.Order>)xmlSerializer.Deserialize(file);
            file.Close();
            return list;
        }
    }
}
