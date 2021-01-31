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

namespace TCP_Chat.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Client client { get; set; }

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

        private string _targetUsername;
        public string targetUsername
        {
            get
            {
                return _targetUsername;
            }
            set
            {
                _targetUsername = value;
                OnPropertyChanged("targetUsername");
            }
        }

        private string _username = "";

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }
        private ObservableCollection<object> _messages = new ObservableCollection<object>();
        public ObservableCollection<object> messages
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
                        param => this.OpenWindow(targetUsername),
                        param => this.CanOpenWindow(targetUsername));
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
        private void Disconnect()
        {
            this.client.TryDisconnect();
        }
        private bool CanConnect()
        {
            if (this.client.isConnected)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void Connect()
        {

            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); - only needed when connecting to your LAN network automatically
            IPAddress ipAddress = null;
            IPAddress.TryParse(serverIp, out ipAddress);
            client.Username = Username;
            if (ipAddress != null && this.port != 0)
            {
                try
                {
                    client.setEndPoint(ipAddress, this.port);

                    Task.Run(() => receiveMessagesAsync());
                }
                catch (SocketException)
                {
                    messages.Add("Connection failed");
                }
            }
            else
            {
                messages.Add("Invalid Server credentials");
            }
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

                if (message != null)
                {
                    if (message is List<string> users)
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
                                    targetUsername = user;
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
                                    PersonalChatWindow personalWindow = new PersonalChatWindow(this.client, Message.sender);
                                    PersonalWindows.Add(Message.sender, personalWindow);
                                    personalWindow.AddMessage(Message.sender + ": " + Message.message);
                                    Application.Current.Properties["personalWindows"] = PersonalWindows;
                                    personalWindow.Show();
                                });
                            }
                        }
                        else
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                messages.Add(Message.sender + ": " + Message.message);
                            });
                        }
                    }
                    else if (message is System.Drawing.Bitmap)
                    {

                        messages.Add(message);

                    }
                }
                else
                {
                    return;
                }

            }
        }
        private bool CanSendFile()
        {
            return true;
        }
        private void SendFile()
        {
            if (this.client.isConnected)
            {
                fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();

                string filePath = fileDialog.FileName;
                ImagePacket imagePacket = new ImagePacket();
                imagePacket.Imagebmp = new System.Drawing.Bitmap(filePath);
                imagePacket.isPersonal = false;
                imagePacket.sender = this.client.Username;
                this.client.TrySendObject(imagePacket);
            }
            else
            {
                messages.Add("not connected");
            }

        }
        private bool CanOpenWindow(string targetUser)
        {
            if (PersonalWindows.ContainsKey(targetUser))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void OpenWindow(string targetUser)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                PersonalChatWindow personalWindow = new PersonalChatWindow(this.client, targetUser);
                PersonalWindows.Add(targetUser, personalWindow);
                personalWindow.Show();
            });
        }
        private bool CanSend()
        {
            return true;
        }
        private void Send()
        {
            if (this.client.isConnected)
            {
                if (currentMessage != string.Empty || currentMessage != "")
                {
                    this.client.sendMessage(currentMessage);
                }
                currentMessage = "";
            }
            else
            {
                currentMessage = "Not connected";
            }
        }

        public void WindowClosing(object sender, CancelEventArgs e)
        {
            this.Disconnect();
            e.Cancel = false;
        }
    }
}
