using ElectronicShopManager.Commands;
using ElectronicShopManager.Models;
using ElectronicShopManager.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectronicShopManager.ViewModels
{
    class OrderDetailsViewModel:BaseViewModel
    {
        public OrderDetailsViewModel()
        {
            Task.Run(() => GetProducts());
        }
        private async Task GetProducts()
        {
            DataAccessService dataService = new DataAccessService();
            Products = await dataService.GetProductsAsync();
        }
        private async Task UpdateDetails()
        {
            DataAccessService dataService = new DataAccessService();
            Details =await dataService.ViewDetailsAsync(OrderID);
        }
        private string notification;
        public string Notification
        {
            get
            {
                return notification;
            }
            set
            {
                notification = value;
                OnPropertyChanged("Notification");
            }
        }
        private OrderDetails selectedDetail;
        public OrderDetails SelectedDetail
        {
            get
            {
                return selectedDetail;
            }
            set
            {
                selectedDetail = value;
                OnPropertyChanged("SelectedDetail");
            }
        }

        private int orderID;
        public int OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                orderID = value;
                Task.Run(()=>RaisePropertyChanged("OrderID"));
            }
        }
        private List<OrderDetails> details;
        public List<OrderDetails> Details
        {
            get
            {
                return details;
            }
            set
            {
                details = value;
                OnPropertyChanged("Details");
            }
        }
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
                Task.Run(()=>RaisePropertyChanged("SelectedProductID"));
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
                OnPropertyChanged("Discount");
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
        private async Task RaisePropertyChanged(string propertyName)
        {
            if (propertyName == "SelectedProductID")
            {
                Quantity = GetStockQuantity();
            }else if(propertyName =="OrderID")
            {
                await UpdateDetails();
            }
            OnPropertyChanged(propertyName);
        }
        private List<int> GetStockQuantity()
        {
            DataAccessService dataService = new DataAccessService();
            List<int> quantitiesToPurchase = new List<int>();
            for (int i = 1; i <= dataService.GetProductStock(selectedProductID); i++)
            {
                quantitiesToPurchase.Add(i);
            }
            return quantitiesToPurchase;
        }
        private ICommand addDetailsCommand;
        public ICommand AddDetailsCommand
        {
            get
            {
                if (addDetailsCommand == null)
                {
                    addDetailsCommand = new AsyncCommand(
                        async () => await AddDetails(),
                        param => CanAddDetails());
                }
                return addDetailsCommand;

            }
        }
        private bool CanAddDetails()
        {
            if (SelectedProductID != 0
                && SelectedQuantity != 0)
            {
                return true;
            }
            return false;
        }
        private async Task AddDetails() 
        {
            try
            {
                OrderDetails details = new OrderDetails();
                details.OrderID = OrderID;
                details.Quantity = SelectedQuantity;
                details.ProductID = SelectedProductID;
                details.Discount = Discount;

                DataUpdateService updateService = new DataUpdateService();
                await updateService.AddOrderDetailAsync(details);
                await UpdateDetails();

                SelectedProductID = -1;
                SelectedQuantity = -1;
                Discount = 0;
                Notification = "Details added successfuly";
            }
            catch
            {
                Notification = "Failed to add order, try again after a while";
            }

        }
        private ICommand removeDetailCommand;

        public ICommand RemoveDetailCommand
        {
            get
            {
                if (removeDetailCommand == null)
                {
                    removeDetailCommand = new AsyncCommand(
                        async () => await RemoveDetail(),
                        param => CanRemoveDetail());
                }
                return removeDetailCommand;

            }
        }


        private bool CanRemoveDetail()
        {
            if (selectedDetail != null)
            {
                return true;
            }
            return false;
        }
        private async Task RemoveDetail() //Review Code
        {
            try
            {
                DataUpdateService updateService = new DataUpdateService();
                await updateService.DeleteOrderDetailAsync(selectedDetail.OrderDetailID);
                await UpdateDetails();
                selectedDetail = null;
                Notification = "Details removed successfuly";
            }
            catch
            {

                Notification = "Failed to remove details";
            }
         

        }
             
    }
}
