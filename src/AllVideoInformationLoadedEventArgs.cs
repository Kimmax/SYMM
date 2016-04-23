using ResolveList;
using System;
using System.Collections.Generic;
using SYMM.Interfaces;

namespace SYMM_Backend
{
    public class AllVideoInformationLoadedEventArgs : EventArgs
    {
        public AllVideoInformationLoadedEventArgs(List<IYouTubeVideo> video)
        {
            this.Videos = video;
        }

        /// <summary>
        /// Returns the completed video object
        /// </summary>
        public List<IYouTubeVideo> Videos { get; private set; }
    }
}