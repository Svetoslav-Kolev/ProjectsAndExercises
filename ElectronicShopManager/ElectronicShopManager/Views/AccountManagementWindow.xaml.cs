using ElectronicShopManager.Commands;
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
using ElectronicShopManager.Converters;

namespace ElectronicShopManager.Views
{
    /// <summary>
    /// Interaction logic for AccountManagementWindow.xaml
    /// </summary>
    public partial class AccountManagementWindow : Window
    {
        AccountManagementViewModel accountModel;
        public AccountManagementWindow()
        {

            InitializeComponent();
            accountModel = new AccountManagementViewModel();
            accountModel.currentUser = (Users)Application.Current.Properties["CurrentUser"];
            this.DataContext = accountModel;
            
        }
        public void LogOut_Clicked(object sender, RoutedEventArgs e )
        {
            Application.Current.Properties["CurrentUser"] = null;
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();

        }
        public void AddOrder_Clicked(object sender , RoutedEventArgs e)
        {
            AddOrderWindow addWindow = new AddOrderWindow();
            addWindow.ShowDialog();
        }
        public void ModifyOrder_Clicked(object sender, RoutedEventArgs e)
        {
            ModifyOrderView addWindow = new ModifyOrderView(accountModel.SelectedOrder);
            addWindow.ShowDialog();
        }
        public void ViewDetails_Clicked(object sender , RoutedEventArgs e)
        {
            OrderDetailsWindow detailsWindow = new OrderDetailsWindow(accountModel.SelectedOrder.OrderID);
            detailsWindow.Show();
        }
        private void OrderGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
           if(e.PropertyName == "EmployeeID")
            {
              
                EmployeeIDToNameConverter con = new EmployeeIDToNameConverter();
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                dgtc.Header = "Employee Name";
                (dgtc.Binding as Binding).Converter = con;
            }
            else if (e.PropertyName == "Status")
            {
                
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                StatusConverter con = new StatusConverter();
                (dgtc.Binding as Binding).Converter = con;
            }else if (e.PropertyName == "CustomerID")
            {
                DataGridTextColumn dgtc = e.Column as DataGridTextColumn;
                CustomerIDToNameConverter con = new CustomerIDToNameConverter();
                dgtc.Header = "Customer Name";
                (dgtc.Binding as Binding).Converter = con;
            }
            else if (e.PropertyName == "Users" || e.PropertyName=="OrderDetails" || e.PropertyName=="Employees")
            {
                e.Cancel = true;
            }
        }

    }
}
