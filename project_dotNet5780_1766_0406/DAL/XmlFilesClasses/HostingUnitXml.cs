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
    internal class HostingUnitXml
    {
        // class vars
        private XElement _hostingUnitRoot;
        private string _hostingUnitPath = @"GuestRequestXml.xml";

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
            if (!File.Exists(_hostingUnitPath))
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
            _hostingUnitRoot = new XElement("HostingUnits");
            _hostingUnitRoot.Save(_hostingUnitPath);
            //init empty list
            SaveListToXML(new List<BE.HostingUnit>());
        }

        /// <summary>
        /// load xml file
        /// </summary>
        private void LoadData()
        {
            try
            {
                _hostingUnitRoot = XElement.Load(_hostingUnitPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        public void SaveListToXML(List<BE.HostingUnit> list)
        {
            FileStream file = new FileStream(_hostingUnitPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }
        public List<BE.HostingUnit> LoadListFromXML()
        {
            FileStream file = new FileStream(_hostingUnitPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BE.HostingUnit>));
            List<BE.HostingUnit> list = (List<BE.HostingUnit>)xmlSerializer.Deserialize(file);
            file.Close();
            return list;
        }
    }
}
