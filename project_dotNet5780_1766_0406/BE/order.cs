using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        // the following 3 bool properties are in order to make the next 3 keys 
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        private bool _hostingUnitKey_setAlready = false;
        private bool _guestRequestKey_setAlready = false;
        private bool _orderKey_setAlready = false;

        private double _hostingUnitKey;
        public double HostingUnitKey
        {
            get => _hostingUnitKey;
            set
            {
                if (!_hostingUnitKey_setAlready)
                {
                    _hostingUnitKey = value;
                    _hostingUnitKey_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.Order._hostingUnitKey property can only once change!");
            }
        }

        private double _guestRequestKey;
        public double GuestRequestKey
        {
            get => _guestRequestKey;
            set
            {
                if (!_guestRequestKey_setAlready)
                {
                    _guestRequestKey = value;
                    _guestRequestKey_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.Order._guestRequestKey property can only once change!");
            }
        }

        private double _orderKey;
        public double OrderKey
        {
            get => _orderKey;
            set
            {
                if (!_orderKey_setAlready)
                {
                    _orderKey = value;
                    _orderKey_setAlready = true;
                }
                else
                    throw new AccessViolationException("BE.Order._orderKey property can only once change!");
            }
        }

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
