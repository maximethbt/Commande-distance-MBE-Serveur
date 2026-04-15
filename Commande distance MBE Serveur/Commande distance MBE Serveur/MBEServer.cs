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

        public string ReadMessage()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
            }
            catch
            {
                return null;
            }
            if (bytesRead == 0) return null;
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        public void Respond(string Message)
        {
            byte[] data = Encoding.UTF8.GetBytes(Message);
            stream.Write(data, 0, data.Length);
        }

        public void Stop()
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (listener != null) listener.Stop();
        }
    }
}
