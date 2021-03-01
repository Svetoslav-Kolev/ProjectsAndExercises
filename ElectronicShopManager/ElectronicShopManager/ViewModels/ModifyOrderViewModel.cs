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
    class ModifyOrderViewModel : OrderBaseViewModel, INotifyDataErrorInfo
    {
        public int SelectedOrderID;
        private string address;
        public new string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                ValidateAddress();
                OnPropertyChanged("Address");
            }
        }
        private string dateGiven;
        public string DateGiven
        {
            get
            {
                return dateGiven;
            }
            set
            {
                dateGiven = value;
                ValidateDateTime();
                OnPropertyChanged("DateGiven");
            }
        }

        private string receiptNumber;
        public string ReceiptNumber
        {
            get
            {
                return receiptNumber;
            }
            set
            {
                receiptNumber = value;
                ValidateReceipt();
                OnPropertyChanged("ReceiptNumber");
            }
        }
        private ICommand modifyOrderCommand;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public ICommand ModifyOrderCommand
        {
            get
            {
                if (modifyOrderCommand == null)
                {
                    modifyOrderCommand = new AsyncCommand(
                        async () => await ModifyOrder(),
                        param => CanModifyOrder());
                }
                return modifyOrderCommand;
            }
        }

        public bool HasErrors => _errorsByPropertyName.Any();
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        private void ValidateReceipt()
        {
            ClearErrors(nameof(ReceiptNumber));
            if (string.IsNullOrWhiteSpace(ReceiptNumber))
                AddError(nameof(ReceiptNumber), "Receipt cannot be empty.");
            if (ReceiptNumber == null || ReceiptNumber?.Length != 13)
                AddError(nameof(ReceiptNumber), "Receipt Number must be 13 characters long.");
        }
        private void ValidateAddress()
        {
            ClearErrors(nameof(Address));
            if (string.IsNullOrWhiteSpace(Address))
                AddError(nameof(Address), "Address cannot be empty.");

        }
        private void ValidateDateTime()
        {
            ClearErrors(nameof(DateGiven));
            if (string.IsNullOrWhiteSpace(DateGiven.ToString()))
                AddError(nameof(DateGiven), "Date cannot be empty.");
            DateTime minDate = new DateTime(1970, 1, 1); //Example min date
            DateTime maxDate = DateTime.Now;
            if (!DateTime.TryParse(DateGiven, out DateTime result))
            {
                AddError(nameof(DateGiven), "Invalid Date");

            }
            else
            {
                if (result < minDate || result > maxDate)
                {
                    AddError(nameof(DateGiven), "Invalid Date");
                }
            }




        }
        private bool CanModifyOrder()
        {
            if (HasErrors ||SelectedEmployeeID == 0)
            {
                return false;
            }
            return true;
        }
        private async Task ModifyOrder()
        {
            try
            {
                DataUpdateService updateService = new DataUpdateService();
                OrderHistory modifiedOrder = new OrderHistory();
                modifiedOrder.DeliveryAddress = Address;
                modifiedOrder.EmployeeID = SelectedEmployeeID;
                modifiedOrder.ReceiptNumber = ReceiptNumber;
                modifiedOrder.OrderID = SelectedOrderID;
                modifiedOrder.Status = Status;
                modifiedOrder.OrderDate = Convert.ToDateTime(DateGiven);
                await updateService.ModifyOrderAsync(modifiedOrder);
                Notification = "Order has been modified , please refresh your orders to see any changes made.";

            }
            catch
            {

                Notification = "Review Data and try again";
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
             _errorsByPropertyName[propertyName] : null;
        }
    }
}
