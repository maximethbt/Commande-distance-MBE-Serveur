using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Commande_distance_MBE_Serveur
{
    internal class MBEServer
    {

        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        public MBEServer(int Port)
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("Server started on port " + Port);
            Console.WriteLine("Waiting for connection");
            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            Console.WriteLine("Connected client");
        }
    }
}
