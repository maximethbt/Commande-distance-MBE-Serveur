using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commande_distance_MBE_Serveur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MBEServer Server = new MBEServer(9000);
            string Message;

            while(true)
            {
                Message = Server.ReadMessage();
                if(Message == null)
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }

                Console.WriteLine("Received : " + Message);

                Server.Respond("Server has received : " + Message);
                Console.WriteLine("Response sent");
            }

            Server.Stop();
            Console.WriteLine("Server stopped.");
            Console.ReadKey();
        }
    }
}
