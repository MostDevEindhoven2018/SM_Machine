using System.Threading;

namespace SM_Machine
{
    internal class Cashiere
    {
        private SuperMarket superMarket;
        private Timer timer;

        public Cashiere(SuperMarket superMarket)
        {
            this.superMarket = superMarket;
            timer = new Timer(HelpCustomer, null, SuperMarket.Rand.Next(2000), 1000);
            //timer = new Timer(HelpCustomer, null, 1000, 1000);
        }

        public void HelpCustomer(object o)
        {
            bool success = superMarket.WaitingForCheckoutCustomers.TryTake(out StockItem item, 500);
            if (success)
            {
                lock (superMarket)
                {
                    superMarket.CashFlow += item.SellingPrice;
                    superMarket.Profit += item.SellingPrice - item.OrderPrice;
                    superMarket.CustomersHelped++;
                }
            }
        }
    }
}