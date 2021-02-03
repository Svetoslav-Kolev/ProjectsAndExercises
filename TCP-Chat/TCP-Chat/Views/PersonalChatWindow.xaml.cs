using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        private void PersonalChatWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dictionary<string ,PersonalChatWindow> windowsDictionary =(Dictionary<string,PersonalChatWindow>) Application.Current.Properties["personalWindows"];
            windowsDictionary.Remove(target);
            Application.Current.Properties["personalWindows"] = windowsDictionary;
        }

        public void AddMessage(string message)
        {
            viewModel.messages.Add(new ViewItemModel() { message = target + ":" + message });


            //add autoscrolling when first collection message is added
            AddAutoScrolling();
        }
        public void AddImage(BitmapImage img)
        {
            viewModel.messages.Add(new ViewItemModel() { bmpImage = img , message = target + " sent an Image!" });

            //add autoscrolling when first collection message is added
            AddAutoScrolling();
        }
        public void AddAutoScrolling()
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
