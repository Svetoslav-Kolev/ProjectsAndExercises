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
using System.Net.Sockets;
using System.Net;
using System.Windows.Threading;
using TCP_Chat.ViewModels;
using Microsoft.Win32;

namespace TCP_Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            MainWindowViewModel vm = (MainWindowViewModel)this.DataContext;
            Closing += vm.WindowClosing;



            //Auto scrolling 
            vm.messages.CollectionChanged += (sender, e) =>
             {
                 if (e.NewItems != null)
                 {
                     Decorator border = VisualTreeHelper.GetChild(ChatTextBlock, 0) as Decorator;
                     ScrollViewer scroll = border.Child as ScrollViewer;
                     scroll.ScrollToBottom();
                 }
             };
        }
    }
}

