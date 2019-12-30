using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Enums
    {
        public enum Status
        {
            NotYetApproved, MailSent, CloseByClient, CloseByApp, Approved
        }

        public enum Area
        {
            Jerusalem, North, South, Center
        }

        // optional
        public enum SubArea
        {

        }

        public enum UnitType
        {
            Zimmer, Hotel, Camping, RentingRoom
        }
    }
}
