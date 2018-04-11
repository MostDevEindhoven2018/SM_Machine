using System;
using System.Threading;
using System.Threading.Tasks;

namespace SM_Machine
{
    public class Chief
    {
        private const int OrderCount = 200;
        private SuperMarket _superMarket;
        Timer timer;

        public void CheckStock(object o)
        {
            System.Threading.Tasks.Parallel.ForEach(_superMarket.SMStock, (item, state, i) => CheckItemStock(item));
        }

        private void CheckItemStock(StockItem item)
        {
            bool startOrder = false;
            lock (item)
            {
                if(item.Stock < 3 && !item.OrderPlaced)
                {
                    item.OrderPlaced = true;
                    startOrder = true;
                }
            }
            if(startOrder)
            {
                lock(_superMarket)
                {
                    _superMarket.CashFlow -= item.OrderPrice * OrderCount;
                }
                Task.Delay(1000).ContinueWith((o) => OrderItem(item));
            }
        }

        private void OrderItem(StockItem item)
        {
            lock(item)
            {
                item.Stock += OrderCount;
                item.OrderPlaced = false;
            }
            _superMarket.CheckCustomersWaitingInStore();
        }

        public Chief(SuperMarket sm)
        {
            _superMarket = sm;
            timer = new Timer(CheckStock, null, 200, 200);
        }
    }
}