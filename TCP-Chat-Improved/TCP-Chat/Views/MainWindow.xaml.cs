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
        MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            vm = (MainWindowViewModel)this.DataContext;
            Closing += vm.WindowClosing;



            //Auto scrolling 
            vm.Messages.CollectionChanged += (sender, e) =>
             {
                 if (e.NewItems != null)
                 {
                     Decorator border = VisualTreeHelper.GetChild(ChatTextBlock, 0) as Decorator;
                     ScrollViewer scroll = border.Child as ScrollViewer;
                     scroll.ScrollToBottom();
                 }
             };
        }
        private void SendFile_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            fileDialog.DefaultExt = ".png";
            fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            string filePath = fileDialog.FileName;
            vm.filePath = filePath;
            if (vm.sendFileCommand.CanExecute(null))
                vm.sendFileCommand.Execute(filePath);
           
           
        }
    }
}

