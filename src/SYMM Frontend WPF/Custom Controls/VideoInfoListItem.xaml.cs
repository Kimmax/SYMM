using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SYMM_Backend;
using SYMM_Frontend_WPF.View_model;

namespace SYMM_Frontend_WPF
{
    /// <summary>
    /// Interaktionslogik für VideoInfoListItem.xaml
    /// </summary>
    public partial class VideoInfoListItem : UserControl
    {
        // The vido object will be saved here
        private YouTubeVideo _video;

        // Public reading, private setting
        public YouTubeVideo Video
        {
            get { return _video; }
            private set { _video = value; }
        }

        public bool? IsSelectedToDownload
        {
            get { return checkDownloadBox.IsChecked; }
        }

        // Constructor
        public VideoInfoListItem(YouTubeVideo video)
        {
            InitializeComponent();

            // Set info fields on URL
            this._video = video;
            this.txtTitle.Text = Video.VideoTitle;
            this.txtChannel.Text = Video.ChannelTitle;

            // Init databinding
            this.txtDownloadStatus.DataContext = new DownloadStatusModel();

            // Receive and set thumbnail on GUI
            SetThumb(this.Video.ThumbURL);
        }

        public void SetThumb(string url)
        {
            // New image
            var image = new Image();

            // Origin: Googles server
            var fullFilePath = url;

            // Receive image from web
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            // Annnd set it
            this.imgThumb.Source = bitmap;
        }
    }
}
