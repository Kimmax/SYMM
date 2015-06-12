using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYMM_Backend;
using System.Threading;

namespace SYMM_Test_Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            Base myBase = new Base();
            myBase.Run("AIzaSyAj82IqIloWupFnhn-hmmUo7iAkcj2xk3g");
            Console.WriteLine("\n--------------------------------------------------------------\n");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }

    class Base
    {
        SYMMHandler backend;
        bool exit = false;

        private int work = 0;
        private string APIKey;

        public void Run(string APIKey)
        {
            this.APIKey = APIKey;

            DoWork();

            while(!exit)
            {
                Thread.Sleep(300);
            }
        }

        public void DoWork()
        {
            backend = new SYMMHandler(this.APIKey);
            List<YouTubeVideo> videoList = backend.LoadVideosFromChannel("OfficialTrapCity");

            Console.WriteLine("Download first video and extract the audio.");

            VideoDownloader downloader = new VideoDownloader(videoList[videoList.Count - 1]);
            work++;

            downloader.DownloadProgressChanged += (sender, args) =>
            {
                    Console.WriteLine("Downloading " + videoList[videoList.Count -1].VideoTitle + " is " + (int)args.ProgressPercentage + "% done");
            };

            downloader.AudioExtractionProgressChanged += (sender, args) =>
            {
                Console.WriteLine("Extracting audio for " + videoList[videoList.Count - 1].VideoTitle + " is " + (int)args.ProgressPercentage + "% done");
            };

            downloader.VideoDownloadComplete += (sender, args) =>
            {
                Console.WriteLine("\n--------------------------------------------------------------\n");
                Console.WriteLine("Downloading and extraction of Video '" + args.Video.VideoTitle + "' finished");
                
                work--;

                if (work == 0)
                    exit = true;
            };

            downloader.DownloadVideo(@"C:\Users\Kim\Desktop");
        }
    }
}
