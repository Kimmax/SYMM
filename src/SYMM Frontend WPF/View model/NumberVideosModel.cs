using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SYMM_Frontend_WPF.View_model
{
    public class NumberVideosModel : INotifyPropertyChanged
    {
        private int _totalNumberVideos = 0;
        public int TotalNumberVideos
        {
            get { return _totalNumberVideos; }
            set
            {
                _totalNumberVideos = value;
                NotifyPropertyChanged("TotalNumberVideos");
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
