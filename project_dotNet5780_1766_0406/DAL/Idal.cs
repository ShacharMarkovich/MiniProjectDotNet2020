using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public interface IDal
    {
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
        /// <param name="hostingUnit">HostingUnit to remove</param>
        void DeleteHostingUnit(BE.HostingUnit hostingUnit);
        /// <summary>
        /// TODO: what we need to update??
        /// </summary>
        /// <param name="hostingUnit">hostingUnit to update</param>
        /// <param name="update">TODO</param>
        void UpdateHostingUnit(BE.HostingUnit hostingUnit, object update);
        #endregion

        #region Order functions signature
        /// <summary>
        /// this function add the new BE.Order to data source
        /// </summary>
        void AddOrder(BE.Order newOrder);
        /// <summary>
        /// update the given BE.Order to the new BE.Enums.Status
        /// </summary>
        /// <param name="order">exists BE.Order to update </param>
        /// <param name="newStat">new BE.Enums.Status</param>
        void UpdateOrder(BE.Order order, BE.Enums.Status newStat);
        #endregion

        #region  BE classes' Get lists functions signature
        /// <summary>
        /// return all BE.GuestRequest
        /// </summary>
        List<BE.GuestRequest> GetAllRequests();
        /// <summary>
        /// return all BE.HostingUnit
        /// </summary>
        List<BE.HostingUnit> GetAllHostingUnits();
        /// <summary>
        /// return all BE.Order
        /// </summary>
        List<BE.Order> GetAllOrders();
        /// <summary>
        /// return all BE.BankBranch
        /// </summary>
        List<BE.BankBranch> GetAllBranches();
        #endregion
    }
}
