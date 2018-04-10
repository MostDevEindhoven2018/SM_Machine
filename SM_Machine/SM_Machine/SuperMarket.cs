using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SM_Machine
{
    class SuperMarket
    {
        public static Random rand = new Random();
        private Stock _stock;
        private Queue<StockItem> _waitingCustomers = new Queue<StockItem>();

        public Stock SMStock { get => _stock; private set => _stock = value; }

        public long CashFlow { get; set; }
        public long Profit { get; set; }

        public SuperMarket()
        {
            SMStock = new Stock();

            PopulateItems();
            if (SynchronizationContext.Current != null)
            {
                SynchronizationContext.Current.Post(async (obj) =>
                {
                    await StartGeneration();
                }, null);
            } else
            {
                Task.Run(async () =>
                {
                    await StartGeneration();
                });
            }
            //GenerateCustomer();
        }

        private async Task StartGeneration()
        {
            Task t = GenerateCustomer();
            try
            {
                await t;
            }
            catch (Exception e)
            {
                Console.WriteLine("Customers caused and exception");
                Console.WriteLine(e);
            }
        }

        private async Task Order(StockItem item)
        {
            const int orderCount = 100;
            bool locking = System.Threading.SynchronizationContext.Current == null;
            bool run = false;
            if (locking)
            {
                lock (item)
                {
                    if (!item.OrderPlaced)
                    {
                        item.OrderPlaced = true;
                        lock(this)
                        {
                            CashFlow -= item.OrderPrice * orderCount;
                        }
                        run = true;
                    }
                }
            } else
            {
                if (!item.OrderPlaced)
                {
                    item.OrderPlaced = true;
                    run = true;
                }
            }

            if (run)
            {
                await Task.Delay(1000);
                if (locking)
                {
                    lock (item)
                    {
                        item.Stock += orderCount;
                        item.OrderPlaced = false;
                    }
                } else
                {
                    item.Stock += orderCount;
                    item.OrderPlaced = false;
                }
                //Console.WriteLine(item.Name + " has arrived");
                int qCount = _waitingCustomers.Count;
                for (int i = 0; i < qCount; i++)
                {
                    StockItem qItem = _waitingCustomers.Dequeue();
                    Buy(qItem);
                }
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
            item.StockChanged += (value) =>
            {
                if (value < 3)
                {
                    bool locking = SynchronizationContext.Current == null;
                    if (locking)
                    {
                        Task.Run(() => RunOrder(item));
                    }
                    else
                    {
                        SynchronizationContext.Current.Post(async (obj) =>
                        {
                            await RunOrder(item);
                        }, null);
                    }
                }
            };
        }

        private async Task RunOrder(StockItem item)
        {
            Task t = Order(item);
            try
            {
                await t;
            }
            catch (Exception e)
            {
                Console.WriteLine("Item ordering caused an exception");
                Console.WriteLine(e);
            }
        }

        private async Task GenerateCustomer()
        {
            int count = 0;
            while (true)
            {
                int countCopy = count++;
                await Task.Delay(1 + (int)(8 * (rand.NextDouble())));
                StockItem itemToOrder = SMStock[rand.Next(SMStock.Count)];
                Buy(itemToOrder);

                //Console.WriteLine("Hello, I'm customer No:" + countCopy);
            }
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
                    _waitingCustomers.Enqueue(itemToOrder);
                }
            }
            if(bought)
            {
                lock(this)
                {
                    CashFlow += itemToOrder.SellingPrice;
                    Profit += itemToOrder.SellingPrice - itemToOrder.OrderPrice;
                }
            }
        }
    }
}
