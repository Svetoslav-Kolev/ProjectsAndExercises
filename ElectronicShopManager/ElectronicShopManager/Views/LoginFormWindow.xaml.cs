using ElectronicShopManager.ViewModels;
using ElectronicShopManager.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicShopManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LogInViewModel loginModel;
        public MainWindow()
        {
            InitializeComponent();
            loginModel = new LogInViewModel();
            this.DataContext = loginModel; 
            
        }
        public async void LogIn_Click(object sender ,RoutedEventArgs e)
        {
            loginModel.Password = PassBox.Password;
            if (loginModel.LogUserCommand.CanExecute(null))
               await loginModel.LogUser(); // command.execute executes async without awaiting the result in this method , thus the result I receive has not actually completed the login procedure unless I use LogUser
            if (!loginModel.NotLoggedIn)
            {
                AccountManagementWindow accountWindow = new AccountManagementWindow();
                accountWindow.Show();
            }

        }

    }
}
