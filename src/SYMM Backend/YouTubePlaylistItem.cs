using System;

namespace SYMM_Backend
{
    class YouTubePlaylistItem
    {

        private String _publishedAt;
        public String PublishedAt
        {
            get { return _publishedAt; }
            set { _publishedAt = value; }
        }
        
        private String _channelId;
        public String ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }

        private String _title;
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private String _description;
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private String _thumbDefault;
        public String ThumbDefault
        {
            get { return _thumbDefault; }
            set { _thumbDefault = value; }
        }

        private String _thumbMedium;
        public String ThumbMedium
        {
            get { return _thumbMedium; }
            set { _thumbMedium = value; }
        }

        private String _thumbHigh;
        public String ThumbHigh
        {
            get { return _thumbHigh; }
            set { _thumbHigh = value; }
        }

        private String _thumbStandard;
        public String ThumbStandard
        {
            get { return _thumbStandard; }
            set { _thumbStandard = value; }
        }

        private String _channelTitle;
        public String ChannelTitle
        {
            get { return _channelTitle; }
            set { _channelTitle = value; }
        }

        private String _playlistId;
        public String PlaylistId
        {
            get { return _playlistId; }
            set { _playlistId = value; }
        }

        private String _position;
        public String Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public YouTubePlaylistItem()
        {

        }
    }
}
