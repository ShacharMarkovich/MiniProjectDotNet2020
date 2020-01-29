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

        /// <summary>
        /// add host to xml file
        /// </summary>
        public void AddHostingUnit(BE.HostingUnit unit)
        {
            XElement hostingUnit = CreateHostingUnitXElem(unit);
            _hostingUnitsRoot.Add(hostingUnit);
            _hostingUnitsRoot.Save(_hostingUnitsPath);
        }

        public bool DeleteHostingUnit(BE.HostingUnit hostingUnit)
        {
            XElement hostingUnitElement;
            try
            {
                hostingUnitElement = (from unit in _hostingUnitsRoot.Elements()
                                  where double.Parse(unit.Element("HostingUnitKey").Value) == hostingUnit.HostingUnitKey
                                      select unit).FirstOrDefault();
                hostingUnitElement.Remove();
                _hostingUnitsRoot.Save(_hostingUnitsPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// crate new XElement from given HostingUnit
        /// </summary>
        private static XElement CreateHostingUnitXElem(BE.HostingUnit unit)
        {
            XElement HostingUnitKey = new XElement("HostingUnitKey", unit.HostingUnitKey);
            XElement HostingUnitName = new XElement("HostingUnitName", unit.HostingUnitName);
            XElement Owner = HostXml.CreateHostXElem(unit.Owner, "Owner");
            XElement type = new XElement("type", unit.type);
            XElement Area = new XElement("Area", unit.Area);
            XElement Diary = new XElement("Diary", unit.XmlDiary);
            return new XElement("HostingUnit", HostingUnitKey, HostingUnitName, Owner, type, Area, Diary);
        }

        /// <summary>
        /// update existshost to it new values
        /// </summary>
        public void UpdateHostingUnit(BE.HostingUnit unit)
        {
            // get hostingUnitElement which match to given unit key
            XElement hostingUnitElement = _hostingUnitsRoot.Elements()
                .Where(hostingUnit =>
                double.Parse(hostingUnit.Element("HostingUnitKey").Value) == unit.HostingUnitKey)
                .FirstOrDefault();

            // update values in xmk file
            // (only those properties can change, so only them update)
            hostingUnitElement.Element("HostingUnitName").Value = unit.HostingUnitName;
            hostingUnitElement.Element("Diary").Value = unit.XmlDiary;

            _hostingUnitsRoot.Save(_hostingUnitsPath);
        }

        /// <summary>
        /// return all hostingUnits as list
        /// </summary>
        public List<BE.HostingUnit> GetAllHostingUnit()
        {
            List<BE.HostingUnit> hostingUnits;
            try
            {
                hostingUnits = (from hostingUnit in _hostingUnitsRoot.Elements()
                                select new BE.HostingUnit()
                                {
                                    HostingUnitKey = double.Parse(hostingUnit.Element("HostingUnitKey").Value),
                                    HostingUnitName = hostingUnit.Element("HostingUnitName").Value,
                                    Owner = HostXml.XElement2Host(hostingUnit.Element("Owner")),
                                    Area = (BE.Enums.Area)Enum.Parse(typeof(BE.Enums.Area), hostingUnit.Element("Area").Value),
                                    type = (BE.Enums.UnitType)Enum.Parse(typeof(BE.Enums.UnitType), hostingUnit.Element("type").Value),
                                    XmlDiary = hostingUnit.Element("Diary").Value,
                                }).ToList();
            }
            catch
            {
                hostingUnits = null;
            }

            return hostingUnits;
        }
    }
}
