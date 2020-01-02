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
        /// return true if entryDate is at least 1 day earlier than releaseDate
        /// </summary>
        bool Ischronological(DateTime entryDate, DateTime releaseDate);
        
        /// <summary>
        /// return true if the host gives approval for debiting bank account
        /// </summary>
        bool IsCollectionClearance(BE.Host host);

        /// <summary>
        /// check if dates between entryDate to releaseDate are available to order.
        /// return true if is.
        /// else - return false.
        /// </summary>
        bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);
        
        /// <summary>
        /// return true if order stauts is CloseByClient or CloseByApp or Approved
        /// else - return false
        /// </summary>
        bool IsOrderClose(BE.Order order);
        
        /// <summary>
        /// Take a fee from matching Host 
        /// </summary>
        void TakeFee(BE.Order order);
        
        /// <summary>
        /// make hostingUnit's diary busy ftom entryDate to releaseDate
        /// </summary>
        void UpdateCalendar(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);

        /// <summary>
        /// update matching GuestRequest's stauts to given order, and cancel other offers.
        /// </summary>
        void SelectInvitation(BE.Order order);
        
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
        /// send mail from host to given email
        /// TODO: realy send the mail
        /// </summary>
        void SendEmail(BE.Host host/*FROM*/, string gReqEmail/*TO*/);


        //////////////////////////////////////////////////////////
        /// <summary>
        /// create list of HostingUnit available from the given date for 'daysNumber' days
        /// </summary>
        List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int daysNumber);
        
        /// <summary>
        /// return number of days between ArrDate or the number of days past from date given
        /// </summary>
        int DaysNumber(params DateTime[] ArrDate);
        
        /// <summary>
        /// return list of all order that the number of days since created is greater than or equal to the number of days received
        /// </summary>
        List<BE.Order> AtLeastnDays(int daysNumber);
        
        /// <summary>
        /// returns a list of all GuestRequest that match a specific condition.
        /// </summary>
        List<BE.GuestRequest> AccordingTo(BE.Configuration.Term foo);
        
        /// <summary>
        /// getting guestRequest and return the number of orders sent to him
        /// </summary>
        int OrderCount(BE.GuestRequest gReq);
        
        /// <summary>
        /// get a hosting unit and returns a namber of all order that have been closed for the hosting unit
        /// </summary>
        int ApprovedCount(BE.HostingUnit hostingUnit);

        /// <summary>
        /// return list of hosting unit group by area
        /// </summary>
        /// <returns></returns>
        List<IGrouping<BE.Enums.Area, BE.GuestRequest>> GroupGuestRequestByArea();
        
        /// <summary>
        /// return list of hosting unit group by amount of people 
        /// </summary>
        /// <returns></returns>
        List<IGrouping<int, BE.GuestRequest>> GroupGuestRequestByPeopleCount();
        
        /// <summary>
        /// return list of host group by hosting unit count they have
        /// </summary>
        /// <returns></returns>
        List<IGrouping<int, BE.Host>> GroupHostByfHostingUnitCount();
        
        /// <summary>
        /// return list of hosting unit group by area
        /// </summary>
        List<IGrouping<BE.Enums.Area, BE.HostingUnit>> GroupHostingUnitByArea();

        //////////////////////////////////////////////////////////
        // our additional functions:

        /// <summary>
        /// return true if time is max 11 months ahead from now
        /// </summary>
        bool InCalendar(DateTime time);
    }
}
