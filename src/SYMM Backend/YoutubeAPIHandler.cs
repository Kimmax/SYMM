using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;

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

        public List<YouTubeVideo> LoadChannelVideos(string channelName, List<YouTubeVideo> videos = null, string nextPageToken = null)
        {
            // Load the 'uploaded' playlist
            ChannelsResource.ListRequest channelPlaylistIDReq = YouTubeService.Channels.List("contentDetails");
            channelPlaylistIDReq.ForUsername = channelName;
            ChannelListResponse channelPlaylistIDResp = channelPlaylistIDReq.Execute();

            string channelUploadsPlaylistID = channelPlaylistIDResp.Items[0].ContentDetails.RelatedPlaylists.Uploads;

            // Load Uploaded Playlist
            PlaylistItemsResource.ListRequest playlistVideosReq = YouTubeService.PlaylistItems.List("snippet,id");
            playlistVideosReq.PlaylistId = channelUploadsPlaylistID;
            playlistVideosReq.PageToken = nextPageToken;
            playlistVideosReq.MaxResults = 50;
            PlaylistItemListResponse playlistVideosRes = playlistVideosReq.Execute();

            // Finnaly grab the video IDs
            if(videos == null)
                videos = videos = new List<YouTubeVideo>();

            // Populate video list
            foreach(PlaylistItem videoItem in playlistVideosRes.Items)
            {
                videos.Add(new YouTubeVideo(videoItem.Snippet.Title, videoItem.Snippet.ResourceId.VideoId, videoItem.Snippet.Description, videoItem.Snippet.PublishedAt, videoItem.Snippet.Thumbnails.High.Url, videoItem.Snippet.ChannelTitle, videoItem.Snippet.Position));
            }

            // Check if we have more to grab
            if (!string.IsNullOrEmpty(playlistVideosRes.NextPageToken))
                videos = LoadChannelVideos(channelName, videos, playlistVideosRes.NextPageToken);

            return videos;
        }
    }
}