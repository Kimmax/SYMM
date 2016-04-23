using ResolveList;
using SYMM.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SYMM_Backend
{
    public class SYMMHandler
    {
        private readonly YTPlaylistResolver YouTubeHandler;

        public event EventHandler<VideoInformationLoadedEventArgs> OnVideoInformationLoaded;
        public event EventHandler<AllVideoInformationLoadedEventArgs> OnAllVideoInformationLoaded;

        public event EventHandler<DownloadProgressEventArgs> OnVideoDownloadProgressChanged;
        public event EventHandler<DownloadProgressEventArgs> OnVideoAudioExtractionProgressChanged;
        public event EventHandler<VideoDownloadCompleteEventArgs> OnVideoDownloadComplete;
        public event EventHandler<VideoDownloadFailedEventArgs> OnVideoDownloadFailed;

        public event EventHandler<DownloadProgressEventArgs> OnStreamPostionChanged;
        public event EventHandler<VideoDownloadCompleteEventArgs> OnStreamComplete;

        private readonly string _APIKey;
        public string APIKey
        {
            get { return _APIKey; }
        }

        public SYMMHandler(string APIKey)
        {
            this._APIKey = APIKey;
            this.YouTubeHandler = new YTPlaylistResolver("SYMM", this.APIKey);
        }

        public string GetYoutubeChannelPlaylist(string channel)
        {
            return "https://www.youtube.com/list=" + YouTubeHandler.GetChannelPlaylist(channel);
        }

        public void LoadVideosFromURL(ISYMMSettings settings)
        {
            YouTubeHandler.OnAllVideoInformationLoaded += (s, e) =>
            {
                if (OnAllVideoInformationLoaded != null)
                    OnAllVideoInformationLoaded(this, new AllVideoInformationLoadedEventArgs(e.Videos));
            };

            YouTubeHandler.OnVideoInformationLoaded += (s, e) =>
            {
                if (OnVideoInformationLoaded != null)
                    OnVideoInformationLoaded(this, new VideoInformationLoadedEventArgs(e.Video));
            };

            YouTubeHandler.LoadURLVideos(settings);
        }

        public void DownloadVideoNonBlocking(IYouTubeVideo video, ISYMMSettings settings)
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

            new Thread(() => { downloader.Execute(settings); }).Start();
        }

        public void Execute(IYouTubeVideo video, ISYMMSettings settings)
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

            downloader.StreamPositionChanged += (s, e) =>
            {
                if (OnStreamPostionChanged != null)
                    OnStreamPostionChanged(this, e);
            };

            downloader.StreamFinished += (s, e) =>
            {
                if (OnStreamComplete != null)
                    OnStreamComplete(this, e);
            };

            downloader.Execute(settings);
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
            OnStreamComplete = null;
            OnStreamPostionChanged = null;
            YouTubeHandler.ResetEvents();
        }
    }
}
