using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SYMM_Remote.Commands
{
    public class LoadByChannelName : Command
    {
        public string ChannelName { get; set; }

        public override void Run()
        {
            // Display information for one video when all information is loaded
            APIHandler.OnVideoInformationLoaded += (s, e) =>
            {
                
            };

            new Thread(() =>
            {
                // Start actual work on backend
                APIHandler.LoadVideosFromChannelNonBlocking(ChannelName);
            }).Start();
        }
    }
}
