using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public class Order
    {
        // the following 3 bool properties are in order to make the next 3 keys 
        // only once possible to change, like 'readonly' but not in c'tor.
        // we make it because we need to create new instances of this class manually
        [XmlIgnore]
        private bool _hostingUnitKey_setAlready = false;
        [XmlIgnore]
        private bool _guestRequestKey_setAlready = false;
        [XmlIgnore]
        private bool _orderKey_setAlready = false;

        [XmlIgnore]
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

        [XmlIgnore]
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

        [XmlIgnore]
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

        public Enums.Status Status { get; set; }

        public  DateTime CreateDate { get; set; }

        public  DateTime OrderDate { get; set; }
    }
}
