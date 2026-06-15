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
using System.Threading;

namespace Commande_distance_MBE_Serveur
{
    internal class MBEServer
    {
        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private volatile bool clientActive = false;

        public MBEServer(int Port)
        {
            listener = new TcpListener(IPAddress.Any, Port);
        }

        public bool Start()
        {
            try
            {
                listener.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void WaitForClient()
        {
            client = listener.AcceptTcpClient();
            client.NoDelay = true;
            stream = client.GetStream();
            clientActive = true;
            StartRejector();
        }

        public void DisconnectClient()
        {
            if (stream != null) { stream.Close(); stream = null; }
            if (client != null) { client.Close(); client = null; }
            clientActive = false;
        }

        private void StartRejector()
        {
            Thread rejectorThread = new Thread(() =>
            {
                while (clientActive)
                {
                    try
                    {
                        TcpClient intruder = listener.AcceptTcpClient();
                        Console.WriteLine("Refused extra client");
                        intruder.Close();
                    }
                    catch
                    {
                        break;
                    }
                }
            });
            rejectorThread.IsBackground = true;
            rejectorThread.Start();
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

        public void SetReadTimeout(int milliseconds)
        {
            if (stream != null)
                stream.ReadTimeout = milliseconds;
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

        // === Transfert de fichier serveur -> client ===
        public bool SendFile(string path)
        {
            try
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(Path.GetFileName(path));
                byte[] fileBytes = File.ReadAllBytes(path);

                stream.WriteByte(1);                                        // 1 = un fichier suit
                stream.Write(BitConverter.GetBytes(nameBytes.Length), 0, 4);
                stream.Write(nameBytes, 0, nameBytes.Length);
                stream.Write(BitConverter.GetBytes(fileBytes.Length), 0, 4);
                stream.Write(fileBytes, 0, fileBytes.Length);
                return true;
            }
            catch { return false; }
        }

        public void SendNoFile()
        {
            try { stream.WriteByte(0); } catch { }                          // 0 = rien
        }

        private byte[] ReadExact(int n)
        {
            byte[] buf = new byte[n];
            int total = 0;
            while (total < n)
            {
                int r = stream.Read(buf, total, n - total);
                if (r <= 0) throw new Exception("Disconnected");
                total += r;
            }
            return buf;
        }

        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadExact(4), 0);
        }
    }
}