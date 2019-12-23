using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class DataSource
    {
        public static List<BE.GuestRequest> _guestRequestsList;
        public static List<BE.HostingUnit> _hostingUnitsList;
        public static List<BE.Order> _ordersList;
        public static List<BE.BankBranch> _bankBranchList;

        public DataSource()
        {
            // TODO: Manual initialization
            _guestRequestsList = new List<BE.GuestRequest>()
            {
                new BE.GuestRequest()
                {
                    PrivateName = "D",
                    Pool = false
                }
            };
            _hostingUnitsList = new List<BE.HostingUnit>();
            _ordersList = new List<BE.Order>();
            _bankBranchList = new List<BE.BankBranch>();
        }
    }
}
