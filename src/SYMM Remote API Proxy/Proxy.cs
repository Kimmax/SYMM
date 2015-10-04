using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SYMM_Remote
{
    class Proxy
    {
        private List<APIClient> clients = new List<APIClient>();
        private TcpListener server;
        private IPAddress _serverAdress;
        private ushort _serverPort;
        private bool running = false;

        public ushort ServerPort
        {
            get { return _serverPort; }
            private set { _serverPort = value; }
        }

        public IPAddress ServerAdress
        {
            get;
            set;
        }

        public Proxy(IPAddress serverAdress, ushort port = 3110)
        {
            this.ServerAdress = serverAdress;
            this.ServerPort = port;
        }

        public void Run()
        {
            try
            {
                server = new TcpListener(this.ServerAdress, this.ServerPort);
                server.Start();
                running = true;

                while(running)
                {
                    if(server.Pending())
                    {
                        AcceptClient(server.AcceptTcpClient());
                    }

                    Thread.Sleep(500);
                }

                server.Stop();
            }
            catch
            {
                // ToDo: Handle exceptions
                throw;
            }
        }

        private void AcceptClient(TcpClient client)
        {
            APIClient apiClient = new APIClient(client);
            clients.Add(apiClient);
            apiClient.Run();
        }

        public void Close()
        {
            running = false;

            foreach(APIClient client in clients)
            {
                client.Close();
            }
        }
    }
}
