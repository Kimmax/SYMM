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

        public string LoadChannelVideos(string channelName, string nextPageToken = null)
        {
            // Load the 'uploaded' playlist
            ChannelsResource.ListRequest channelPlaylistIDReq = YouTubeService.Channels.List("contentDetails");
            channelPlaylistIDReq.ForUsername = channelName;
            ChannelListResponse channelPlaylistIDResp = channelPlaylistIDReq.Execute();

            string channelUploadsPlaylistID = channelPlaylistIDResp.Items[0].ContentDetails.RelatedPlaylists.Uploads;

            // Load Uploaded Playlist
            PlaylistItemsResource.ListRequest playlistVideosReq = YouTubeService.PlaylistItems.List("snippet,id");
            playlistVideosReq.PlaylistId = channelUploadsPlaylistID;
            playlistVideosReq.MaxResults = 50;
            PlaylistItemListResponse playlistVideosRes = playlistVideosReq.Execute();

            // Finnaly grab the video IDs
            string videoIDs = "";
            foreach(PlaylistItem videoItem in playlistVideosRes.Items)
            {
                videoIDs += videoItem.Snippet.Title + ": " + videoItem.Id + "\n";
            }

            return videoIDs;
        }
    }
}