using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    class HostingUnit
    {
        public double _HostingUnitKey { get; }//number Hosting Unit 
        private Host _Owner { get; }// host
        private string HostingUnitName { get; }//name Hosting Unit 
        private Diary _diary { get; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
