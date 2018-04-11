using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SM_Machine
{
    public class SuperMarket
    {
        public static Random Rand { get; private set; } = new Random();
        private Stock _stock;
        public ConcurrentQueue<StockItem> WaitingForProductCustomers { get; private set; } = new ConcurrentQueue<StockItem>();
        public BlockingCollection<StockItem> WaitingForCheckoutCustomers { get; private set; } = new BlockingCollection<StockItem>();

        public Stock SMStock { get => _stock; private set => _stock = value; }

        public long CashFlow { get; set; }
        public long Profit { get; set; }

        public long CustomersArrived { get; private set; } = 0;
        public long CustomersHelped { get; set; } = 0;

        private Chief Chief;
        private List<Cashiere> _cashieres;
        private Timer custTimer;

        public SuperMarket()
        {
            SMStock = new Stock();

            PopulateItems();
            Chief = new Chief(this);
            _cashieres = new List<Cashiere>();
            const int cashiereCount = 51;
            for (int i = 0; i < cashiereCount; i++)
            {
                _cashieres.Add(new Cashiere(this));
            }
            //_cashieres.Add(new Cashiere(this));

            if (SynchronizationContext.Current != null)
            {
                SynchronizationContext.Current.Post((obj) =>
                {
                    GenerateCustomer();
                }, null);
            }
            else
            {
                GenerateCustomer();
            }
        }

        private void PopulateItems()
        {
            StockItem milk = new StockItem()
            {
                Name = "Milk",
                OrderPrice = 50,
                SellingPrice = 100,
                Stock = 5,
            };
            AddStockItem(milk);

            StockItem bread = new StockItem()
            {
                Name = "Bread",
                OrderPrice = 100,
                SellingPrice = 100,
                Stock = 5,
            };
            AddStockItem(bread);

            StockItem eggs = new StockItem()
            {
                Name = "Box of Eggs",
                OrderPrice = 60,
                SellingPrice = 120,
                Stock = 5,
            };
            AddStockItem(eggs);

            StockItem bananar = new StockItem()
            {
                Name = "Bananar",
                OrderPrice = 10,
                SellingPrice = 15,
                Stock = 5,
            };
            AddStockItem(bananar);

            StockItem loyalty = new StockItem()
            {
                Name = "Voters loyalty",
                OrderPrice = 175,
                SellingPrice = 500,
                Stock = 5,
            };
            AddStockItem(loyalty);
        }

        /// <summary>
        /// Initialize method that adds an item to the super market.
        /// This method is not expected to run after initialization.
        /// This method also adds listeners to the items to ensure 
        /// orders are placed when the item is about to run out.
        /// </summary>
        /// <param name="item"></param>
        private void AddStockItem(StockItem item)
        {
            SMStock.Add(item);
        }

        private void GenerateCustomer()
        {
            bool useStopWatch = true;
            if (!useStopWatch)
            {
                custTimer = new Timer((a) =>
                {
                    Task.Delay((int)(18 * (Rand.NextDouble()))).ContinueWith((_) =>
                    {
                        CustomerEntersStore();
                    });
                }, null, 100, 100);
            }
            else
            {
                Stopwatch stopwatch = new Stopwatch();
                const int maxRand = 41;
                const int custPerTick = 1;
                long prevMS = 0;
                long tillNextMs = Rand.Next(maxRand);
                stopwatch.Start();
                custTimer = new Timer((a) =>
                {
                    long now = stopwatch.ElapsedMilliseconds;
                    long ellapsed = now - prevMS;

                    while (ellapsed >= tillNextMs)
                    {
                        ellapsed -= tillNextMs;
                        for (int i = 0; i < custPerTick; i++)
                        {
                            CustomerEntersStore();
                        }
                        tillNextMs = Rand.Next(maxRand); ;
                    }
                    tillNextMs -= ellapsed;
                    prevMS = now;
                }, null, 200, 200);
            }
        }

        private void CustomerEntersStore()
        {
            CustomersArrived++;
            StockItem itemToOrder = SMStock[Rand.Next(SMStock.Count)];
            Buy(itemToOrder);
        }

        private void Buy(StockItem itemToOrder)
        {
            bool bought = false;
            lock (itemToOrder)
            {
                if (itemToOrder.Stock > 0)
                {
                    itemToOrder.Stock--;
                    bought = true;
                }
                else
                {
                    //Console.WriteLine("customer has to wait");
                    WaitingForProductCustomers.Enqueue(itemToOrder);
                }
            }
            if (bought)
            {
                WaitingForCheckoutCustomers.Add(itemToOrder);
            }
        }

        public void CheckCustomersWaitingInStore()
        {
            int count = WaitingForProductCustomers.Count;
            for (int i = 0; i < count && (WaitingForProductCustomers.TryDequeue(out StockItem item)); i++)
            {
                Buy(item);
            }
            if (WaitingForProductCustomers.Count >= count)
            {
                Console.WriteLine("wtf");
            }
        }
    }
}
