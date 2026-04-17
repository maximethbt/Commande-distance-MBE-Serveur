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
            while(true)
            {
                int requete = Server.ReadRequest();
                if (requete == -1) break;

                switch (requete)
                {
                    case 0x01:  // Screenshot
                        image = CaptureMBE.Capture(1);
                        Server.SendImage(image);
                        break;
                }
            }

            Server.Stop();
            Console.WriteLine("Server stopped.");
            Console.ReadKey();
        }
    }
}
