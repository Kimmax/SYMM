using FirstFloor.ModernUI.Windows;
using SYMM_Backend;
using SYMM_Frontend_WPF.View_model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SYMM_Frontend_WPF.Pages
{
    /// <summary>
    /// Interaktionslogik für Erfassen.xaml
    /// </summary>
    public partial class Downloaden : Page, IContent
    {
        SYMMHandler downloader = new SYMMHandler("AIzaSyAj82IqIloWupFnhn-hmmUo7iAkcj2xk3g");
        List<YouTubeVideo> rawVideoList = new List<YouTubeVideo>();

        public Downloaden()
        {
            InitializeComponent();
            videoInfoList.DataContext = new VideoInfoListModel();
            labNumVids.DataContext = new NumberVideosModel();
        }

        public void LoadByChannelName(string channel)
        {
            downloader.OnVideoInformationLoaded += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    rawVideoList.Add(e.Video);
                    (videoInfoList.DataContext as VideoInfoListModel).AddVideo(e.Video);
                    (labNumVids.DataContext as NumberVideosModel).TotalNumberVideos++;
                }), DispatcherPriority.Background);
            };

            new Thread(() =>
            {
                downloader.LoadVideosFromChannelNonBlocking(channel);
            }).Start();
        }

        public void LoadByURL(string url)
        {
            downloader.OnVideoInformationLoaded += (s, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    rawVideoList.Add(e.Video);
                    (videoInfoList.DataContext as VideoInfoListModel).AddVideo(e.Video);
                    (labNumVids.DataContext as NumberVideosModel).TotalNumberVideos++;
                }), DispatcherPriority.Background);
            };

            new Thread(() =>
            {
                downloader.LoadVideosFromURL(url);
            }).Start();
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            string method = e.Source.ToString().Split('?')[1].Split('&')[0].Split('=')[1];
            string extra = Regex.Split(e.Source.ToString(), "&extra=")[1];
            if (method == "channelname")
            {
                LoadByChannelName(extra);
            }
            else if (method == "url")
            {
                LoadByURL(extra);
            }
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {

        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        private Boolean AutoScroll = true;
        private void infoScroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset autoscroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (infoScroller.VerticalOffset == infoScroller.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set autoscroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset autoscroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : autoscroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and autoscroll mode set
                // Autoscroll
                infoScroller.ScrollToVerticalOffset(infoScroller.ExtentHeight);
            }
        }

        private void btnDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int maxSynDownloadingVideo = 4;
            int workingVideos = 0;
            string dest = @"D:\Music\Youtube\Uploads by Trap City";

            ManualResetEvent resetEvent = new ManualResetEvent(false);

            downloader.OnVideoDownloadProgressChanged += (dsender, deventargs) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Downloading " + deventargs.ProgressPercentage.ToString("f2") + "%");
                }), DispatcherPriority.Background);
            };

            downloader.OnVideoAudioExtractionProgressChanged += (dsender, deventargs) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Extracting Audio " + deventargs.ProgressPercentage.ToString("f2") + "%");
                }), DispatcherPriority.Background);
            };

            downloader.OnVideoDownloadComplete += (dsender, deventargs) =>
            {
                resetEvent.Set();
                resetEvent.Reset();
                workingVideos--;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Download complete");
                }), DispatcherPriority.Background);
            };

            downloader.OnVideoDownloadFailed += (dsender, deventargs) =>
            {
                resetEvent.Set();
                resetEvent.Reset();
                workingVideos--;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Download failed :(");
                }), DispatcherPriority.Background);
            };

            Thread downloadWorker = new Thread(() =>
            {
                foreach (YouTubeVideo video in rawVideoList)
                {
                    if (workingVideos >= maxSynDownloadingVideo)
                        resetEvent.WaitOne();
                   
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(video, "Starting download..");
                    }), DispatcherPriority.Background);

                    string audioDestination = downloader.BuildSavePath(dest, video);

                    if(downloader.SongExists(audioDestination))
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(video, "Skipped: Already downloaded");
                        }), DispatcherPriority.Background);
                        continue;
                    }

                    downloader.DownloadVideoNonBlocking(video, audioDestination);
                    workingVideos++;
                }
            });

            downloadWorker.Start();
        }
    }
}
