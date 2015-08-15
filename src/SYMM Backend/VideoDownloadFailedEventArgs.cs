using System;

namespace SYMM_Backend
{
    public class VideoDownloadFailedEventArgs : EventArgs
    {
        public VideoDownloadFailedEventArgs(YouTubeVideo video, Exception innerException)
        {
            this.Video = video;
            this.InnerException = innerException;
        }

        /// <summary>
        /// Returns the completed video object
        /// </summary>
        public YouTubeVideo Video { get; private set; }
        public Exception InnerException { get; private set; }
    }
}