using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SYMM_Frontend_WPF.View_model
{
    public class DownloadStatusModel : INotifyPropertyChanged
    {
        private string _downloadStatus = "Waiting";
        public string DownloadStatus
        {
            get { return _downloadStatus; }
            set
            {
                _downloadStatus = value;
                NotifyPropertyChanged("DownloadStatus");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
