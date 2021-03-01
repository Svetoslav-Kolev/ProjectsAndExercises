using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        public string filePath = string.Empty;
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

        private bool CanSendFile()
        {
            if (this.client.isConnected)
            {
                return true;
            }
            return false;
        }
        private async Task SendFile()
        {
            if (this.client.isConnected)
            {
                try
                {                    
                    ImagePacket imagePacket = new ImagePacket();
                    imagePacket.Imagebmp = new System.Drawing.Bitmap(filePath);
                    imagePacket.isPersonal = true;
                    imagePacket.sender = this.client.Username;
                    imagePacket.targetUsername = targetUsername;
                    try
                    {
                        await this.client.TrySendObject(imagePacket);
                    }
                    catch
                    {

                        if (this.client.requestDisconnection)
                        {
                            AddMessage(new ViewItemModel() { message = "You have been disconnected." });
                        }
                        else
                        {
                            AddMessage(new ViewItemModel() { message = "You have been forcefully disconnected. Try to reconnect from the main window." });
                        }
                       
                    }
                    finally
                    {
                        filePath = string.Empty;
                    }
                    BitmapToImageConverter bmpConverter = new BitmapToImageConverter();
                    var image = bmpConverter.Convert(imagePacket.Imagebmp);

                    AddMessage(new ViewItemModel() { bmpImage = (BitmapImage)image, message = client.Username + "sent and Image!" });
                }
                catch (ArgumentException)
                {

                    AddMessage(new ViewItemModel() { message = "File is not in a valid format, please send only  jpeg, png , gif or jpg files!" });
                    filePath = string.Empty;
                }


            }
            else
            {

                AddMessage(new ViewItemModel() { message = "Not connected" });
            }
            filePath = string.Empty;
        }
        private bool CanSend()
        {
            if (this.client.isConnected)
            {
                return true;
            }
            return false;
        }
        private async Task Send()
        {
            if (this.client.isConnected)
            {
                if (currentMessage != string.Empty && currentMessage != "" && currentMessage.Length <= 150)
                {
                    MessagePacket personalMessage = new MessagePacket(currentMessage, targetUsername, true);
                    personalMessage.sender = this.client.Username;
                    try
                    {
                        await this.client.TrySendObject(personalMessage);
                        messages.Add(new ViewItemModel() { message = client.Username + ":" + personalMessage.message });
                        currentMessage = "";
                    }
                    catch 
                    {
                        if (this.client.requestDisconnection)
                        {
                            AddMessage(new ViewItemModel() { message = "You have been disconnected." });
                        }
                        else
                        {
                            AddMessage(new ViewItemModel() { message = "You have been forcefully disconnected. Try to reconnect from the Main window" });
                        }
                    }

                }
                else if (currentMessage.Length > 150)
                {
                    AddMessage(new ViewItemModel() { message = "Your message is too long! Max Lenght is 150 characters" });
                }

            }
            else
            {
                currentMessage = "Not connected";
            }
        }
        public void AddMessage(ViewItemModel item)
        {
            messages.Add(item);
        }
    }
}
