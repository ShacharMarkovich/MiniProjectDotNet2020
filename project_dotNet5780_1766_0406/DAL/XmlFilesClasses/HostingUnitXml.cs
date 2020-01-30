using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;

namespace DAL
{
    internal class HostingUnitXml
    {
        // class vars
        private XElement _hostingUnitsRoot;
        private string _hostingUnitsPath = @"HostingUnitXml.xml";

        #region Singleton
        private static HostingUnitXml _instance = null;
        internal static HostingUnitXml Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HostingUnitXml();
                return _instance;
            }
        }

        private HostingUnitXml()
        {
            if (!File.Exists(_hostingUnitsPath))
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
            _hostingUnitsRoot = new XElement("HostingUnits");
            _hostingUnitsRoot.Save(_hostingUnitsPath);
            SaveListToXML(new List<HostingUnit>());
        }

        /// <summary>
        /// load xml file
        /// </summary>
        private void LoadData()
        {
            try
            {
                _hostingUnitsRoot = XElement.Load(_hostingUnitsPath);
            }
            catch
            {
                throw new ArgumentException("HostingUnits Xml File upload problem");
            }
        }

        public void SaveListToXML(List<BE.HostingUnit> list)
        {
            FileStream file = new FileStream(_hostingUnitsPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }

        public List<BE.HostingUnit> LoadListFromXML()
        {
            FileStream file = new FileStream(_hostingUnitsPath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BE.HostingUnit>));
            List<BE.HostingUnit> list = (List<BE.HostingUnit>)xmlSerializer.Deserialize(file);
            file.Close();
            return list;
        }
    }
}
