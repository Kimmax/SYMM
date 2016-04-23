using System;
using SYMM.Interfaces;

namespace SYMM_Backend
{
    public class VideoInformationLoadedEventArgs : EventArgs
    {
        public VideoInformationLoadedEventArgs(IYouTubeVideo video)
        {
            this.Video = video;
        }

        /// <summary>
        /// Returns the completed video object
        /// </summary>
        public IYouTubeVideo Video { get; private set; }
    }
}