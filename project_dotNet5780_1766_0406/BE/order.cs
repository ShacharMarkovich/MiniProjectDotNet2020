using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// in the next part of the project we will make this class singleton
    public class Order
    {
        public readonly double _hostingUnitKey;
        public readonly double _guestRequestKey;
        public readonly double _orderKey;

        private Enums.Status _status { get; set; }//status order
        public Enums.Status Status
        {
            set { _status = value; }
        }
        private string _createDate { get; set; }// dd.mm.yyyy
        public string CreateDate
        {
            set { _createDate = value; }
        }
        private string _orderDate { get; set; } // dd.mm.yyyy
        public string OrderDate
        {
            set { _orderDate = value; }
        }

        public override string ToString()
        {
            return "Hosting Unit Key:\t" + _hostingUnitKey +
                "\nGuest Request Key:\t" + _guestRequestKey +
                "\nOrder Key:\t" + _orderKey +
                "\nStatus:\t" + _status +
                "\nCreate Date:\t" + _createDate +
                "\nOrder Date:\t" + _orderDate;
        }
    }
}
