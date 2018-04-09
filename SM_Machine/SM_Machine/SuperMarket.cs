using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_Machine
{
    class SuperMarket
    {
        public static Random rand = new Random();
        private Stock _stock;
        private ObservableCollection<Customer> _customers;
        private List<WaitingLine> _waitingLines;

        public Stock SMStock { get => _stock; private set => _stock = value; }
        public ObservableCollection<Customer> Customers { get => _customers; private set => _customers = value; }
        public List<WaitingLine> WaitingLines { get => _waitingLines; private set => _waitingLines = value; }

        public SuperMarket()
        {
            SMStock = new Stock();
            Customers = new ObservableCollection<Customer>();
            WaitingLines = new List<WaitingLine>();

            populateItems();
            generateCustomer();
        }

        private void populateItems()
        {
            SMStock.Add(new StockItem()
            {
                Name = "Milk",
                OrderPrice = 50,
                SellingPrice = 100,
                Stock = 5,
            });
            SMStock.Add(new StockItem()
            {
                Name = "Bread",
                OrderPrice = 100,
                SellingPrice = 100,
                Stock = 5,
            });
            SMStock.Add(new StockItem()
            {
                Name = "Box of Eggs",
                OrderPrice = 60,
                SellingPrice = 120,
                Stock = 5,
            });
            SMStock.Add(new StockItem()
            {
                Name = "Bananar",
                OrderPrice = 10,
                SellingPrice = 15,
                Stock = 5,
            });
            SMStock.Add(new StockItem()
            {
                Name = "Voters loyalty",
                OrderPrice = 175,
                SellingPrice = 500,
                Stock = 5,
            });
        }

        private async Task generateCustomer()
        {
            int count = 0;
            while (true)
            {
                int countCopy = count++;
                await Task.Delay(1000 + (int) (1000 * (rand.NextDouble())));
                SMStock[rand.Next(SMStock.Count)].Stock--;
                Console.WriteLine("Hello, I'm customer No:" + countCopy);
            }
        }
    }
}
