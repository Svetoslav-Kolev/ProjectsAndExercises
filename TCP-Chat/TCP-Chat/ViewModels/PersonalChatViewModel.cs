using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TCP_Chat.Commands;
using TCP_Chat.ValueConverters;
using TCPClientServer;

namespace TCP_Chat.ViewModels
{
    public class PersonalChatViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private string _targetUsername;
        public string targetUsername
        {
            get { return _targetUsername; }
            set { _targetUsername = value; OnPropertyChanged("currentMessage"); }
        }
        public Client client { get; set; }
        private ObservableCollection<ViewItemModel> _messages = new ObservableCollection<ViewItemModel>();
        public ObservableCollection<ViewItemModel> messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged("messages"); }
        }

        private string _currentMessage = "";
        public string currentMessage
        {
            get { return _currentMessage; }
            set { _currentMessage = value; OnPropertyChanged("currentMessage"); }
        }
        public PersonalChatViewModel()
        {
            messages = new ObservableCollection<ViewItemModel>();

        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        private OpenFileDialog fileDialog;

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

        private bool CanSendFile()
        {
            return true;
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
                imagePacket.Imagebmp = new System.Drawing.Bitmap(filePath);
                imagePacket.isPersonal = true;
                imagePacket.sender = this.client.Username;
                imagePacket.targetUsername = targetUsername;

                this.client.TrySendObject(imagePacket);

                BitmapToImageConverter bmpConverter = new BitmapToImageConverter();
                var image = bmpConverter.Convert(imagePacket.Imagebmp);

                messages.Add(new ViewItemModel() { bmpImage = (BitmapImage)image, message = client.Username + "sent and Image!" });

            }
            else
            {

                messages.Add(new ViewItemModel() { message = "Not connected" });

            }

        }
        private bool CanSend()
        {
            return true;
        }
        private void Send()
        {
            if (this.client.isConnected)
            {
                if (currentMessage != string.Empty && currentMessage != "" && currentMessage.Length <= 150)
                {
                    MessagePacket personalMessage = new MessagePacket(currentMessage, targetUsername, true);
                    personalMessage.sender = this.client.Username;
                    this.client.TrySendObject(personalMessage);

                    messages.Add(new ViewItemModel() { message = client.Username + ":" + personalMessage.message });
                    currentMessage = "";
                }
                else if (currentMessage.Length > 150)
                {
                    messages.Add(new ViewItemModel() { message = "Your message is too long! Max Lenght is 150 characters" });
                }

            }
            else
            {
                currentMessage = "Not connected";
            }
        }
    }
}
