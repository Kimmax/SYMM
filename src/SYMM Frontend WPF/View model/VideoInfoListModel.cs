using SYMM_Backend;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SYMM_Frontend_WPF.View_model
{
    public class VideoInfoListModel : INotifyPropertyChanged
    {
        private ObservableCollection<VideoInfoListItem> _loadedVideos = new ObservableCollection<VideoInfoListItem>();

        public ObservableCollection<VideoInfoListItem> LoadedVideos
        {
            set { 
                _loadedVideos = value;
                NotifyPropertyChanged("LoadedVideos");
            }

            get { return _loadedVideos; }
        }

        public void AddVideo(YouTubeVideo video)
        {
            LoadedVideos.Add(new VideoInfoListItem(video));
            NotifyPropertyChanged("LoadedVideos");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
