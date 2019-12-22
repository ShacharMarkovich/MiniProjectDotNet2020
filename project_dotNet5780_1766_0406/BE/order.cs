using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        public readonly double _hostingUnitKey;
        public readonly double _guestRequestKey;
        public readonly double _orderKey;

        private Enums.Status _status { get; }//status order
        private string _createDate { get; }// dd.mm.yyyy
        private string _orderDate { get; } // dd.mm.yyyy

        public override string ToString() => base.ToString();

    }
}
