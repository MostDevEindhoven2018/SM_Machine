using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_Machine
{
    public class Customer
    {
        public bool Buy(Stock from)
        {
            return true;
        }

        private void Buy(StockItem itemToOrder)
        {
            if (itemToOrder.Stock > 0)
            {
                itemToOrder.Stock--;
            }
            else
            {
                //_waitingCustomers.Enqueue(itemToOrder);
            }
        }
    }
}
