using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace SYMM_Backend
{
    class YoutubeAPIHandler
    {
        private readonly string _APIKey;
        public string APIKey
        {
            get { return _APIKey; }
        }

        private readonly YouTubeService _YouTubeService;
        public YouTubeService YouTubeService
        {
            get { return _YouTubeService; }
        }

        public YoutubeAPIHandler(string APIKey)
        {
            this._APIKey = APIKey;
            this._YouTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApplicationName = "SYMM",
                ApiKey = this._APIKey,
            });
        }

        public void LoadChannelVideos(string nextPageToken = null)
        {
            ChannelsResource.ListRequest channelVidRequest = YouTubeService.Channels.List("snippet");
            channelVidRequest.Id = "OfficialTrapCity";
            channelVidRequest.MaxResults = 50;

            if (nextPageToken != null)
                channelVidRequest.PageToken = nextPageToken;

            ChannelListResponse channelVideos = channelVidRequest.Execute();
            
        }
    }
}