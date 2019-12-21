using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class order
    {
        public double _HostingUnitKey { get; }//number Hosting Unit
        public double _GuestRequestKey { get; }//number Guest Request
        public double _OrderKey { get; }//number order

        private Status _status_inve { get; }//status order
        private string _CreateDate { get; }//day:_ _.moth:_ _.year:_ _ _ _
        private string _OrderDate { get; } //day:_ _.moth:_ _.year:_ _ _ _
        public override string ToString()
        {
            return base.ToString();
        }

    }
}
