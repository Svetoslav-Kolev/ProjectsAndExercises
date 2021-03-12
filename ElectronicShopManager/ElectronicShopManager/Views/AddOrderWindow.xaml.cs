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

namespace ElectronicShopManager.Views
{
    /// <summary>
    /// Interaction logic for AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        AddOrderViewModel viewModel;
        public AddOrderWindow()
        {
            InitializeComponent();
            viewModel = new AddOrderViewModel();
            viewModel.currUser = (Users)Application.Current.Properties["CurrentUser"];
            this.DataContext = viewModel;
        }
        private void IntegerUpDown_PreviewTextInput(object sender,TextCompositionEventArgs e)
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
        private void AddOrder_Clicked(object sender,RoutedEventArgs e)
        {
          
        }
    }
}
