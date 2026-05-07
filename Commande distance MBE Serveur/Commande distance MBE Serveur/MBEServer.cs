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
using System.Diagnostics;

namespace Commande_distance_MBE_Serveur
{
    internal class MBEServer
    {

        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private static readonly ImageCodecInfo JpegCodec = GetJpegCodec();
        private static readonly EncoderParameters JpegParams = CreateJpegParams(50L);


        private static ImageCodecInfo GetJpegCodec()
        {
            foreach (ImageCodecInfo c in ImageCodecInfo.GetImageEncoders())
                if (c.MimeType == "image/jpeg") return c;
            throw new Exception("JPEG codec not found");
        }

        private static EncoderParameters CreateJpegParams(long quality)
        {
            EncoderParameters p = new EncoderParameters(1);
            p.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            return p;
        }
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
            byte[] jpegBytes = TurboJpegEncoder.Encode(image, 50);
            return SendImage(jpegBytes);
            
        }

        public void Stop()
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (listener != null) listener.Stop();
        }
    }
}
