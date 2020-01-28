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
    internal class HostXml
    {
        // class vars
        private XElement _hostsRoot;
        private string _hostsPath = @"HostXml.xml";

        #region Singleton
        private static HostXml _instance = null;
        internal static HostXml Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HostXml();
                return _instance;
            }
        }

        private HostXml()
        {
            if (!File.Exists(_hostsPath))
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
            _hostsRoot = new XElement("Hosts");
            _hostsRoot.Save(_hostsPath);
        }

        /// <summary>
        /// load xml file
        /// </summary>
        private void LoadData()
        {
            try
            {
                _hostsRoot = XElement.Load(_hostsPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }

        /// <summary>
        /// add host to xml file
        /// </summary>
        public void AddHost(BE.Host host)
        {
            XElement HostKey = new XElement("HostKey", host.HostKey);
            XElement PrivateName = new XElement("PrivateName", host.PrivateName);
            XElement FamilyName = new XElement("FamilyName", host.FamilyName);
            XElement PhoneNumber = new XElement("PhoneNumber", host.PhoneNumber);
            XElement Email = new XElement("Email", host.Email);

            XElement BankBranchDetails = new XElement("BankBranchDetails", host.BankBranchDetails.BankNumber,
                host.BankBranchDetails.BankName, host.BankBranchDetails.BranchNumber,
                host.BankBranchDetails.BranchAddress, host.BankBranchDetails.BranchCity);

            XElement BankAccountNumber = new XElement("BankAccountNumber", host.BankAccountNumber);
            XElement Balance = new XElement("Balance", host.Balance);
            XElement CollectionClearance = new XElement("CollectionClearance", host.CollectionClearance);

            _hostsRoot.Add(new XElement("Host", HostKey, PrivateName, FamilyName, PhoneNumber, Email,
                BankBranchDetails, BankAccountNumber, Balance, CollectionClearance));

            _hostsRoot.Save(_hostsPath);
        }

        /// <summary>
        /// update existshost to it new values
        /// </summary>
        public void UpdateHost(BE.Host existshost)
        {
            XElement hostElement = (from host in _hostsRoot.Elements()
                                       where double.Parse(host.Element("HostKey").Value) == existshost.HostKey
                                       select host).FirstOrDefault();

            hostElement.Element("PrivateName").Value = existshost.PrivateName;
            hostElement.Element("FamilyName").Value = existshost.FamilyName;
            hostElement.Element("Email").Value = existshost.Email;
            hostElement.Element("PhoneNumber").Value = existshost.PhoneNumber;

            hostElement.Element("BankAccountNumber").Value = existshost.BankAccountNumber.ToString();
            hostElement.Element("BankBranchDetails").Element("BankNumber").Value = existshost.BankBranchDetails.BankNumber.ToString();
            hostElement.Element("BankBranchDetails").Element("BranchNumber").Value = existshost.BankBranchDetails.BranchNumber.ToString();
            hostElement.Element("BankBranchDetails").Element("BankName").Value = existshost.BankBranchDetails.BankName;
            hostElement.Element("BankBranchDetails").Element("BranchCity").Value = existshost.BankBranchDetails.BranchCity;
            hostElement.Element("BankBranchDetails").Element("BranchAddress").Value = existshost.BankBranchDetails.BranchAddress;

            hostElement.Element("CollectionClearance").Value = existshost.CollectionClearance.ToString();

            _hostsRoot.Save(_hostsPath);
        }

        public List<BE.Host> GetAllHost()
        {
            List<XElement> list =_hostsRoot.Elements().ToList();
            List<BE.Host> hosts;
            try
            {
                hosts = (from host in _hostsRoot.Elements()
                         select new BE.Host()
                         {
                             HostKey = double.Parse(host.Element("HostKey").Value),
                             PrivateName = host.Element("PrivateName").Value,

                             FamilyName = host.Element("FamilyName").Value,
                             Email = host.Element("Email").Value,
                             PhoneNumber = host.Element("PhoneNumber").Value,
                             Balance = double.Parse(host.Element("Balance").Value),
                             BankAccountNumber = double.Parse(host.Element("BankAccountNumber").Value),
                             CollectionClearance = bool.Parse(host.Element("CollectionClearance").Value),
                             BankBranchDetails = new BE.BankBranch()
                             {
                                 BankNumber = double.Parse(host.Element("BankBranchDetails").Element("BankNumber").Value),
                                 BankName = host.Element("BankBranchDetails").Element("BankName").Value,
                                 BranchNumber = int.Parse(host.Element("BankBranchDetails").Element("BranchNumber").Value),
                                 BranchAddress = host.Element("BankBranchDetails").Element("BranchAddress").Value,
                                 BranchCity = host.Element("BankBranchDetails").Element("BranchCity").Value,
                             },
                         }).ToList();
            }
            catch
            {
                hosts = null;
            }
            return hosts;
        }
    }
}
