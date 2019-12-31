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

            foreach (BE.Order item in _dal.GetAllOrders())
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
        }

        static void Main(string[] args)
        {
            PrintDS();

            BE.Order newOrder = new BE.Order()
            {
                Status = (BE.Enums.Status)0,
                CreateDate = new DateTime(),
                OrderDate = new DateTime()
            };


            Console.ReadKey();
        }
    }
}
