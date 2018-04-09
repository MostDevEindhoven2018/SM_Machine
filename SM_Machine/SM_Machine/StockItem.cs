using System;

namespace SM_Machine
{
    public class StockItem
    {
        private int _stock;

        public string Name { get; set; }
        public int SellingPrice { get; set; }
        public int OrderPrice { get; set; }
        public int Stock
        {
            get => _stock;
            set
            {
                if (_stock != value) {
                    _stock = value;
                    StockChanged?.Invoke(value);
                }
            }
        }
        public bool OrderPlaced { get; set; }

        public event Action<int> StockChanged;

        public override string ToString()
        {
            return Name + " " + Stock;
        }
    }
}