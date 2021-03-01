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
    /// Interaction logic for ModifyOrderView.xaml
    /// </summary>
    public partial class ModifyOrderView : Window
    {
        ModifyOrderViewModel modifyModel;
        public ModifyOrderView(OrderHistory SelectedOrder)
        {
            InitializeComponent();
            modifyModel = new ModifyOrderViewModel();
            modifyModel.SelectedOrderID = SelectedOrder.OrderID;
            modifyModel.Status = SelectedOrder.Status;
            modifyModel.SelectedEmployeeID = SelectedOrder.EmployeeID;
            modifyModel.Address = SelectedOrder.DeliveryAddress;
            modifyModel.ReceiptNumber = SelectedOrder.ReceiptNumber;
            modifyModel.DateGiven = SelectedOrder.OrderDate.ToString();
            this.DataContext = modifyModel;

        }
    }
}
