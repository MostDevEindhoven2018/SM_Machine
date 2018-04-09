using System;
using System.Collections;
using System.Collections.Generic;

namespace SM_Machine
{
    public class Stock : IList<StockItem>
    {
        List<StockItem> _stock = new List<StockItem>();

        public StockItem this[int index]
        {
            get => ((IList<StockItem>)_stock)[index];
            set
            {
                StockItem old = ((IList<StockItem>)_stock)[index];
                if (old != null)
                {
                    old.StockChanged -= ItemChangedListener;
                }
                ((IList<StockItem>)_stock)[index] = value;
                ((IList<StockItem>)_stock)[index].StockChanged += ItemChangedListener;
            }
        }

        public int Count => ((IList<StockItem>)_stock).Count;

        public bool IsReadOnly => ((IList<StockItem>)_stock).IsReadOnly;

        public event Action ItemChanged;

        private void ItemChangedListener(int value)
        {
            ItemChanged?.Invoke();
        }

        public void Add(StockItem item)
        {
            item.StockChanged += ItemChangedListener;
            ((IList<StockItem>)_stock).Add(item);
        }

        public void Clear()
        {
            ((IList<StockItem>)_stock).Clear();
        }

        public bool Contains(StockItem item)
        {
            return ((IList<StockItem>)_stock).Contains(item);
        }

        public void CopyTo(StockItem[] array, int arrayIndex)
        {
            ((IList<StockItem>)_stock).CopyTo(array, arrayIndex);
        }

        public IEnumerator<StockItem> GetEnumerator()
        {
            return ((IList<StockItem>)_stock).GetEnumerator();
        }

        public int IndexOf(StockItem item)
        {
            return ((IList<StockItem>)_stock).IndexOf(item);
        }

        public void Insert(int index, StockItem item)
        {
            item.StockChanged += ItemChangedListener;
            ((IList<StockItem>)_stock).Insert(index, item);
        }

        public bool Remove(StockItem item)
        {
            item.StockChanged -= ItemChangedListener;
            return ((IList<StockItem>)_stock).Remove(item);
        }

        public void RemoveAt(int index)
        {
            _stock[index].StockChanged -= ItemChangedListener;
            ((IList<StockItem>)_stock).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<StockItem>)_stock).GetEnumerator();
        }
    }
}