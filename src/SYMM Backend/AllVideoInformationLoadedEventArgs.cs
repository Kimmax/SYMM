using System;
using System.Collections.Generic;

namespace SYMM_Backend
{
    public class AllVideoInformationLoadedEventArgs : EventArgs
    {
        public AllVideoInformationLoadedEventArgs(List<YouTubeVideo> video)
        {
            this.Videos = video;
        }

        /// <summary>
        /// Returns the completed video object
        /// </summary>
        public List<YouTubeVideo> Videos { get; private set; }
    }
}