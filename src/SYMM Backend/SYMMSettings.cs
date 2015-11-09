using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SYMM_Backend
{
    public class SYMMSettings
    {
        public enum AudioFormats
        {
            mp3,
            wav,
            m4a,
            wma,
            ogg
        }

        public enum Actions
        {
            Download,
            ExtractAudio,
            Stream
        }

        public string PathSafefileName { get; set; }
        public bool CheckDuplicate { get; private set; }
        public string DownloadURL { get; private set; }
        public string AlbumNameRegex { get; private set; }
        public short VideoResolution { get; private set; }
        public short AudioBitrate { get; private set; }
        public string SavePath { get; private set; }
        public AudioFormats AudioFormat { get; private set; }
        public Actions Action { get; private set; }

        public SYMMSettings(string downloadURL, string savePath, short videoResolution = 1080, bool checkDuplicate = true)
        {
            this.DownloadURL = downloadURL;
            this.SavePath = savePath;
            this.VideoResolution = videoResolution;
            this.CheckDuplicate = checkDuplicate;
            this.Action = Actions.Download;
        }

        public SYMMSettings(string downloadURL, string savePath, Actions action, bool checkDuplicate = true, AudioFormats audioFormat = SYMMSettings.AudioFormats.mp3, short audioBitrate = 192)
        {
            this.DownloadURL = downloadURL;
            this.SavePath = savePath;
            this.Action = action;
            this.CheckDuplicate = checkDuplicate;
            this.AudioFormat = audioFormat;
            this.AudioBitrate = audioBitrate;
        }

        public SYMMSettings(string streamURL, Actions action, short audioBitrate = 192)
        {
            this.DownloadURL = streamURL;
            this.Action = action;
            this.AudioBitrate = audioBitrate;
        }
    }
}
