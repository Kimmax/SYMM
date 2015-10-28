using System;
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

        public void DownloadVideoNonBlocking(YouTubeVideo video, string dest, bool extractAudio = true)
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

            new Thread(() => { downloader.DownloadVideo(dest, extractAudio); }).Start();
        }

        public void DownloadVideo(YouTubeVideo video, string dest, bool extractAudio = true)
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

            downloader.DownloadVideo(dest, extractAudio);
        }

        public string BuildSavePath(string dest, YouTubeVideo video)
        {
            try
            {
                return Path.Combine(dest, video.VideoTitle.Split('-')[1].Trim());
            }
            catch
            {
                return Path.Combine(dest, video.VideoTitle);
            }
           
        }

        public bool SongExists(string dest)
        {
            string[] AudioExtensions = new string[] { ".aac", ".mp3", ".wav", ".m4a", ".wma", ".ogg" };
            
            foreach(string extension in AudioExtensions)
            {
                if(File.Exists(dest + extension))
                    return true;
            }

            return false;
        }
    }
}
