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
                PrintStock(supermarket.SMStock, supermarket.Profit, supermarket.CashFlow);
            }
        }

        private static void PrintStock(Stock stock, long profit, long cashflow)
        {
            Console.WriteLine("---------------------");
            foreach (var a in stock) {
                Console.WriteLine(a);
            }
            Console.WriteLine($"Profit:\t{profit}\nClashFlow:\t{cashflow}");
        }
    }
}
