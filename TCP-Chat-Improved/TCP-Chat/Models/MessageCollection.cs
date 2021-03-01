using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TCP_Chat.Models
{
    public class MessageCollection : INotifyPropertyChanged // Currently unused
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ViewItemModel> _messages = new ObservableCollection<ViewItemModel>();
        public ObservableCollection<ViewItemModel> messages
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged("messages"); }
        }
        public MessageCollection()
        {
            messages =  new ObservableCollection<ViewItemModel>();
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
