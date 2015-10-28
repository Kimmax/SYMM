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

        private readonly YouTubeVideo video;

        public YouTubeVideo Video
        {
            get { return video; }
        }

        public VideoDownloader(YouTubeVideo video)
        {
            this.video = video;
        }

        public void DownloadVideo(string dest, bool extractAudio)
        {
            // Yotube url
            string link = "http://youtube.com/watch?v=" + Video.VideoWatchID;

            /*
             * Get the available video formats.
             * We'll work with them in the video and audio download examples.
             */
            try
            {
                if (extractAudio)
                {
                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
                    /*
                     * We want the last extractable video with the highest audio quality.
                    */
                    VideoInfo videoInfo = videoInfos
                        .Where(info => info.CanExtractAudio && info.AudioBitrate > 0)
                        .OrderBy(info => info.AudioBitrate)
                        .OrderBy(info => info.AdaptiveType == AdaptiveType.Audio)
                        .OrderBy(info => info.AudioType == AudioType.Aac)
                        .Last();

                    /*
                     * If the video has a decrypted signature, decipher it
                     */
                    if (videoInfo.RequiresDecryption)
                    {
                        DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                    }

                    /*
                     * Create the audio downloader.
                     * The first argument is the video where the audio should be extracted from.
                     * The second argument is the path to save the audio file.
                     */
                    var audioDownloader = new AudioDownloader(videoInfo, dest + videoInfo.AudioExtension);

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
                else
                {
                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
                    /*
                * We want the highest video quality
                */
                    VideoInfo videoInfo = videoInfos
                        .Where(info => info.AudioBitrate > 0 && info.Resolution > 0)
                        .OrderBy(info => info.AudioBitrate)
                        .OrderBy(info => info.Resolution <= 1024)
                        .Last();

                    /*
                     * If the video has a decrypted signature, decipher it
                     */
                    if (videoInfo.RequiresDecryption)
                    {
                        DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                    }

                    /*
                     * Create the audio downloader.
                     * The first argument is the video where the audio should be extracted from.
                     * The second argument is the path to save the audio file.
                     */
                    YoutubeExtractor.VideoDownloader videoDownloader = new YoutubeExtractor.VideoDownloader(videoInfo, dest + videoInfo.VideoExtension);

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
            }
            catch(YoutubeParseException ex)
            {
                if (this.VideoDownloadFailed != null)
                    this.VideoDownloadFailed(this, new VideoDownloadFailedEventArgs(this.Video, ex));
            }
        }
    }
}
