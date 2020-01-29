using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        #region IDAL functions

        #region GuestRequest functions signature
        /// <summary>
        /// this function add the new BE.GuestRequest to the data source
        /// </summary>
        void AddGuestRequest(BE.GuestRequest gRequest);
        /// <summary>
        /// update the given BE.GuestRequest to the new BE.Enums.Status
        /// </summary>
        /// <param name="gRequest">exists BE.GuestRequest to update </param>
        /// <param name="newStat">new BE.Enums.Status</param>
        void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat);
        #endregion

        #region HostingUnit functions signature
        /// <summary>
        /// this function add the new BE.HostingUnit to data source
        /// </summary>
        void AddHostingUnit(BE.HostingUnit newHostingUnit);
        /// <summary>
        /// delete the given BE.HostingUnit from the data source
        /// </summary>
        void DeleteHostingUnit(BE.HostingUnit hostingUnit);
        /// <summary>
        /// update the matching unit in DS to his new filed
        /// </summary>
        void UpdateHostingUnit(BE.HostingUnit hostingUnit);
        #endregion

        #region Order functions signature
        /// <summary>
        /// this function try to approved this order
        /// </summary>
        void ApprovedOrder(BE.Order newOrder);
        /// <summary>
        /// this function create the new BE.Order to data source
        /// </summary>
        void CreateOrder(BE.Order newOrder);
        /// <summary>
        /// update the given BE.Order to the new BE.Enums.Status
        /// </summary>
        void UpdateOrder(BE.Order order, BE.Enums.Status newStat);
        #endregion

        /// <summary>
        /// this function add new BE.Host to the data source
        /// </summary>
        void AddHost(BE.Host newHost);

        /// <summary>
        /// this function get BE.Host that exists in datasource and update it
        /// </summary>
        void UpdateHost(BE.Host newHost);

        /// <summary>
        /// this function return list of all Hosts in the dataSource
        /// </summary>
        List<BE.Host> GetAllHosts();
        #endregion


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
        /// return true if gReq stauts is CloseByClient or CloseByApp or Approved
        /// else - return false
        /// </summary>
        bool IsGuestRequestClose(BE.GuestRequest gReq);

        /// <summary>
        /// Take a fee from matching Host 
        /// </summary>
        void TakeFee(BE.Order order);
        
        /// <summary>
        /// make hostingUnit's diary busy ftom entryDate to releaseDate
        /// </summary>
        void UpdateCalendar(BE.HostingUnit hostingUnit, DateTime entryDate, DateTime releaseDate);

        /// <summary>
        /// update status of given order to approved,
        /// close all other open orders of the matching guest request,
        /// and update status of matching guest request to approved too
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
        void SendEmail(BE.Order order);


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
        /// returns a list of all Orders that match a specific condition.
        /// </summary>
        List<BE.Order> AccordingTo(BE.Configuration.Term<BE.Order> term);

        /// <summary>
        /// returns a list of all GuestRequest that match a specific condition.
        /// </summary>
        List<BE.GuestRequest> AccordingTo(BE.Configuration.Term<BE.GuestRequest> foo);

        /// <summary>
        /// returns a list of all HostingUnit that match a specific condition.
        /// </summary>
        List<BE.HostingUnit> AccordingTo(BE.Configuration.Term<BE.HostingUnit> term);

        /// <summary>
        /// getting guestRequest and return the number of orders sent to him
        /// </summary>
        int OrderCount(BE.GuestRequest gReq);
        
        /// <summary>
        /// get a hosting unit and returns a number of all order that have been closed for the hosting unit
        /// </summary>
        int ApprovedCount(BE.HostingUnit hostingUnit);

        /// <summary>
        /// return list of hosting unit group by area
        /// </summary>
        List<IGrouping<BE.Enums.Area, BE.GuestRequest>> GroupGuestRequestByArea();
        
        /// <summary>
        /// return list of hosting unit group by amount of people 
        /// </summary>
        List<IGrouping<int, BE.GuestRequest>> GroupGuestRequestByPeopleCount();
        
        /// <summary>
        /// return list of host group by hosting unit count they have
        /// </summary>
        List<IGrouping<int, BE.Host>> GroupHostByfHostingUnitCount();
        
        /// <summary>
        /// return list of hosting unit group by area
        /// </summary>
        List<IGrouping<BE.Enums.Area, BE.HostingUnit>> GroupHostingUnitByArea();

        List<IGrouping<string, BE.BankBranch>> GetAllBankBranchGroupByBank();

        List<IGrouping<string, BE.BankBranch>> GetAllBankBranchGroupByCity();

        List<string> GetAllBankNames();

        List<BE.BankBranch> GetAllBanks(string bankName);

        List<IGrouping<string, IGrouping<string, BE.BankBranch>>> GetAllBankBranchGroupByBankAndCity();


        //////////////////////////////////////////////////////////
        // our additional functions:

        /// <summary>
        /// return true if time is max 11 months ahead from now
        /// </summary>
        bool InCalendar(DateTime time);

        void UpdateConfig();


    }
}
