﻿using ElectronicShopManager.Commands;
using ElectronicShopManager.Models;
using ElectronicShopManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectronicShopManager.ViewModels
{
    class AddOrderViewModel : OrderBaseViewModel
    {
        public AddOrderViewModel()
        {
            Task.Run(() => GetProducts());
        }
        public Users currUser;

        private Dictionary<int, string> products;
    
        public Dictionary<int, string> Products
        {
            get
            {     
                return products;
            }
            set
            {
                products = value;
                OnPropertyChanged("Products");
            }
        }
        private int discount;
        public int Discount
        {
            get
            {
                return discount;
            }
            set
            {
                discount = value;
                RaisePropertyChanged("Discount");
            }
        }
        private int selectedProductID;
        public int SelectedProductID
        {
            get
            {
                return selectedProductID;
            }
            set
            {
                selectedProductID = value;
                RaisePropertyChanged("SelectedProductID");
            }
        }
        private int selectedQuantity;
        public int SelectedQuantity
        {
            get
            {
                return selectedQuantity;
            }
            set
            {
                selectedQuantity = value;
                OnPropertyChanged("SelectedQuantity");
            }
        }
        private List<int> quantity;
        public List<int> Quantity
        {
            get
            {             
                return quantity;
            }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if(propertyName == "SelectedProductID")
            {
                Quantity = GetStockQuantity();
            }
            OnPropertyChanged(propertyName);
        }
        private List<int> GetStockQuantity()
        {
            DataAccessService dataService = new DataAccessService();
            List<int> quantitiesToPurchase = new List<int>(); //Visualised as a dropdown list to minimize user input
            for (int i = 1; i <= dataService.GetProductStock(selectedProductID); i++)
            {
                quantitiesToPurchase.Add(i);
            }
            return quantitiesToPurchase;
        }
        private async Task GetProducts()
        {
            DataAccessService dataService = new DataAccessService();
            Products = await dataService.GetProductsAsync();
        }
        private ICommand addOrderCommand;
        public ICommand AddOrderCommand
        {
            get
            {
                if (addOrderCommand == null)
                {
                    addOrderCommand = new AsyncCommand(
                        async() => await AddOrder(),
                        param => CanAddOrder());
                }
                return addOrderCommand;

            }
        }
        private bool CanAddOrder()
        {
            if (SelectedEmployeeID != 0 
                && SelectedProductID != 0
                && SelectedQuantity > 0
                && Address != string.Empty)
            {
                return true;
            }
            return false;
        }
        private async Task AddOrder() //Review Code
        {
            try
            {
                OrderHistory order = new OrderHistory();
                order.DeliveryAddress = Address;
                order.EmployeeID = SelectedEmployeeID;
                order.Status = (byte)Status;

                OrderDetails details = new OrderDetails();

                details.Quantity = SelectedQuantity;
                details.ProductID = SelectedProductID;
                details.Discount = Discount;

                DataUpdateService updateService = new DataUpdateService();
                await updateService.AddOrderAsync(currUser, order, details);
                 
                Notification = "Order added successfuly, refresh orders to see any changes";
                ClearSelections();
            }
            catch 
            {
                Notification = "Failed to add order, try again after a while";
                throw;
            }
           
        }
        private void ClearSelections()
        {
            SelectedEmployeeID = -1;
            SelectedProductID = -1;
            SelectedQuantity = -1;
            Discount = 0;
            Address = string.Empty;
        }
    }
}