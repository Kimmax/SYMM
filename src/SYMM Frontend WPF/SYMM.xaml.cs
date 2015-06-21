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
using System.Threading;
using SYMM_Backend;
using FirstFloor.ModernUI.Windows.Controls;

namespace SYMM_Frontend_WPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        List<YouTubeVideo> loadedVideos = new List<YouTubeVideo>();
        SYMMHandler downloader = new SYMMHandler("AIzaSyAj82IqIloWupFnhn-hmmUo7iAkcj2xk3g");

        void PopulateUI()
        {
            loadedVideos.ForEach(video =>
            {
                //this.LoadedVideoList.AddVideoItem(video);
            });
        }

        void LoadFromChannel(string channel)
        {
            ManualResetEvent isContentLoadedResetEvent = new ManualResetEvent(false);
            new Thread(() =>
            {
                this.loadedVideos = downloader.LoadVideosFromChannel(channel);
                isContentLoadedResetEvent.Set();
            }).Start();

            isContentLoadedResetEvent.WaitOne();
            PopulateUI();
        }

        private void menuURLLoadChannel_Click(object sender, RoutedEventArgs e)
        {
            LoadFromChannel("OfficialTrapCity");
        }
    }
}
