using ElectronicShopManager.Converters;
using ElectronicShopManager.Models;
using ElectronicShopManager.ViewModels;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ElectronicShopManager.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        private OrderDetailsViewModel detailsModel;
        public OrderDetailsWindow(int orderID)
        {
            InitializeComponent();
            detailsModel = new OrderDetailsViewModel();
            detailsModel.OrderID = orderID;
            this.DataContext = detailsModel;
        }
        private void IntegerUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (var ch in e.Text)
            {
                if (!(Char.IsDigit(ch)))
                {
                    e.Handled = true;

                    break;
                }
            }

        }

        private void OrderGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "ProductID") //Remove extra fields from the command properties of Order
            {
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                ProductIDToNameConverter con = new ProductIDToNameConverter();
                dgtc.Header = "Product";
                (dgtc.Binding as Binding).Converter = con;
            }
            else if (e.PropertyName == "OrderHistory" || e.PropertyName == "Products")
            {
                e.Cancel = true;
            }
            else if (e.PropertyName == "Discount")
            {
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;

                dgtc.Binding.StringFormat = "{0} %";
            }
        }

        private void myUpDownControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
