using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Commande_distance_MBE_Serveur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MBEServer Server = new MBEServer(9000);
            string Message;
            Bitmap image;
            int i = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            if (!Server.Start())
            {
                Console.WriteLine("Server failed to start");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Server successfully started");


            while (true)
            {
                Console.WriteLine("Waiting ffor connection");
                Server.WaitForClient();
                Console.WriteLine("Connected Client");
                while (true)
                {
                    sw.Reset();
                    i++;
                    int requete = Server.ReadRequest();
                    if (requete == -1) break;

                    switch (requete)
                    {
                        case 0x01:  // Screenshot
                            sw.Start();
                            image = CaptureMBE.Capture(1);
                            sw.Stop();
                            long cap = sw.ElapsedMilliseconds;
                            sw.Reset();
                            sw.Start();
                            Server.SendImage(image);
                            sw.Stop();
                            long env = sw.ElapsedMilliseconds;
                            image.Dispose();
                            Console.WriteLine(i + "capture = " + cap + " envoi = " + env);
                            break;

                    }
                }

                Server.DisconnectClient();
                Console.WriteLine("Current client has disconnected");
            }
        }
    }
}
