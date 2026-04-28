using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Encoder = System.Drawing.Imaging.Encoder;

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

        public int ReadRequest()
        {
            try
            {
                return stream.ReadByte();  // Retourne -1 si déconnecté
            }
            catch
            {
                return -1;
            }
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

        public bool SendImage(byte[] imageBytes)
        {
            try
            {
                byte[] taille = BitConverter.GetBytes(imageBytes.Length);
                stream.Write(taille, 0, 4);
                stream.Write(imageBytes, 0, imageBytes.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendImage(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();

            // Trouve le codec JPEG
            ImageCodecInfo jpegCodec = null;
            foreach (ImageCodecInfo c in ImageCodecInfo.GetImageEncoders())
                if (c.MimeType == "image/jpeg") { jpegCodec = c; break; }

            // Qualité 50%
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 50L);

            image.Save(ms, jpegCodec, parameters);
            byte[] imageBytes = ms.ToArray();
            ms.Dispose();
            return SendImage(imageBytes);
        }
        public void Stop()
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (listener != null) listener.Stop();
        }
    }
}
