using ElectronicShopManager.Commands;
using ElectronicShopManager.Models;
using ElectronicShopManager.Services;
using ElectronicShopManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicShopManager.ViewModels
{
    class AccountManagementViewModel : BaseViewModel
    {
        public AccountManagementViewModel()
        {
            Task.Run(() => GetOrders());
        }
        public Users currentUser;
        private OrderHistory selectedOrder;

        private bool canViewDetails;
        public bool CanViewDetails
        {
            get
            {
                canViewDetails = CanDeleteOrder();
                return canViewDetails;
            }
            set
            {
                canViewDetails = value;
                OnPropertyChanged("CanViewDetails");
            }
        }
        public OrderHistory SelectedOrder
        {
            get
            {
                return selectedOrder;
            }
            set
            {
                selectedOrder = value;
                CanViewDetails = true;
                OnPropertyChanged("SelectedOrder");
            }
        }

        private ObservableCollection<OrderHistory> orderData;
        public ObservableCollection<OrderHistory> OrderData
        {
            get
            {
                return orderData;
            }
            set
            {
                orderData = value;
                OnPropertyChanged("OrderData");
            }
        }
        private ICommand getOrdersCommand;
        public ICommand GetOrdersCommand
        {
            get
            {
                if (getOrdersCommand == null)
                {
                    getOrdersCommand = new AsyncCommand(
                        async() => await GetOrders(),
                        param => CanGetOrders());
                }
                return getOrdersCommand;

            }
        }
        private bool CanGetOrders()
        {
            return true;
        }
        private async Task GetOrders()
        {
            DataAccessService dataService = new DataAccessService();
            List<OrderHistory> orders = await (dataService.GetUserOrdersAsync(currentUser));
            OrderData = new ObservableCollection<OrderHistory>(orders);
        }
        private ICommand deleteOrderCommand;
        public ICommand DeleteOrderCommand
        {
            get
            {
                if (deleteOrderCommand == null)
                {
                    deleteOrderCommand = new AsyncCommand(
                        async () => await DeleteOrder(),
                        param => CanDeleteOrder());
                }
                return deleteOrderCommand;

            }
        }
        
        private bool CanDeleteOrder()
        {
            if (SelectedOrder != null)
            {
                return true;
            }
            return false;
        }
        private async Task DeleteOrder()
        {
            DataUpdateService updateService = new DataUpdateService();
            await updateService.DeleteOrderAsync(SelectedOrder.OrderID);
            OrderHistory orderToRemove = OrderData.Where(o => o.OrderID == SelectedOrder.OrderID).FirstOrDefault();
            OrderData.Remove(orderToRemove);
        }

    }
}
