using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
using TCP_Chat.Commands;
using TCPClientServer;

namespace TCP_Chat.ViewModels
{
   public class PersonalChatViewModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public string targetUsername { get; set; }
        public Client client { get; set; }
        private ObservableCollection<object> _messages = new ObservableCollection<object>();
        public ObservableCollection<object> messages
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
            messages = new ObservableCollection<object>();
           
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
        private void SendFile()
        {
            if (this.client.isConnected)
            {
                fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();

                string filePath = fileDialog.FileName;
                ImagePacket imagePacket = new ImagePacket();
                imagePacket.Imagebmp = new System.Drawing.Bitmap(filePath);
                imagePacket.isPersonal = true;
                imagePacket.sender = this.client.Username;
                imagePacket.targetUsername = targetUsername;
                this.client.TrySendObject(imagePacket);
            }
            else
            {
                messages.Add("not connected");
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
                MessagePacket personalMessage = new MessagePacket(currentMessage, targetUsername, true);
                personalMessage.sender = this.client.Username;
                this.client.TrySendObject(personalMessage);
                messages.Add(personalMessage.message);
                currentMessage = "";
            }
            else
            {
                currentMessage = "Not connected";
            }
        }
    }
}
