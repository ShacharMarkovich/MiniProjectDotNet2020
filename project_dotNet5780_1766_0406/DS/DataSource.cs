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
                new BE.GuestRequest("name1", "lest_name1", "1@gmail.com", 0, "00.00.0000", "11.11.1111", "22.22.2222", 0, 0, 0, 1, 1, false, true, false, true),
                new BE.GuestRequest("name2", "lest_name2", "2@gmail.com", 0, "00.00.0000", "11.11.1111", "22.22.2222", 0, 0, 0, 1, 1, false, true, false, true),
                new BE.GuestRequest("name3", "lest_name3", "3@gmail.com", 0, "00.00.0000", "11.11.1111", "22.22.2222", 0, 0, 0, 1, 1, false, true, false, true)
            };
            _hostingUnitsList = new List<BE.HostingUnit>();
            _ordersList = new List<BE.Order>();
            _bankBranchList = new List<BE.BankBranch>();
        }
    }
}
