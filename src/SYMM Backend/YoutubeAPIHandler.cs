using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SYMM_Backend
{
    class YoutubeAPIHandler
    {
        private readonly string _APIKey;
        public string APIKey
        {
            get { return _APIKey; }
        }

        private const string YoutubeApiBaseURL = "https://www.googleapis.com/youtube/v3/";
        private const string PlaylistAPIRequestURL = YoutubeApiBaseURL + "playlistItems?part=snippet&playlistId={1}&maxResults={0}&key={2}";
        private const string VideoItemAPIRequestURL = YoutubeApiBaseURL + "videos?part=snippet&id={0}&maxResults={1}&key={2}";

        private WebClient wClient = new WebClient();

        public YoutubeAPIHandler(string APIKey)
        {
            this._APIKey = APIKey;
        }

        private string FormPlayListRequest(string playlistID, int maxresult = 50, string nextPageToken = "")
        {
            if (String.IsNullOrEmpty(nextPageToken))
            {
                return String.Format(PlaylistAPIRequestURL, playlistID, maxresult, this.APIKey);
            }
            else
            {
                return String.Format(PlaylistAPIRequestURL, playlistID, maxresult, this.APIKey) + "&pageToken=" + nextPageToken;
            }
        }

        private string FormVideoDetailRequest(string videoIDs, int maxresult = 50, string nextPageToken = "")
        {
            if (String.IsNullOrEmpty(nextPageToken))
            {
                return String.Format(PlaylistAPIRequestURL, Regex.Replace(videoIDs, ",", "%2"), maxresult, this.APIKey);
            }
            else
            {
                return String.Format(PlaylistAPIRequestURL, Regex.Replace(videoIDs, ",", "%2"), maxresult, this.APIKey) + "&pageToken=" + nextPageToken;
            }
        }
    }
}
