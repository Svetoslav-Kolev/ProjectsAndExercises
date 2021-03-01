using ElectronicShopManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManager.ViewModels
{
    abstract class OrderBaseViewModel:BaseViewModel
    {
        public  OrderBaseViewModel() //Base viewmodel for the data of the order
        {
            Task.Run(() => GetEmployees());
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
        private Dictionary<int, string> employees;
        public Dictionary<int, string> Employees
        {
            get
            {
                return employees;
            }
            set
            {
                employees = value;
                OnPropertyChanged("Employees");
            }
        }
        private int selectedEmployeeID;
        public int SelectedEmployeeID
        {
            get
            {
                return selectedEmployeeID;
            }
            set
            {
                selectedEmployeeID = value;
                OnPropertyChanged("SelectedEmployeeID");
            }
        }

        private string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        private List<byte> statuses;
        public List<byte> Statuses
        {
            get
            {
                statuses = new List<byte> { 0, 1, 2, 3 };
                return statuses;
            }
            set
            {
                statuses = value;
                OnPropertyChanged("Statuses");
            }
        }
        private byte status;
        public byte Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private async Task GetEmployees()
        {
            DataAccessService dataService = new DataAccessService();
            Employees = await dataService.GetEmployeesAsync();
        }
    }
}
