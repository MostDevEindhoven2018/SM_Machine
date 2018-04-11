using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SM_Machine
{
    public class ConsoleVersion
    {
        public static void Main(String[] args)
        {
            SuperMarket supermarket = new SuperMarket();
            
            //supermarket.SMStock.ItemChanged += () => {
            //    PrintStock(supermarket.SMStock, supermarket.Profit, supermarket.CashFlow);
            //};

            while(true)
            {
                Thread.Sleep(500);
                lock (supermarket)
                {
                    PrintStock(supermarket);
                }
            }
        }

        static long prevCustArrived = 0;

        private static void PrintStock(SuperMarket sm)
        {
            Console.WriteLine("---------------------");
            foreach (var a in sm.SMStock) {
                Console.WriteLine(a);
            }
            Console.WriteLine($"Profit:\t{sm.Profit}\nClashFlow:\t{sm.CashFlow}");
            Console.WriteLine($"Cust in Line:\t{sm.WaitingForCheckoutCustomers.Count}");
            long arrived = sm.CustomersArrived;
            Console.WriteLine($"Helped: {sm.CustomersHelped}, Arrived: {arrived}, In store {sm.WaitingForProductCustomers.Count}");
            Console.WriteLine($"Arrived per sec: {(arrived - prevCustArrived) *2}");
            prevCustArrived = arrived;
        }
    }
}
