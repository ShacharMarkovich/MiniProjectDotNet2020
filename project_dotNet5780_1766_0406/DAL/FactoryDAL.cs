using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class FactoryDAL
    {
        public static IDal Dal()
        {
                return Dal_imp.Instance();
                //case "XML": return DAL_XML.Instance();
        }
    }
}
