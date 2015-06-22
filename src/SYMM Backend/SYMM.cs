using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SYMM_Backend
{
    public class SYMMHandler
    {
        private readonly YoutubeAPIHandler YouTubeHandler;

        private readonly string _APIKey;
        public string APIKey
        {
            get { return _APIKey; }
        }

        public SYMMHandler(string APIKey)
        {
            this._APIKey = APIKey;
            this.YouTubeHandler = new YoutubeAPIHandler(this.APIKey);
        }

        public List<YouTubeVideo> LoadVideosFromChannel(string channelName)
        {
            return YouTubeHandler.LoadChannelVideos(channelName);
        }

        public void LoadVideosFromChannelNonBlocking(string channelName)
        {
            YouTubeHandler.LoadChannelVideos(channelName);
        }
    }
}
