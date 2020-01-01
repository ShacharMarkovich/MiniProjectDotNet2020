using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        /// <summary>
        /// return true if entryDate is at least 1 day earlyer than releaseDate
        /// </summary>
        bool Ischronological(DateTime entryDate, DateTime releaseDate);
        /// <summary>
        /// return true if the host gives approval for debiting bank account
        /// </summary>
        bool IsCollectionClearance(BE.Host host);
        /// <summary>
        /// return true if the number of order days ,Are dates available to order
        /// </summary>
        bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);
        /// <summary>
        /// return true if order stauts is CloseByClient or CloseByApp or Approved
        /// else - return false
        /// </summary>
        bool IsOrderClose(BE.Order order);
        /// <summary>
        /// return void,Take a fee 
        /// </summary>
        void TakeFee(BE.Order order);
        /// <summary>
        /// return void,make hostingUnit's diary beasy in entryDate until releaseDate
        /// </summary>
        void UpdateCalendar(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);
        /// <summary>
        ///return void, Update order selection, and cancel other offers.
        /// </summary>
        void SelectInvitation(BE.Order gReq);
        /// <summary>
        /// check if hostingUnit has any open order.
        /// return true if thers isn't.
        /// </summary>
        bool IsPossibleToDelete(BE.HostingUnit hostingUnit);
        /// <summary>
        /// check if host has any open order.
        /// return true if there is.
        /// </summary>
        bool IsCanCancalCollectionClearance(BE.Host host);
        /// <summary>
        ///return void, TODO: realy send the mail
        /// </summary>
        void SendEmail(BE.Host host/*FROM*/, string gReqEmail/*TO*/);

        //////////////////////////////////////////////////////////
        /// <summary>
        /// Return all available hosting unit on a specific date and to some days specific
        /// </summary>
        List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int daysNumber);
        /// <summary>
        /// return number of days between ArrDate or the number of days past from date given
        /// </summary>
        int DaysNumber(params DateTime[] ArrDate);
        List<BE.Order> AtLeastnDays(int daysNumber);
        List<BE.GuestRequest> AccordingTo(BE.Configuration.Term foo);
        int OrderCount(BE.GuestRequest gReq);
        int ApprovedCount(BE.HostingUnit hostingUnit);

        List<IGrouping<BE.Enums.Area, BE.GuestRequest>> GroupGuestRequestByArea();
        List<IGrouping<int, BE.GuestRequest>> GroupGuestRequestByPeopleCount();
        List<IGrouping<int, BE.Host>> GroupHostByfHostingUnitCount();
        List<IGrouping<BE.Enums.Area, BE.HostingUnit>> GroupHostingUnitByArea();


        //////////////////////////////////////////////////////////
        // our additional functions:
        bool InCalendar(DateTime time);
    }
}
