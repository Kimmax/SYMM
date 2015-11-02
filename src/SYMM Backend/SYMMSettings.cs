using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SYMM_Backend
{
    public static class SYMMFileFormats
    {
        public enum AudioFormats
        {
            mp3,
            wav,
            m4a,
            wma,
            ogg
        }
    }

    public class SYMMSettings
    {
        public string PathSafefileName { get; set; }
        public bool CheckDuplicate { get; private set; }
        public string DownloadURL { get; private set; }
        public string AlbumNameRegex { get; private set; }
        public short VideoResolution { get; private set; }
        public short AudioBitrate { get; private set; }
        public bool ExtractAudio { get; private set; }
        public string SavePath { get; private set; }
        public SYMMFileFormats.AudioFormats AudioFormat { get; private set; }

        public SYMMSettings(string downloadURL, string savePath, short videoResolution = 1080, bool checkDuplicate = true)
        {
            this.DownloadURL = downloadURL;
            this.SavePath = savePath;
            this.VideoResolution = videoResolution;
            this.CheckDuplicate = checkDuplicate;
            this.ExtractAudio = false;
        }

        public SYMMSettings(string downloadURL, string savePath,  bool extractAudio, bool checkDuplicate = true, SYMMFileFormats.AudioFormats audioFormat = SYMMFileFormats.AudioFormats.mp3, short audioBitrate = 192)
        {
            this.DownloadURL = downloadURL;
            this.SavePath = savePath;
            this.CheckDuplicate = checkDuplicate;
            this.ExtractAudio = extractAudio;
            this.AudioFormat = audioFormat;
            this.AudioBitrate = audioBitrate;
        }
    }
}
