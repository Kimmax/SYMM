﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SYMM_Backend
{
    public class SYMMHandler
    {
        private readonly YoutubeAPIHandler YouTubeHandler;

        public event EventHandler<VideoInformationLoadedEventArgs> OnVideoInformationLoaded;
        public event EventHandler<AllVideoInformationLoadedEventArgs> OnAllVideoInformationLoaded;

        public event EventHandler<DownloadProgressEventArgs> OnVideoDownloadProgressChanged;
        public event EventHandler<DownloadProgressEventArgs> OnVideoAudioExtractionProgressChanged;
        public event EventHandler<VideoDownloadCompleteEventArgs> OnVideoDownloadComplete;
        public event EventHandler<VideoDownloadFailedEventArgs> OnVideoDownloadFailed;

        private readonly string _APIKey;
        public string APIKey
        {
            get { return _APIKey; }
        }

        public SYMMHandler(string APIKey)
        {
            this._APIKey = APIKey;
            this.YouTubeHandler = new YoutubeAPIHandler(this.APIKey);
        }

        public List<YouTubeVideo> LoadVideosFromChannel(string channelName)
        {
            return YouTubeHandler.LoadChannelVideos(channelName);
        }

        public void LoadVideosFromChannelNonBlocking(string channelName)
        {
            YouTubeHandler.OnAllVideoInformationLoaded += (s, e) =>
            {
                if (OnAllVideoInformationLoaded != null)
                    OnAllVideoInformationLoaded(this, e);
            };

            YouTubeHandler.OnVideoInformationLoaded += (s, e) =>
            {
                if (OnVideoInformationLoaded != null)
                    OnVideoInformationLoaded(this, e);
            };

            YouTubeHandler.LoadChannelVideos(channelName);
        }

        public void LoadVideosFromURL(string url)
        {
            YouTubeHandler.OnAllVideoInformationLoaded += (s, e) =>
            {
                if (OnAllVideoInformationLoaded != null)
                    OnAllVideoInformationLoaded(this, e);
            };

            YouTubeHandler.OnVideoInformationLoaded += (s, e) =>
            {
                if (OnVideoInformationLoaded != null)
                    OnVideoInformationLoaded(this, e);
            };

            YouTubeHandler.LoadURLVideos(url);
        }

        public void DownloadVideoNonBlocking(YouTubeVideo video, SYMMSettings settings)
        {
            VideoDownloader downloader = new VideoDownloader(video);
            downloader.DownloadProgressChanged += (s, e) =>
            {
                if (OnVideoDownloadProgressChanged != null)
                    OnVideoDownloadProgressChanged(this, e);
            };

            downloader.AudioExtractionProgressChanged += (s, e) =>
            {
                if (OnVideoAudioExtractionProgressChanged != null)
                    OnVideoAudioExtractionProgressChanged(this, e);
            };

            downloader.VideoDownloadComplete += (s, e) =>
            {
                if (OnVideoDownloadComplete != null)
                    OnVideoDownloadComplete(this, e);
            };

            downloader.VideoDownloadFailed += (s, e) =>
            {
                if (OnVideoDownloadFailed != null)
                    OnVideoDownloadFailed(this, e);
            };

            new Thread(() => { downloader.DownloadVideo(settings); }).Start();
        }

        public void DownloadVideo(YouTubeVideo video, SYMMSettings settings)
        {
            VideoDownloader downloader = new VideoDownloader(video);
            downloader.DownloadProgressChanged += (s, e) =>
            {
                if (OnVideoDownloadProgressChanged != null)
                    OnVideoDownloadProgressChanged(this, e);
            };

            downloader.AudioExtractionProgressChanged += (s, e) =>
            {
                if (OnVideoAudioExtractionProgressChanged != null)
                    OnVideoAudioExtractionProgressChanged(this, e);
            };

            downloader.VideoDownloadComplete += (s, e) =>
            {
                if (OnVideoDownloadComplete != null)
                    OnVideoDownloadComplete(this, e);
            };

            downloader.VideoDownloadFailed += (s, e) =>
            {
                if (OnVideoDownloadFailed != null)
                    OnVideoDownloadFailed(this, e);
            };

            downloader.DownloadVideo(settings);
            downloader = null;
        }

        public string BuildPathSafeName(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c, '-'));
        }

        public void ResetEvents()
        {
            OnVideoInformationLoaded = null;
            OnAllVideoInformationLoaded = null;
            OnVideoDownloadProgressChanged = null;
            OnVideoAudioExtractionProgressChanged = null;
            OnVideoDownloadComplete = null;
            OnVideoDownloadFailed = null;
            YouTubeHandler.ResetEvents();
        }
    }
}
