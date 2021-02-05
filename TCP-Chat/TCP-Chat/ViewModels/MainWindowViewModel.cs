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
using System.IO;
using TCP_Chat.Views;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using TCP_Chat.ValueConverters;

namespace TCP_Chat.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Client client { get; set; }
        public bool attemptingConnection;
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
            Application.Current.Properties["personalWindows"] = this.PersonalWindows;
        }

        private ICommand _openPersonalWindow;
        public ICommand OpenPersonalWindow
        {
            get
            {
                if (_openPersonalWindow == null)
                {
                    _openPersonalWindow = new RelayCommand(
                        param => this.OpenWindow(param),
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
                    _connectCommand = new RelayCommand(
                        param => this.Connect(),
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
                    _disconnectCommand = new RelayCommand(
                        param => this.Disconnect(),
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
                    _sendCommand = new RelayCommand(
                        param => this.Send(),
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
                    _sendFileCommand = new RelayCommand(
                        param => this.SendFile(),
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

            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); - only needed when connecting to your LAN network automatically
            //Failed connections take a while to display the "Connection failed message"
            IPAddress ipAddress = null;
            IPAddress.TryParse(serverIp, out ipAddress);
            client.Username = Username;
            bool connect = false;
            attemptingConnection = true;
            if (ipAddress != null && this.port != 0)
            {
                try
                {
                    messages.Add(new ViewItemModel() { message = "Attempting to connect..." });
                    connect = await Task.Run(() => client.setEndPoint(ipAddress, this.port));
                    if (connect == false)
                    {
                        messages.Add(new ViewItemModel() { message = "Connection failed" });
                    }
                    attemptingConnection = false;
                    Task.Run(() => receiveMessagesAsync());
                }
                catch (SocketException)
                {
                    attemptingConnection = false;
                    messages.Add(new ViewItemModel() { message = "Connection failed" });
                }
            }
            else
            {
                attemptingConnection = false;
                messages.Add(new ViewItemModel() { message = "Connection failed" });
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
                PersonalWindows = (Dictionary<string, PersonalChatWindow>)Application.Current.Properties["personalWindows"];
                object message = await client.receiveMessageAsync();
                UpdateUI(message);
            }
        }
        private void UpdateUI(object message)
        {
            if (message != null)
            {
                if (message is string disconnectionReason)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        messages.Add(new ViewItemModel { message = disconnectionReason });
                    });
                }
                else if (message is List<string> users)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        CurrentUsers.Clear();
                        foreach (string user in users)
                        {
                            if (user != Username)
                            {
                                Button userButton = new Button();
                                userButton.Content = user;
                                userButton.CommandParameter = user;
                                userButton.Command = OpenPersonalWindow;

                                if (PersonalWindows.ContainsKey(user))
                                {
                                    userButton.IsEnabled = false;
                                }
                                CurrentUsers.Add(userButton);
                            }
                        }
                    });
                }
                else if (message is MessagePacket Message)
                {
                    if (Message.targetUsername == Username && Message.isPersonal == true)
                    {
                        if (PersonalWindows.ContainsKey(Message.sender))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                PersonalWindows[Message.sender].AddMessage(Message.message);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                CreatePersonalWindow(Message.sender, Message);
                            });
                        }
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            messages.Add(new ViewItemModel() { message = Message.sender + ": " + Message.message });
                        });
                    }
                }
                else if (message is ImagePacket imgPacket)
                {
                    if (imgPacket.isPersonal == true && imgPacket.targetUsername == Username)
                    {
                        if (PersonalWindows.ContainsKey(imgPacket.sender))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                BitmapToImageConverter converter = new BitmapToImageConverter();
                                var receivedImage = converter.Convert(imgPacket.Imagebmp);

                                PersonalWindows[imgPacket.sender].AddImage((BitmapImage)receivedImage);
                            });
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                CreatePersonalWindow(imgPacket.sender, imgPacket);
                            });
                        }
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            BitmapToImageConverter converter = new BitmapToImageConverter();
                            var receivedImage = converter.Convert(imgPacket.Imagebmp);

                            messages.Add(new ViewItemModel() { bmpImage = (BitmapImage)receivedImage, message = imgPacket.sender + " sent an Image!" });
                        });
                    }
                }
            }
            else
            {
                return;
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
                fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                fileDialog.DefaultExt = ".png";
                fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

                string filePath = fileDialog.FileName;
                ImagePacket imagePacket = new ImagePacket();
                imagePacket.Imagebmp = new Bitmap(filePath);
                imagePacket.isPersonal = false;
                imagePacket.sender = this.client.Username;

                await this.client.TrySendObject(imagePacket);
            }
            else
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    messages.Add(new ViewItemModel() { message = "client you are trying to reach is not connected" });
                });
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
        private void OpenWindow(object targetUser)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                PersonalChatWindow personalWindow = new PersonalChatWindow(this.client, targetUser.ToString());
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
                    await this.client.sendMessage(currentMessage);
                    currentMessage = "";
                }
                else if (currentMessage.Length > 200)
                {
                    messages.Add(new ViewItemModel() { message = "Your message is too long! Max Length is 150 characters." });
                }

            }
            else
            {
                currentMessage = "Not connected";
            }
        }
        public void CreatePersonalWindow(string target, object Packet)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                PersonalChatWindow personalWindow = new PersonalChatWindow(this.client, target);
                PersonalWindows.Add(target, personalWindow);
                if (Packet is ImagePacket imgPacket)
                {
                    BitmapToImageConverter converter = new BitmapToImageConverter();
                    var receivedImage = converter.Convert(imgPacket.Imagebmp);

                    personalWindow.AddImage((BitmapImage)receivedImage);
                    Application.Current.Properties["personalWindows"] = PersonalWindows;
                    personalWindow.Show();
                }
                else if (Packet is MessagePacket Message)
                {
                    personalWindow.AddMessage(Message.message);
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
