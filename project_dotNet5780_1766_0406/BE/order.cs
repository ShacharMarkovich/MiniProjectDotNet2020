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
        // TODO: do matching between order to hosting unit and guest request
        public readonly double _hostingUnitKey = ++BE.Configuration.HostingUnitKey;
        public readonly double _guestRequestKey = ++BE.Configuration.GuestRequestKey;
        public readonly double _orderKey = ++BE.Configuration.OrderKey;

        private Enums.Status _status;
        public Enums.Status Status
        {
            set =>_status = value;
            get => _status;
        }

        private DateTime _createDate;
        public  DateTime CreateDate
        {
            set => _createDate = value;
            get => _createDate; 
        }

        private DateTime _orderDate;
        public  DateTime OrderDate
        {
            set => _orderDate = value; 
            get => _orderDate;
        }
        /// <summary>
        /// swap to string
        /// </summary>
        public override string ToString()
        {

            return "Order Key:\t\t" + _orderKey +
                "\nHosting Unit Key:\t" + _hostingUnitKey +
                "\nGuest Request Key:\t" + _guestRequestKey +
                "\nOrder Status:\t" + _status +
                "\nOrder Creation Date:\t" + _createDate.toString() +
                "\nMail sent Date:\t" + _orderDate.toString();
        }
    }
}
