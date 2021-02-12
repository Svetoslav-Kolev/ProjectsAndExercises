using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

