using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SM_Machine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SuperMarket _supermarket;
        public MainWindow()
        {
            InitializeComponent();

            _supermarket = new SuperMarket();
            
            lv_items.ItemsSource = _supermarket.SMStock;
            _supermarket.SMStock.ItemChanged += () => { lv_items.ItemsSource = _supermarket.SMStock.ToArray();
                Console.Out.Flush();
            };
        }

        private void lv_items_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _supermarket.SMStock.Add(new StockItem());
        }
    }
}
