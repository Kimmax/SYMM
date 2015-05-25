using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace SYMM_Backend
{
    public class YouTubeVideo
    {
        string videoTitle;
        public string VideoTitle
        {
            get { return videoTitle; }
            private set { videoTitle = value; }
        }

        string videoWatchID;
        public string VideoWatchID
        {
            get { return videoWatchID; }
            private set { videoWatchID = value; }
        }

        string desc;
        public string Desc
        {
            get { return desc; }
            private set { desc = value; }
        }

        DateTime? publishDate;
        public DateTime? PublishDate
        {
            get { return publishDate; }
            private set { publishDate = value; }
        }

        string thumbURL;
        public string ThumbURL
        {
            get { return thumbURL; }
            private set { thumbURL = value; }
        }

        string channelTitle;
        public string ChannelTitle
        {
            get { return channelTitle; }
            private set { channelTitle = value; }
        }

        long? playListPos;
        public long? PlayListPos
        {
            get { return playListPos; }
            private set { playListPos = value; }
        }

        public YouTubeVideo(string videoTitle, string videoWatchID, string desc, DateTime? publishDate, string thumbURL, string channelTitle, long? playListPos)
        {
            this.VideoTitle = videoTitle;
            this.VideoWatchID = videoWatchID;
            this.Desc = desc;
            this.PublishDate = publishDate;
            this.ThumbURL = thumbURL;
            this.ChannelTitle = channelTitle;
            this.PlayListPos = playListPos;
        }

        public string GetWatchID()
        {
            return this.VideoWatchID;
        }

        public Bitmap GetThumbnail()
        {
            WebRequest request = WebRequest.Create(this.ThumbURL);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            return new Bitmap(responseStream);
        }
    }
}
