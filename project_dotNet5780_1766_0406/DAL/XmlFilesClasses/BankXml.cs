using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    internal static class BankXml
    {
        const string _xmlLocalPath = @"Banks.xml";
        const string _xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";

        private static void DownloadAtmXml()
        {
            const string xmlLocalPath = @"atm.xml";

            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);

            }
            catch (Exception)
            {
                wc.DownloadFile(_xmlServerPath, xmlLocalPath);
            }
            finally
            {
                wc.Dispose();
            }
        }

        public static IEnumerable<BE.BankBranch> GetAllBankBranch()
        {
            DownloadAtmXml();

            //this work also white xmlServerPath
            XElement xml = XElement.Load(_xmlLocalPath);

            foreach (var item in xml.Elements())
            {
                yield return new BE.BankBranch
                {

                    BankNumber = int.Parse(item.Element("קוד_בנק").Value),
                    BranchNumber = int.Parse(item.Element("קוד_סניף").Value),
                    BankName = item.Element("שם_בנק").Value.Replace('\n', ' ').Trim(),
                    BranchAddress = item.Element("כתובת_ה-ATM").Value.Replace('\n', ' ').Trim(),
                    BranchCity = item.Element("ישוב").Value.Replace('\n', ' ').Trim(),
                };
            }

        }
    }
}
