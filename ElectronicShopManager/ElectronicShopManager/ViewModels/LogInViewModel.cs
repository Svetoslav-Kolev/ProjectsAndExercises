using ElectronicShopManager.Commands;
using ElectronicShopManager.Models;
using ElectronicShopManager.Services;
using ElectronicShopManager.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ElectronicShopManager.ViewModels
{
    class LogInViewModel : BaseViewModel
    {

        private bool notLoggedIn = true;
        public bool NotLoggedIn
        {
            get
            {
                return notLoggedIn;
                
            }
            set
            {
                notLoggedIn = value;
                OnPropertyChanged("NotLoggedIn");
            }
        }
        private string notificaiton = "Welcome , Enter your credentials here:";
        public string Notification
        {
            get
            {
                return notificaiton;
            }
            set
            {
                notificaiton = value;
                OnPropertyChanged("Notification");
            }
        }
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }
        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private ICommand logUserCommmand;
        public ICommand LogUserCommand
        {
            get
            {
              if(logUserCommmand == null)
                {
                    logUserCommmand = new AsyncCommand(
                        async () => await LogUser(),
                        param => CanLogUser());
                }
                return logUserCommmand;
            }
        }
        private bool CanLogUser()
        {
            if (NotLoggedIn)
            {
                return true;
            }
            return true;
        }
        public int LoginResult = 3;
        public async Task LogUser()
        {
            DataAccessService dbService = new DataAccessService();
            LoginResult = await dbService.Login(Username, Password);
            if(LoginResult == 0)
            {
                NotLoggedIn = false;
                //AccountManagementWindow accountWindow = new AccountManagementWindow();
                //accountWindow.Show();
            }
            else if (LoginResult == 1)
            {
                Notification = "Incorrect Password";
                Password = string.Empty;             
            }
            else
            {
                Notification = "Invalid Login";
            }
            Password = string.Empty;
            
        }
    }
}

