using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public interface IDal
    {
        void AddGuestRequest(BE.GuestRequest gRequest);
        void UpdateGuestRequest(BE.GuestRequest gRequest, BE.Enums.Status newStat);

        void AddHostingUnit(BE.HostingUnit newHostingUnit);
        void DeleteHostingUnit(BE.HostingUnit hostingUnit);
        void UpdateHostingUnit(BE.HostingUnit hostingUnit, object update);
       
        void AddOrder(BE.Order newOrder);
        void UpdateOrder(BE.Order order, BE.Enums.Status newStat);

        List<BE.GuestRequest> GetAllRequests(); 
        List<BE.HostingUnit>  GetAllHostingUnits();
        List<BE.Order>        GetAllOrders();
        List<BE.BankBranch>   GetAllBranches();

    }
}
