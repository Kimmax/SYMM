using System;
using SYMM.Interfaces;

namespace SYMM_Backend
{
    public class DownloadProgressEventArgs : EventArgs
    {
        public DownloadProgressEventArgs(double progressPercentage, IYouTubeVideo video = null)
        {
            this.ProgressPercentage = progressPercentage;
            this.Video = video;
        }

        /// <summary>
        /// Gets or sets a token whether the operation that reports the progress should be canceled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the progress percentage in a range from 0.0 to 100.0.
        /// </summary>
        public double ProgressPercentage { get; private set; }

        public IYouTubeVideo Video { get; set; }
    }
}