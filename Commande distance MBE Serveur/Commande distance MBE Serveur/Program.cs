using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                Console.WriteLine("Waiting for connection");
                Server.WaitForClient();
                Console.WriteLine("Connected Client");
                while (true)
                {
                    int requete = Server.ReadRequest();
                    if (requete == -1) break;

                    switch (requete)    
                    {
                        case 0x01:  // Screenshot
                            ThreadPool.QueueUserWorkItem(_ => {
                                var img = CaptureMBE.Capture(1);
                                Server.SendImage(img);
                                img.Dispose();
                            });
                            break;  

                        case 0x02:
                            int x = Server.ReadInt32();
                            int y = Server.ReadInt32();
                            Console.WriteLine($"Mouse {x},{y} at {DateTime.Now:HH:mm:ss.fff}");
                            Cursor.Position = new Point(x, y);
                            break;

                    }
                }

                Server.DisconnectClient();
                Console.WriteLine("Current client has disconnected");
            }
        }
    }
}
