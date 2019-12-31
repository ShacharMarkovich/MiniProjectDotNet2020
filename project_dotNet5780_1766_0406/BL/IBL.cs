using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// delegate to a term that work on BE.GuestRequest and return true
    /// if gReq meets the condition
    /// </summary>
    /// <param name="gReq">BE.GuestRequest to check</param>
    /// <returns>if gReq meets the condition</returns>
    public delegate bool Term(BE.GuestRequest gReq);


    interface IBL
    {
        //void AddGuestRequest(BE.GuestRequest gRequest);
        //void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat);
        //
        //void AddHostingUnit(BE.HostingUnit newHostingUnit);
        //void DeleteHostingUnit(BE.HostingUnit hostingUnit);
        //void UpdateHostingUnit(BE.HostingUnit hostingUnit, object update);
        //
        //void AddOrder(BE.Order newOrder);
        //void UpdateOrder(BE.Order order, BE.Enums.Status newStat);
        //
        //
        //List<BE.GuestRequest> GetAllRequests();
        //List<BE.HostingUnit> GetAllHostingUnits();
        //List<BE.Order> GetAllOrders();
        //List<BE.BankBranch> GetAllBranches();

        ///////////////////////////////////////////////////////////////////////////
        // in the same order as the functions are in pdf
        bool Ischronological(DateTime entryDate, DateTime releaseDate);
        bool IsCollectionClearance( BE.Host host);
        bool IsDateArmor(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);
        bool IsOrderClose(BE.Order order);
        void TakeFee(BE.Order order);
        void UpdateCalendar(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);
        void SelectInvitation(BE.GuestRequest gReq);
        bool IsPossibleToDelete(BE.HostingUnit hostingUnit);
        bool IsCanCancalCollectionClearance(BE.Host host);
        void SendEmail(BE.Host host/*FROM*/, string gReqEmail/*TO*/);

        //////////////////////////////////////////////////////////
        List<BE.HostingUnit> ListOptionsFree(DateTime entryDate, int daysNumber);
        int DaysNumber(params DateTime[] ArrDate);
        List<BE.Order> IsOrderBigFromNumber(int daysNumber);
        List<BE.GuestRequest> AccordingTo(Term foo);
        int OrderCount(BE.GuestRequest gReq);
        int ApprovedCount(BE.HostingUnit hostingUnit);

        List<IGrouping<BE.Enums.Area, BE.GuestRequest>> GuestRequest_GroupbyArea();
        List<IGrouping<int, BE.GuestRequest>> GuestRequest_GroupbyAmountOfPeople();
        List<IGrouping<int, BE.Host>> Host_GroupbyAmountOfHostingUnit();
        IGrouping<BE.Enums.Area, BE.HostingUnit> HostingUnit_GroupbyArea();


        //////////////////////////////////////////////////////////
        // our additional functions:
        bool InCalendar(DateTime time);
    }
}
