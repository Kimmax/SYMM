using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using CMD = SYMM_Remote.Commands;
using SYMM_Backend;

namespace SYMM_Remote
{
    class APIClient
    {
        private SYMMHandler APIHandler;
        private TcpClient client;
        private StreamReader sReader;
        private StreamWriter sWriter;

        private bool running = false;

        public APIClient(TcpClient client, SYMMHandler APIHandler)
        {
            this.client = client;
            this.sReader = new StreamReader(client.GetStream());
            this.sWriter = new StreamWriter(client.GetStream());
            this.APIHandler = APIHandler;
        }

        public void Run()
        {
            new Thread(() =>
            {
                while(running)
                {
                    if(sReader.Peek() > -1)
                    {
                        CMD.LoadByChannelName chanLoadCommand = new CMD.LoadByChannelName();
                        chanLoadCommand.ChannelName = "OfficialTrapCity";
                    }
                }
            }).Start();
        }

        public void Close()
        {
            if (sReader != null)
                sReader.Close();

            if (sWriter != null)
                sWriter.Close();

            if (client.Connected)
                client.Close();
        }
    }
}
