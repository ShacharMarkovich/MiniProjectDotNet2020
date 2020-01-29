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
                throw new ArgumentException("Host Xml File upload problem");
            }
        }

        /// <summary>
        /// add host to xml file
        /// </summary>
        public void AddHost(BE.Host host)
        {
            XElement newHost = CreateHostXElem(host, "Host");
            _hostsRoot.Add(newHost);
            _hostsRoot.Save(_hostsPath);
        }

        public static XElement CreateHostXElem(BE.Host host, string name)
        {
            XElement HostKey = new XElement("HostKey", host.HostKey);
            XElement PrivateName = new XElement("PrivateName", host.PrivateName);
            XElement FamilyName = new XElement("FamilyName", host.FamilyName);
            XElement PhoneNumber = new XElement("PhoneNumber", host.PhoneNumber);
            XElement Email = new XElement("Email", host.Email);
            XElement BankAccountNumber = new XElement("BankAccountNumber", host.BankAccountNumber);
            XElement Balance = new XElement("Balance", host.Balance);
            XElement BankBranchDetails = CreateBankXElem(host.BankBranchDetails);
            XElement CollectionClearance = new XElement("CollectionClearance", host.CollectionClearance);

            return new XElement(name, HostKey, PrivateName, FamilyName, PhoneNumber,
                Email, BankBranchDetails, BankAccountNumber, Balance, CollectionClearance);
        }

        public static BE.Host XElement2Host(XElement host)
        {
            return new BE.Host()
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
                }
            };
        }

        public static XElement CreateBankXElem(BE.BankBranch bankBranchDetails)
        {
            XElement BankNumber = new XElement("BankNumber", bankBranchDetails.BankNumber);
            XElement BankName = new XElement("BankName", bankBranchDetails.BankName);
            XElement BranchNumber = new XElement("BranchNumber", bankBranchDetails.BranchNumber);
            XElement BranchAddress = new XElement("BranchAddress", bankBranchDetails.BranchAddress);
            XElement BranchCity = new XElement("BranchCity", bankBranchDetails.BranchCity);
            return new XElement("BankBranchDetails", BankNumber, BankName, BranchNumber, BranchAddress, BranchCity);
        }

        /// <summary>
        /// update existshost to it new values
        /// </summary>
        public void UpdateHost(BE.Host existshost)
        {
            XElement hostElement = (from host in _hostsRoot.Elements()
                                    where double.Parse(host.Element("HostKey").Value) == existshost.HostKey
                                    select host).FirstOrDefault();

            Update(existshost, hostElement);

            _hostsRoot.Save(_hostsPath);
        }

        private static void Update(BE.Host existshost, XElement hostElement)
        {
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
        }

        public List<BE.Host> GetAllHost()
        {
            List<BE.Host> hosts;
            try
            {
                hosts = (from host in _hostsRoot.Elements()
                         select XElement2Host(host)).ToList();
            }
            catch
            {
                hosts = null;
            }
            return hosts;
        }
    }
}
