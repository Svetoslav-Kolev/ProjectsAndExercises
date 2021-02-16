using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using TCP_Chat.Commands;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System.Net.Sockets;
using TCPClientServer;
using Microsoft.Win32;
using TCP_Chat.Views;
using System.Windows.Media.Imaging;
using System.Drawing;
using TCP_Chat.ValueConverters;
using System.IO;

namespace TCP_Chat.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Client client { get; set; }
        private bool attemptingConnection;
        private string _serverIp;
        public string serverIp
        {
            get
            {
                return _serverIp;
            }
            set
            {
                _serverIp = value;
                OnPropertyChanged("serverIp");
            }
        }
        private int _port;
        public int port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                OnPropertyChanged("port");
            }
        }
        private string _currentMessage = "";
        public string currentMessage
        {
            get { return _currentMessage; }
            set { _currentMessage = value; OnPropertyChanged("currentMessage"); }
        }
        private string _username = "";

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }
        private ObservableCollection<ViewItemModel> _messages = new ObservableCollection<ViewItemModel>();
        public ObservableCollection<ViewItemModel> messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged("messages"); }
        }

        private ObservableCollection<Button> _currentUsers = new ObservableCollection<Button>();
        public ObservableCollection<Button> CurrentUsers
        {
            get { return _currentUsers; }
            set { _currentUsers = value; OnPropertyChanged("CurrentUsers"); }
        }

        public TextBlock chatTextBlock;

        public OpenFileDialog fileDialog;
        public MainWindowViewModel()
        {
            this.client = new Client();
            this.chatTextBlock = new TextBlock();
            this.PersonalWindows = new Dictionary<string, PersonalChatWindow>();
            Application.Current.Properties["personalWindows"] = this.PersonalWindows; //Chat windows for personal chat with other users
        }

        private ICommand _openPersonalWindow;
        public ICommand OpenPersonalWindow
        {
            get
            {
                if (_openPersonalWindow == null)
                {
                    _openPersonalWindow = new RelayCommand(
                        param => this.OpenWindow(param.ToString()),
                        param => this.CanOpenWindow(param));
                }
                return _openPersonalWindow;
            }
        }

        private ICommand _connectCommand;
        public ICommand connectCommand
        {
            get
            {
                if (_connectCommand == null)
                {
                    _connectCommand = new AsyncCommand(
                        async () => await this.Connect(),
                        param => this.CanConnect());
                }
                return _connectCommand;
            }
        }

        private ICommand _disconnectCommand;
        public ICommand disconnectCommand
        {
            get
            {
                if (_disconnectCommand == null)
                {
                    _disconnectCommand = new AsyncCommand(
                        async () => await this.Disconnect(),
                        param => this.CanDisconnect());
                }
                return _disconnectCommand;
            }
        }

        private ICommand _sendCommand;
        public ICommand sendCommand
        {
            get
            {
                if (_sendCommand == null)
                {
                    _sendCommand = new AsyncCommand(
                        async () => await this.Send(),
                        param => this.CanSend());
                }
                return _sendCommand;
            }
        }

        private ICommand _sendFileCommand;
        public ICommand sendFileCommand
        {
            get
            {
                if (_sendFileCommand == null)
                {
                    _sendFileCommand = new AsyncCommand(
                        async () => await this.SendFile(),
                        param => this.CanSendFile());
                }
                return _sendFileCommand;
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanDisconnect()
        {
            if (this.client.isConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task Disconnect()
        {
            await this.client.TryDisconnect();
        }
        private bool CanConnect()
        {
            if (this.client.isConnected && SocketConnected(this.client.socket) || this.attemptingConnection == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private async Task Connect()
        {

            IPAddress ipAddress = null;
            IPAddress.TryParse(serverIp, out ipAddress);
            client.Username = Username;

            bool connected = false;
            attemptingConnection = true;

            if (ipAddress != null && this.port != 0)
            {
                messages.Add(new ViewItemModel() { message = "Attempting to connect..." });

                connected = await Task.Run(() => client.setEndPoint(ipAddress, this.port));
                attemptingConnection = false;
                if (connected == true)
                {
                    await receiveMessagesAsync();
                }
                else
                {
                    messages.Add(new ViewItemModel() { message = "Connection failed" });
                }
            }
            else
            {
                attemptingConnection = false;
                messages.Add(new ViewItemModel() { message = "Invalid Server Credentials" });
            }
        }
        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
        private Dictionary<string, PersonalChatWindow> _personalWindows;
        public Dictionary<string, PersonalChatWindow> PersonalWindows
        {
            get
            {
                return _personalWindows;
            }
            set
            {
                _personalWindows = value;
            }
        }
        private async Task receiveMessagesAsync()
        {
            while (client.isConnected)
            {
                try
                {
                    PersonalWindows = (Dictionary<string, PersonalChatWindow>)Application.Current.Properties["personalWindows"];
                    Package message = await client.receiveMessageAsync();
                    await UpdateUI(message);
                }
                catch
                {
                    await HandleDisconnection();
                }

            }
        }
        private async Task UpdateUI(Package message)
        {
            if (message != null)
            {
                if (message is DisconnectionPackage dcPackage)
                {
                    messages.Add(new ViewItemModel { message = dcPackage.reason });
                    UpdatePersonalWindows();
                    CurrentUsers.Clear();
                    await client.TryDisconnect(); //sets request disconnection to true
                    client.requestDisconnection = false;
                }
                else if (message is UsersPacket users)
                {
                    CurrentUsers.Clear();
                    foreach (string user in users.Usernames)
                    {
                        UpdateConnectedUsers(user);
                    }
                }
                else if (message is MessagePacket Message)
                {
                    if (Message.targetUsername == Username && Message.isPersonal == true)
                    {
                        if (PersonalWindows.ContainsKey(Message.sender))
                        {
                            PersonalWindows[Message.sender].AddMessage(Message);
                        }
                        else
                        { 
                            CreatePersonalWindow(Message.sender, Message);
                        }
                    }
                    else
                    {
                        messages.Add(new ViewItemModel() { message = Message.sender + ": " + Message.message });
                    }
                }
                else if (message is ImagePacket imgPacket)
                {
                    if (imgPacket.isPersonal == true && imgPacket.targetUsername == Username)
                    {
                        if (PersonalWindows.ContainsKey(imgPacket.sender))
                        {

                            PersonalWindows[imgPacket.sender].AddImage(imgPacket);
                        }
                        else
                        {
                            CreatePersonalWindow(imgPacket.sender, imgPacket);
                        }
                    }
                    else
                    {

                        BitmapToImageConverter converter = new BitmapToImageConverter();
                        var receivedImage = converter.Convert(imgPacket.Imagebmp);

                        messages.Add(new ViewItemModel() { bmpImage = (BitmapImage)receivedImage, message = imgPacket.sender + " sent an Image!" });

                    }
                }
            }
            else
            {
                return;
            }
        }
        private void UpdateConnectedUsers(string user)
        {
            if (user != Username)
            {
                Button userButton = new Button();
                userButton.Content = user;
                userButton.CommandParameter = user;
                userButton.Command = OpenPersonalWindow;

                if (PersonalWindows.ContainsKey(user))
                {
                    userButton.IsEnabled = false; //Chat with target is already open , so button is disabled
                }
                CurrentUsers.Add(userButton);
            }
        }
        private bool CanSendFile()
        {
            if (this.client.isConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task SendFile()
        {
            if (this.client.isConnected)
            {
                try
                {
                    fileDialog = new OpenFileDialog();
                    fileDialog.ShowDialog();
                    fileDialog.DefaultExt = ".png";
                    fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

                    string filePath = fileDialog.FileName;

                    if (filePath != null && filePath != string.Empty)
                    {
                        ImagePacket imagePacket = new ImagePacket();
                        imagePacket.Imagebmp = new Bitmap(filePath);
                        imagePacket.isPersonal = false;
                        imagePacket.sender = this.client.Username;
                        try
                        {
                            await this.client.TrySendObject(imagePacket);
                        }
                        catch
                        {
                            await HandleDisconnection(imagePacket);
                        }

                    }
                }
                catch (ArgumentException)
                {

                    messages.Add(new ViewItemModel() { message = "File is not in a valid format, please send only  jpeg, png , gif or jpg files!" });
                }

            }
            else
            {
                messages.Add(new ViewItemModel() { message = "You are not connected" });
            }

        }
        private void UpdatePersonalWindows() // inform personal chat windows that you have been disconnected
        {
            foreach (var window in PersonalWindows.Values)
            {
                MessagePacket message = new MessagePacket("You have been disconnected!");
                message.sender = "Server";
                window.AddMessage(message);
            }
        }
        private async Task HandleDisconnection(Package package = null)
        {
            if (this.client.requestDisconnection)
            {
                //Requested Disconnection
                messages.Add(new ViewItemModel() { message = "You have been disconnected." });
                this.client.requestDisconnection = false;
                UpdatePersonalWindows();
            }
            else
            {
                //Forceful or unexpeted disconnection , try to reconnect , if you were sending an object resend it
                
                if (attemptingConnection == false) //In case of 2 simultanious errors , preventing 2 connection attempts
                {
                    messages.Add(new ViewItemModel() { message = "Connection lost. Trying to reconnect..." });
                    await Connect();
                }

                if (SocketConnected(this.client.socket))
                {
                    if (package != null)
                    {
                        try
                        {
                            await this.client.TrySendObject(package);
                        }
                        catch
                        {
                            if (this.client.socket.Connected)
                            {
                                this.client.Disconnect();
                            }
                            UpdatePersonalWindows();
                        }
                    }

                }
                else
                {
                    UpdatePersonalWindows();
                }
            }

        }
        private bool CanOpenWindow(object targetUser)
        {
            if (PersonalWindows.ContainsKey(targetUser.ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void OpenWindow(string targetUser) //Open a new personal chat
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                PersonalChatWindow personalWindow = new PersonalChatWindow(this.client, targetUser);
                PersonalWindows.Add(targetUser.ToString(), personalWindow);
                personalWindow.Show();
            });
        }
        private bool CanSend()
        {
            if (this.client.isConnected)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private async Task Send()
        {
            if (this.client.isConnected)
            {
                if (currentMessage != string.Empty && currentMessage != "" && currentMessage.Length <= 150)
                {
                    try
                    {
                        await this.client.sendMessage(currentMessage);
                    }
                    catch
                    {
                        MessagePacket message = new MessagePacket(currentMessage);
                        message.sender = this.client.Username;
                        await HandleDisconnection(message);
                    }
                    currentMessage = "";
                }
                else if (currentMessage.Length > 150)
                {
                    messages.Add(new ViewItemModel() { message = "Your message is too long! Max Length is 150 characters." });
                }

            }
            else
            {
                currentMessage = "Not connected";
            }
        }
        private void CreatePersonalWindow(string target, Package Packet) // Create personal window, when a personal message is received and a chat with the sender is not open
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                PersonalChatWindow personalWindow = new PersonalChatWindow(this.client, target);
                PersonalWindows.Add(target, personalWindow);
                if (Packet is ImagePacket imgPacket)
                {

                    personalWindow.AddImage(imgPacket);
                    Application.Current.Properties["personalWindows"] = PersonalWindows;
                    personalWindow.Show();
                }
                else if (Packet is MessagePacket Message)
                {
                    personalWindow.AddMessage(Message);
                    Application.Current.Properties["personalWindows"] = PersonalWindows;
                    personalWindow.Show();
                }

            });
        }

        public async void WindowClosing(object sender, CancelEventArgs e)
        {
            await this.Disconnect();
            e.Cancel = false;
        }
    }
}
