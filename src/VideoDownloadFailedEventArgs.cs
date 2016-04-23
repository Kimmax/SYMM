using System;
using SYMM.Interfaces;

namespace SYMM_Backend
{
    public class VideoDownloadFailedEventArgs : EventArgs
    {
        public VideoDownloadFailedEventArgs(IYouTubeVideo video, Exception innerException)
        {
            this.Video = video;
            this.InnerException = innerException;
        }

        /// <summary>
        /// Returns the completed video object
        /// </summary>
        public IYouTubeVideo Video { get; private set; }
        public Exception InnerException { get; private set; }
    }
}