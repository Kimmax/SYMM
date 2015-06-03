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

namespace SYMM_Frontend_WPF
{
    /// <summary>
    /// Interaktionslogik für VideoItemList.xaml
    /// </summary>
    public partial class VideoItemList : UserControl
    {
        List<VideoInfoListItem> videoItems = new List<VideoInfoListItem>();

        public VideoItemList()
        {
            InitializeComponent();
        }

        public void AddVideoItem(VideoInfoListItem video)
        {
            videoItems.Add(video);
            this.videoInfoPanel.Children.Add(video);
        }

        public void RemoveVideoItem(VideoInfoListItem video)
        {
            this.videoInfoPanel.Children.Remove(video);
        }
    }
}
