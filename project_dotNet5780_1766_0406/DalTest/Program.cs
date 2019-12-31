using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalTest
{
    class Program
    {
        private static DAL.IDal _dal = DAL.FactoryDAL.Instance;

        private static void PrintDS()
        {
            foreach (BE.GuestRequest item in _dal.GetAllRequests())
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");

            foreach (BE.HostingUnit item in _dal.GetAllHostingUnits())
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");

            foreach (BE.BankBranch item in _dal.GetAllBranches())
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");

            foreach (BE.Order item in _dal.GetAllOrders())
            {
                Console.WriteLine(item);
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }

        private static void AddnewOrder()
        {
            List<BE.GuestRequest> rList = _dal.GetAllRequests();
            BE.Order newOrder = new BE.Order()
            {
                HostingUnitKey = _dal.GetAllHostingUnits()[1].HostingUnitKey,
                OrderKey = ++BE.Configuration.OrderKey,
                GuestRequestKey = rList[0].GuestRequestKey,
                Status = rList[0].Stat,
                CreateDate = new DateTime(),
                OrderDate = new DateTime()
            };

            _dal.AddOrder(newOrder);
            _dal.UpdateOrder(newOrder, BE.Enums.Status.Approved);
        }

        static void Main(string[] args)
        {
            PrintDS();
            AddnewOrder();
            PrintDS();

            Console.ReadKey();
        }

    }
}
