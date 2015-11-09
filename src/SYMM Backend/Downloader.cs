using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YoutubeExtractor;

namespace SYMM_Backend
{
    public class VideoDownloader
    {
        public event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;
        public event EventHandler<DownloadProgressEventArgs> AudioExtractionProgressChanged;
        public event EventHandler<VideoDownloadCompleteEventArgs> VideoDownloadComplete;
        public event EventHandler<VideoDownloadFailedEventArgs> VideoDownloadFailed;

        public event EventHandler<DownloadProgressEventArgs> StreamPositionChanged;
        public event EventHandler<VideoDownloadCompleteEventArgs> StreamFinished;

        private readonly YouTubeVideo video;

        public YouTubeVideo Video
        {
            get { return video; }
        }

        public VideoDownloader(YouTubeVideo video)
        {
            this.video = video;
        }

        public void Execute(SYMMSettings settings)
        {
            try
            {
                string link = "http://youtube.com/watch?v=" + Video.VideoWatchID;
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

                if (settings.Action == SYMMSettings.Actions.ExtractAudio)
                {
                    VideoInfo videoInfo = videoInfos
                        .Where(info => info.CanExtractAudio && info.AudioBitrate > 0)
                        .OrderBy(info => info.AudioBitrate)
                        .OrderBy(info => info.AdaptiveType == AdaptiveType.Audio)
                        .OrderBy(info => info.AudioBitrate == settings.AudioBitrate)
                        .Last();

                    if (videoInfo.RequiresDecryption)
                    {
                        DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                    }

                    var audioDownloader = new AudioDownloader(videoInfo, settings.SavePath + String.Format("\\{0}.{1}", settings.PathSafefileName, settings.AudioFormat.ToString()), settings.AudioFormat.ToString());

                    // Track the amount of progress we had the last time, so we can prevent multiple calls without change
                    int lastPrgs = -1;

                    // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
                    // because the download will take much longer than the audio extraction.
                    audioDownloader.DownloadProgressChanged += (sender, args) =>
                    {
                        if (lastPrgs != (int)args.ProgressPercentage)
                        {
                            if (this.DownloadProgressChanged != null)
                                this.DownloadProgressChanged(this, new SYMM_Backend.DownloadProgressEventArgs(args.ProgressPercentage, this.video));

                            lastPrgs = (int)args.ProgressPercentage;
                        }
                    };

                    lastPrgs = -1;

                    audioDownloader.AudioExtractionProgressChanged += (sender, args) =>
                    {
                        if (lastPrgs != (int)args.ProgressPercentage)
                        {
                            if (this.AudioExtractionProgressChanged != null)
                                this.AudioExtractionProgressChanged(this, new SYMM_Backend.DownloadProgressEventArgs(args.ProgressPercentage, this.video));

                            lastPrgs = (int)args.ProgressPercentage;
                        }
                    };

                    audioDownloader.DownloadFinished += (sender, args) =>
                    {
                        if (this.VideoDownloadComplete != null)
                            this.VideoDownloadComplete(this, new VideoDownloadCompleteEventArgs(this.Video));
                    };

                    /*
                     * Execute the audio downloader.
                     * For GUI applications note, that this method runs synchronously.
                     */
                    audioDownloader.Execute();
                }
                else if(settings.Action == SYMMSettings.Actions.Download)
                {
                    VideoInfo videoInfo = videoInfos
                        .Where(info => info.AudioBitrate > 0 && info.Resolution > 0)
                        .OrderBy(info => info.AudioBitrate)
                        .OrderBy(info => info.Resolution)
                        .OrderBy(info => info.Resolution == settings.VideoResolution)
                        .Last();

                    if (videoInfo.RequiresDecryption)
                    {
                        DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                    }

                    YoutubeExtractor.VideoDownloader videoDownloader = new YoutubeExtractor.VideoDownloader(videoInfo, settings.SavePath + String.Format("\\{0}{1}", settings.PathSafefileName, videoInfo.VideoExtension));

                    // Track the amount of progress we had the last time, so we can prevent multiple calls without change
                    int lastPrgs = -1;

                    // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
                    // because the download will take much longer than the audio extraction.
                    videoDownloader.DownloadProgressChanged += (sender, args) =>
                    {
                        if (lastPrgs != (int)args.ProgressPercentage)
                        {
                            if (this.DownloadProgressChanged != null)
                                this.DownloadProgressChanged(this, new SYMM_Backend.DownloadProgressEventArgs(args.ProgressPercentage, this.video));

                            lastPrgs = (int)args.ProgressPercentage;
                        }
                    };

                    videoDownloader.DownloadFinished += (sender, args) =>
                    {
                        if (this.VideoDownloadComplete != null)
                            this.VideoDownloadComplete(this, new VideoDownloadCompleteEventArgs(this.Video));
                    };

                    /*
                     * Execute the audio downloader.
                     * For GUI applications note, that this method runs synchronously.
                     */
                    videoDownloader.Execute();
                } 
                else if(settings.Action == SYMMSettings.Actions.Stream)
                {
                    VideoInfo videoInfo = videoInfos
                        .Where(info => info.CanExtractAudio && info.AudioBitrate > 0)
                        .OrderBy(info => info.AudioBitrate)
                        .OrderBy(info => info.AdaptiveType == AdaptiveType.Audio)
                        .OrderBy(info => info.AudioBitrate == settings.AudioBitrate)
                        .Last();

                    if (videoInfo.RequiresDecryption)
                    {
                        DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                    }

                    var aduioStream = new AduioStreamer(videoInfo);

                    // Track the amount of progress we had the last time, so we can prevent multiple calls without change
                    int lastPrgs = -1;
                    aduioStream.StreamPositionChanged += (sender, args) =>
                    {
                        if (lastPrgs != (int)args.ProgressPercentage)
                        {
                            if (this.StreamPositionChanged != null)
                                this.StreamPositionChanged(this, new DownloadProgressEventArgs(args.ProgressPercentage, this.video));

                            lastPrgs = (int)args.ProgressPercentage;
                        }
                    };

                    aduioStream.StreamFinished += (sender, args) =>
                    {
                        if (this.StreamFinished != null)
                            this.StreamFinished(this, new VideoDownloadCompleteEventArgs(this.Video));
                    };

                    aduioStream.Execute();
                }
            }
            catch(YoutubeParseException ex)
            {
                if (this.VideoDownloadFailed != null)
                    this.VideoDownloadFailed(this, new VideoDownloadFailedEventArgs(this.Video, ex));
            }
        }
    }
}
