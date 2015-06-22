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

namespace SYMM_Frontend_WPF
{
    /// <summary>
    /// Interaktionslogik für VideoInfoListItem.xaml
    /// </summary>
    public partial class VideoInfoListItem : UserControl
    {
        private YouTubeVideo _video;

        public YouTubeVideo Video
        {
            get { return _video; }
            private set { _video = value; }
        }

        public VideoInfoListItem(YouTubeVideo video)
        {
            InitializeComponent();
            this._video = video;
            this.txtTitle.Text = Video.VideoTitle;
            this.txtChannel.Text = Video.ChannelTitle;

            SetThumb(this.Video.ThumbURL);
        }

        public void SetThumb(string url)
        {
            var image = new Image();
            var fullFilePath = url;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            this.imgThumb.Source = bitmap;
        }
    }
}
