using FirstFloor.ModernUI.Windows.Controls;
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
using SYMM_Frontend_WPF.Dialogs;
using SYMM_Backend;
using System.Threading;
using FirstFloor.ModernUI.Windows;

namespace SYMM_Frontend_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Erfassen.xaml
    /// </summary>
    public partial class Downloaden : Page, IContent
    {
        public Downloaden()
        {
            InitializeComponent();
        }

        List<YouTubeVideo> loadedVideos = new List<YouTubeVideo>();
        SYMMHandler downloader = new SYMMHandler("AIzaSyAj82IqIloWupFnhn-hmmUo7iAkcj2xk3g");

        void PopulateUI()
        {
            loadedVideos.ForEach(video =>
            {
                this.videoList.AddVideoItem(video);
            });
        }

        public void LoadByChannelName(string channel)
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

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            string method = e.Source.ToString().Split('?')[1].Split('&')[0].Split('=')[1];
            if (method == "channelname")
            {
                LoadByChannelName(e.Source.ToString().Split('?')[1].Split('&')[1].Split('=')[1]);
            }
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }
    }
}
