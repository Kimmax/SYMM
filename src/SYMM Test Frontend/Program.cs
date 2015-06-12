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

        public void Run(string APIKey)
        {
            backend = new SYMMHandler(APIKey);
            List<YouTubeVideo> videoList = backend.LoadVideosFromChannel("OfficialTrapCity");

            bool done1 = false;

            Console.WriteLine("Download first video and extract the audio.");
            new Thread(() =>
            {
                VideoDownloader downloader = new VideoDownloader();

                int lastPrgs = -1;

                downloader.DownloadProgressChanged += (sender, args) =>
                {
                    if (lastPrgs != (int)args.ProgressPercentage)
                    {
                        Console.WriteLine("Downloading " + videoList[videoList.Count -1].VideoTitle + " is " + (int)args.ProgressPercentage + "% done");
                        lastPrgs = (int)args.ProgressPercentage;
                    }
                };

                lastPrgs = -1;

                downloader.AudioExtractionProgressChanged += (sender, args) =>
                {
                    if (lastPrgs != (int)args.ProgressPercentage)
                    {
                        Console.WriteLine("Extracting audio for " + videoList[videoList.Count - 1].VideoTitle + " is " + (int)args.ProgressPercentage + "% done");
                       lastPrgs = (int)args.ProgressPercentage;
                    }
                };

                downloader.DownloadVideo(videoList[videoList.Count - 1].VideoWatchID, @"C:\Users\Kim\Desktop");
                done1 = true;
            }).Start();

            while(!done1)
            {
                Thread.Sleep(300);
            }

            Console.WriteLine("\n--------------------------------------------------------------\n");
            Console.WriteLine("Downloading and extraction finshed!");

        }
    }
}
