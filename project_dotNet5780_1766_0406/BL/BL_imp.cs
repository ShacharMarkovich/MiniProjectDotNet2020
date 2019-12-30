using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class BL_imp : IBL
    {
        private static IBL _instance = null;
        private BL_imp() { }
        public static IBL Instance()
        {
            if (_instance == null)
                _instance = new BL_imp();
            return _instance;
        }


        public bool Ischronological(DateTime entryDate, DateTime releaseDate)
        {
            return false;
        }
        public bool IsCollectionClearance(BE.Host host)
        {
            return false;
        }
        public bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate)
        {
            return false;
        }
        public bool IsOrderClose(BE.Order order)
        {
            return false;
        }
        public void TakeFee(BE.Order order)
        {

        }
        public void UpdateCalendar(BE.Host host, DateTime entryDate, DateTime releaseDate)
        {

        }
        public void SelectInvitation(BE.GuestRequest gReq)
        {

        }
        public bool IsPossibleToDelete(BE.HostingUnit hostingUnit)
        {
            return false;
        }
        public bool IsCanCancalCollectionClearance(BE.Order order, BE.HostingUnit hostingUnit/*maybe this field unnecessary*/)
        {
            return false;
        }
        public void SendEmail(string HostEmail/*FROM*/, string gReqEmail/*TO*/)
        {

        }

        //////////////////////////////////////////////////////////
        public List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int NumberDay)
        {
            return null;
        }
        public int NumberDay(params DateTime[] ArrDate)
        {
            return -1;
        }
        public List<BE.Order> IsOrderBigFromNumber(int NumberDay)
        {
            return null;
        }
        public List<BE.GuestRequest> AccordingTo(Term foo)
        {
            return null;
        }
        public int OrderCount(BE.GuestRequest gReq) { return 0; }
        public int ApprovedCount(BE.HostingUnit hostingUnit) { return 0; }

        public IGrouping<BE.Enums.Area, BE.GuestRequest> GuestRequest_GroupbyArea()
        {
            return null;
        }
        public IGrouping<int, BE.GuestRequest> GuestRequest_GroupbyAmountOfPeople()
        {
            return null;
        }
        public IGrouping<int, BE.Host> Host_GroupbyAmountOfHostingUnit()
        {
            return null;
        }
        public IGrouping<BE.Enums.Area, BE.HostingUnit> HostingUnit_GroupbyArea()
        {
            return null;
        }

    }
}
