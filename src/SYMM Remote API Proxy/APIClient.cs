using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;

namespace SYMM_Remote
{
    class APIClient
    {
        private TcpClient client;
        private StreamReader sReader;
        private StreamWriter sWriter;

        private bool running = false;

        public APIClient(TcpClient client)
        {
            this.client = client;
            this.sReader = new StreamReader(client.GetStream());
            this.sWriter = new StreamWriter(client.GetStream());
        }

        public void Run()
        {
            new Thread(() =>
            {
                while(running)
                {
                    if(sReader.Peek() > -1)
                    {

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
