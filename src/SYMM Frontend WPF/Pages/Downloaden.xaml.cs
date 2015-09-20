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
    /// Codebehind for Downloaden.xaml
    /// </summary>
    public partial class Downloaden : Page, IContent
    {
        // This initalizes the downloader, a referenze to the backend, used to do the actual work.
        // You need to update the API Key in the projects settings file, key "YTDataAPIv3Key"
        SYMMHandler downloader = new SYMMHandler(Properties.Settings.Default.YTDataAPIv3Key);

        // Videolist used to store all videos in it's original class, before giving it to the view model
        // Mostly used for references
        List<YouTubeVideo> rawVideoList = new List<YouTubeVideo>();

        // The consturctor of the class
        public Downloaden()
        {
            InitializeComponent();

            // Set databindings to viewmodels
            videoInfoList.DataContext = new VideoInfoListModel();
            labNumVids.DataContext = new NumberVideosModel();
        }

        /// <summary>
        /// Displays all information received from the backend and displays it on the GUI.
        /// This method loads all uploaded videos from one channel by the channel name.
        /// </summary>
        /// <param name="channel">The name of the channel infos should be received from.</param>
        public void LoadByChannelName(string channel)
        {
            // Display information for one video when all information is loaded
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
                // Start actual work on backend
                downloader.LoadVideosFromChannelNonBlocking(channel);
            }).Start();
        }

        /// <summary>
        /// Displays all information received from the backend and displays it on the GUI.
        /// This method loads all uploaded videos from one url (playlist or single video)
        /// </summary>
        /// <param name="url">The url of the playlist or video the infos should be received from.</param>
        public void LoadByURL(string url)
        {
            // Display information for one video when all information is loaded
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
                // Start actual work on backend
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


        /// <summary>
        /// Activates or deactivates the automatic scrolling.
        /// </summary>
        private Boolean AutoScroll = true;
        /// <summary>
        /// Automaticly scroll the scroller when new video views are being loaded and added out of view
        /// Does not scroll when the scrollbar has been moved manually until it's set on the very bottom again.
        /// </summary>
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

        /// <summary>
        /// Starts the process of downloading the videos.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Defines how many videos are allowed to download at the same time
            int maxSynDownloadingVideo = 4;

            // Holds current number of downloading videos. Used to control maximum syncron downloads
            int workingVideos = 0;

            // Folder path the audio file gets saved at
            string dest = @"D:\Music\Youtube\Uploads by Trap City";

            // Reset event controlling max Downloads. If workingVideos >= maxSynDownloadVideo the thread waits for this event to set
            ManualResetEvent resetEvent = new ManualResetEvent(false);

            // Register changed download progress of one video
            downloader.OnVideoDownloadProgressChanged += (dsender, deventargs) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Show progress on GUI
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Downloading " + deventargs.ProgressPercentage.ToString("f2") + "%");
                }), DispatcherPriority.Background);
            };

            // Register changed audio extraction progress of one video
            downloader.OnVideoAudioExtractionProgressChanged += (dsender, deventargs) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Show progress on GUI
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Extracting Audio " + deventargs.ProgressPercentage.ToString("f2") + "%");
                }), DispatcherPriority.Background);
            };

            // Register finished download of one video
            downloader.OnVideoDownloadComplete += (dsender, deventargs) =>
            {
                // One video download is done, signal download thread to go for the next one
                resetEvent.Set();
                resetEvent.Reset();

                // One video is done. Note that
                workingVideos--;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Show on GUI that video download is complete
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Download complete");
                }), DispatcherPriority.Background);
            };

            // Register when a video failed to download
            downloader.OnVideoDownloadFailed += (dsender, deventargs) =>
            {
                // One video download failed, signal download thread to go for the next one
                resetEvent.Set();
                resetEvent.Reset();

                // Even a fail frees up working space
                workingVideos--;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Show the user that the download failed. Meh.
                    (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(deventargs.Video, "Download failed :(");
                }), DispatcherPriority.Background);
            };

            // Here actual work gets done. Don't block the GUI, so new Thread
            Thread downloadWorker = new Thread(() =>
            {
                // We want to download every video on this list
                foreach (YouTubeVideo video in rawVideoList)
                {
                    // If all download slots are full, let the loop wait for them to get free
                    if (workingVideos >= maxSynDownloadingVideo)
                        resetEvent.WaitOne();
                   
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        // Show the start of the download on GUI
                        (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(video, "Starting download..");
                    }), DispatcherPriority.Background);

                    // Prepare backend
                    string audioDestination = downloader.BuildSavePath(dest, video);

                    // Looks like we downloaded that already. Skip.
                    if(downloader.SongExists(audioDestination))
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            // Show the user that we safed some time and bandwidth
                            (videoInfoList.DataContext as VideoInfoListModel).UpdateVideoDownloadStatus(video, "Skipped: Already downloaded");
                        }), DispatcherPriority.Background);

                        // Skip the rest
                        continue;
                    }

                    // Tell backend to download the video spceifed to destination spceifed in the variable
                    downloader.DownloadVideoNonBlocking(video, audioDestination);

                    // We are using one download slot. Not that.
                    workingVideos++;
                }
            });

            // Thread is set, let's go!
            downloadWorker.Start();
        }
    }
}
