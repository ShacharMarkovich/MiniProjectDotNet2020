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

        private Enums.Status _status; //status order
        public Enums.Status Status
        {
            set =>_status = value;
            get => _status;
        }
        private DateTime _createDate; // dd.mm.yyyy
        public  DateTime CreateDate
        {
            set => _createDate = value;
            get => _createDate; 
        }
        private DateTime _orderDate; // dd.mm.yyyy
        public  DateTime OrderDate
        {
            set => _orderDate = value; 
            get => _orderDate;
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
