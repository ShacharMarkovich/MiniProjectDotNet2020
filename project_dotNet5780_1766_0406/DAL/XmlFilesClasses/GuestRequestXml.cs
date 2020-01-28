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
    internal class GuestRequestXml
    {
        // class vars
        private XElement _guestRequestRoot;
        private string _guestRequestPath = @"GuestRequestXml.xml";

        #region Singleton
        private static GuestRequestXml _instance = null;
        internal static GuestRequestXml Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GuestRequestXml();
                return _instance;
            }
        }

        private GuestRequestXml()
        {
            if (!File.Exists(_guestRequestPath))
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
            _guestRequestRoot = new XElement("GuestRequests");
            _guestRequestRoot.Save(_guestRequestPath);
            //init empty list
            SaveListToXML(new List<BE.GuestRequest>());
        }

        /// <summary>
        /// load xml file
        /// </summary>
        private void LoadData()
        {
            try
            {
                _guestRequestRoot = XElement.Load(_guestRequestPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        public void SaveListToXML(List<BE.GuestRequest> list)
        {
            FileStream file = new FileStream(_guestRequestPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }
        public List<BE.GuestRequest> LoadListFromXML()
        {
            FileStream file = new FileStream(_guestRequestPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<BE.GuestRequest>));
            List<BE.GuestRequest> list = (List<BE.GuestRequest>)xmlSerializer.Deserialize(file);
            file.Close();
            return list;
        }
    }
}
