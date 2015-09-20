using SYMM_Backend;
using System;
using System.Collections.Generic;
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

        public bool IsDownloadSelected(YouTubeVideo video)
        {
            return (bool)(LoadedVideos.First(x => x.Video == video)).IsSelectedToDownload;
        }

        public void UpdateVideoDownloadStatus(YouTubeVideo video, string status)
        {
            (LoadedVideos.First(x => x.Video == video).txtDownloadStatus.DataContext as DownloadStatusModel).DownloadStatus = status;
        }

        public void AddVideo(YouTubeVideo video)
        {
            LoadedVideos.Add(new VideoInfoListItem(video));
            NotifyPropertyChanged("LoadedVideos");
        }

        public void RemoveVideo(YouTubeVideo video)
        {
            LoadedVideos.Remove(LoadedVideos.First(x => x.Video == video));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
