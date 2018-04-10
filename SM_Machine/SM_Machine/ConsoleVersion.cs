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
            
            supermarket.SMStock.ItemChanged += () => {
                PrintStock(supermarket.SMStock);
            };

            while(true)
            {
                Thread.Sleep(50000000);
            }
        }

        private static void PrintStock(Stock stock)
        {
            Console.WriteLine("---------------------");
            foreach (var a in stock) {
                Console.WriteLine(a);
            }
        }
    }
}
