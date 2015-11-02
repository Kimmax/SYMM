using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public event EventHandler<VideoInformationLoadedEventArgs> OnVideoInformationLoaded;
        public event EventHandler<AllVideoInformationLoadedEventArgs> OnAllVideoInformationLoaded;

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
                videos = new List<YouTubeVideo>();

            // Populate video list
            foreach(PlaylistItem videoItem in playlistVideosRes.Items)
            {
                YouTubeVideo loadedVideo = new YouTubeVideo(videoItem.Snippet.Title, videoItem.Snippet.ResourceId.VideoId, videoItem.Snippet.Description, videoItem.Snippet.PublishedAt, videoItem.Snippet.Thumbnails.Default__.Url, videoItem.Snippet.ChannelTitle, videoItem.Snippet.Position);
                videos.Add(loadedVideo);

                if(OnVideoInformationLoaded != null)
                    OnVideoInformationLoaded(this, new VideoInformationLoadedEventArgs(loadedVideo));
            }

            // Check if we have more to grab
            if (!string.IsNullOrEmpty(playlistVideosRes.NextPageToken))
                videos = LoadChannelVideos(channelName, videos, playlistVideosRes.NextPageToken);

            if(OnAllVideoInformationLoaded != null)
                OnAllVideoInformationLoaded(this, new AllVideoInformationLoadedEventArgs(videos));

            return videos;
        }

        public List<YouTubeVideo> LoadURLVideos(string url, List<YouTubeVideo> videos = null, string nextPageToken = null)
        {
            if (url.Contains("playlist"))
            {
                string playlistID = Regex.Split(url, @"playlist\?list=")[1];

                // Load Uploaded Playlist
                PlaylistItemsResource.ListRequest playlistVideosReq = YouTubeService.PlaylistItems.List("snippet,id");
                playlistVideosReq.PlaylistId = playlistID;
                playlistVideosReq.PageToken = nextPageToken;
                playlistVideosReq.MaxResults = 50;
                PlaylistItemListResponse playlistVideosRes = playlistVideosReq.Execute();

                // Finnaly grab the video IDs
                if (videos == null)
                    videos = new List<YouTubeVideo>();

                // Populate video list
                foreach (PlaylistItem videoItem in playlistVideosRes.Items)
                {
                    YouTubeVideo loadedVideo = new YouTubeVideo(videoItem.Snippet.Title, videoItem.Snippet.ResourceId.VideoId, videoItem.Snippet.Description, videoItem.Snippet.PublishedAt, videoItem.Snippet.Thumbnails.Default__.Url, videoItem.Snippet.ChannelTitle, videoItem.Snippet.Position);
                    videos.Add(loadedVideo);

                    if (OnVideoInformationLoaded != null)
                        OnVideoInformationLoaded(this, new VideoInformationLoadedEventArgs(loadedVideo));
                }

                // Check if we have more to grab
                if (!string.IsNullOrEmpty(playlistVideosRes.NextPageToken))
                    videos = LoadURLVideos(url, videos, playlistVideosRes.NextPageToken);

                if (OnAllVideoInformationLoaded != null)
                    OnAllVideoInformationLoaded(this, new AllVideoInformationLoadedEventArgs(videos));

                return videos;
            }
            else if (url.Contains("watch?v="))
            {
                string watchID = Regex.Split(Regex.Split(url, @"v=")[1], "&")[0];

                VideosResource.ListRequest videoListReq = YouTubeService.Videos.List("snippet, id");
                videoListReq.Id = watchID;
                VideoListResponse videoListResp = videoListReq.Execute();

                if (videos == null)
                    videos = new List<YouTubeVideo>();

                Video videoItem = videoListResp.Items[0];
                YouTubeVideo loadedVideo = new YouTubeVideo(videoItem.Snippet.Title, videoItem.Id, videoItem.Snippet.Description, videoItem.Snippet.PublishedAt, videoItem.Snippet.Thumbnails.Default__.Url, videoItem.Snippet.ChannelTitle, 0);
                videos.Add(loadedVideo);

                if (OnVideoInformationLoaded != null)
                    OnVideoInformationLoaded(this, new VideoInformationLoadedEventArgs(loadedVideo));

                if (OnAllVideoInformationLoaded != null)
                    OnAllVideoInformationLoaded(this, new AllVideoInformationLoadedEventArgs(videos));

                return videos;
            }
            else
            {
                throw new ArgumentException("The URL specified is not a valid youtube playlist or watch url");
            }
        }

        public void ResetEvents()
        {
            OnVideoInformationLoaded = null;
            OnAllVideoInformationLoaded = null;
        }
    }
}