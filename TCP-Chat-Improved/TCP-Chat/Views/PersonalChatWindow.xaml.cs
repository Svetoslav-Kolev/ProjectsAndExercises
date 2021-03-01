using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCP_Chat.ValueConverters;
using TCP_Chat.ViewModels;
using TCPClientServer;

namespace TCP_Chat.Views
{
    /// <summary>
    /// Interaction logic for PersonalChatWindow.xaml
    /// </summary>
    public partial class PersonalChatWindow : Window
    {
        public PersonalChatViewModel viewModel = new PersonalChatViewModel();
        public string target;
        public PersonalChatWindow(Client client,string targetName)
        {
            InitializeComponent();

            this.DataContext = viewModel;
            viewModel.client = client;
            viewModel.targetUsername = targetName;
            target = targetName;
            Closing += PersonalChatWindow_Closing;
            

        }
        private void SendFile_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            fileDialog.DefaultExt = ".png";
            fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            string filePath = fileDialog.FileName;
            viewModel.filePath = filePath;
            if (viewModel.sendFileCommand.CanExecute(null))
                viewModel.sendFileCommand.Execute(filePath);

        }
        private void PersonalChatWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dictionary<string ,PersonalChatWindow> windowsDictionary =(Dictionary<string,PersonalChatWindow>) Application.Current.Properties["personalWindows"];
            windowsDictionary.Remove(target);
            Application.Current.Properties["personalWindows"] = windowsDictionary;
        }

        public void AddMessage(MessagePacket messagePacket)
        {
            viewModel.AddMessage(new ViewItemModel() { message = messagePacket.sender + ":" + messagePacket.message });


            //add autoscrolling when first collection message is added
            if (viewModel.messages.Count == 1)
            {
                AddAutoScrolling();
            }
        }
        public void AddImage(ImagePacket imgPacket)
        {
            BitmapToImageConverter converter = new BitmapToImageConverter();
            var receivedImage = converter.Convert(imgPacket.Imagebmp);

            viewModel.AddMessage(new ViewItemModel() { bmpImage = (BitmapImage)receivedImage , message = imgPacket.sender + " sent an Image!" });

            //add autoscrolling when first collection message is added
            if (viewModel.messages.Count == 1)
            {
                AddAutoScrolling();
            }
           
        }
        private void AddAutoScrolling()
        {
            viewModel.messages.CollectionChanged += (sender, e) =>
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
