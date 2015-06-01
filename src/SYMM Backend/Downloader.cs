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
        public event EventHandler<ProgressEventArgs> DownloadProgressChanged;
        public event EventHandler<ProgressEventArgs> AudioExtractionProgressChanged;

        public void DownloadVideo(string watchID, string dest)
        {
            // Yotube url
            string link = "http://youtube.com/watch?v=" + watchID;

            /*
             * Get the available video formats.
             * We'll work with them in the video and audio download examples.
             */
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

            /*
             * We want the last extractable video with the highest audio quality.
             */
            VideoInfo video = videoInfos
                .Where(info => info.CanExtractAudio && info.AudioBitrate > 0)
                .OrderBy(info => info.AudioBitrate)
                .Last();

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            /*
             * Create the audio downloader.
             * The first argument is the video where the audio should be extracted from.
             * The second argument is the path to save the audio file.
             */
            var audioDownloader = new AudioDownloader(video, Path.Combine(dest, video.Title + video.AudioExtension));

            // Register the progress events. We treat the download progress as 85% of the progress and the extraction progress only as 15% of the progress,
            // because the download will take much longer than the audio extraction.
            audioDownloader.DownloadProgressChanged += (sender, args) => this.DownloadProgressChanged(this, new SYMM_Backend.ProgressEventArgs(args.ProgressPercentage));
            audioDownloader.AudioExtractionProgressChanged += (sender, args) => this.AudioExtractionProgressChanged(this, new SYMM_Backend.ProgressEventArgs(args.ProgressPercentage));

            /*
             * Execute the audio downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            audioDownloader.Execute();
        }
    }
}
