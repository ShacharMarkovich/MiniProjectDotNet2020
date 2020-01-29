using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DAL
{
    internal class ConfigurationXml
    {
        // class vars
        private XElement _configurationRoot;
        private string _configurationPath = @"ConfigurationXml.xml";

        #region Singleton
        private static ConfigurationXml _instance = null;
        internal static ConfigurationXml Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigurationXml();
                return _instance;
            }
        }
        private ConfigurationXml()
        {
            if (!File.Exists(_configurationPath))
                CreateFile();
            else
                LoadData();
        }
        #endregion

        /// <summary>
        /// create Configuration xml file
        /// </summary>
        private void CreateFile()
        {
            _configurationRoot = new XElement("Configurations");
            _configurationRoot.Save(_configurationPath);

            // file isn't exists so we assign values in first time
            BE.Configuration.GuestRequestKey = BE.Configuration._firstKey;
            BE.Configuration.BankNumber = BE.Configuration._firstKey;
            BE.Configuration.HostKey = BE.Configuration._firstKey;
            BE.Configuration.HostingUnitKey = BE.Configuration._firstKey;
            BE.Configuration.OrderKey = BE.Configuration._firstKey;

            XElement GuestRequestKey = new XElement("GuestRequestKey", BE.Configuration.GuestRequestKey);
            XElement BankNumber = new XElement("BankNumber", BE.Configuration.BankNumber);
            XElement HostKey = new XElement("HostKey", BE.Configuration.HostKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", BE.Configuration.HostingUnitKey);
            XElement OrderKey = new XElement("OrderKey", BE.Configuration.OrderKey);

            _configurationRoot.Add(GuestRequestKey, BankNumber, HostingUnitKey, HostKey, OrderKey);
            UpdateConfig();
            _configurationRoot.Save(_configurationPath);
        }

        /// <summary>
        /// load xml file and BE.Configuration keys' value
        /// </summary>
        private void LoadData()
        {
            try
            {
                _configurationRoot = XElement.Load(_configurationPath);
            }
            catch
            {
                throw new ArgumentException("Configuration Xml File upload problem");
            }

            List<XElement> conf = _configurationRoot.Elements().ToList();

            // assign keys' value:
            BE.Configuration.GuestRequestKey = double.Parse(conf.Where(xElem => xElem.Name == "GuestRequestKey")
                                                .FirstOrDefault().Value);
            BE.Configuration.BankNumber = double.Parse(conf.Where(xElem => xElem.Name == "BankNumber")
                                                .FirstOrDefault().Value);
            BE.Configuration.HostKey = double.Parse(conf.Where(xElem => xElem.Name == "HostKey")
                                                .FirstOrDefault().Value);
            BE.Configuration.HostingUnitKey = double.Parse(conf.Where(xElem => xElem.Name == "HostingUnitKey")
                                                .FirstOrDefault().Value);
            BE.Configuration.OrderKey = double.Parse(conf.Where(xElem => xElem.Name == "OrderKey")
                                                .FirstOrDefault().Value);
        }
        
        /// <summary>
        /// save keys' value in xml file
        /// </summary>
        public void UpdateConfig()
        {
            _configurationRoot.SetElementValue("GuestRequestKey", BE.Configuration.GuestRequestKey);
            _configurationRoot.SetElementValue("BankNumber", BE.Configuration.BankNumber);
            _configurationRoot.SetElementValue("HostKey", BE.Configuration.HostKey);
            _configurationRoot.SetElementValue("HostingUnitKey", BE.Configuration.HostingUnitKey);
            _configurationRoot.SetElementValue("OrderKey", BE.Configuration.OrderKey);
            _configurationRoot.Save(_configurationPath);
        }
    }
}
