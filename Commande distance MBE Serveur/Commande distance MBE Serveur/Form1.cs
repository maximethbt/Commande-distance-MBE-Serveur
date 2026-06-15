using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Commande_distance_MBE_Serveur
{
    public partial class Form1 : Form
    {
        Thread ServerThread;
        volatile bool running = true;

        readonly object fileLock = new object();
        string pendingFilePath = null;

        public Form1()
        {
            InitializeComponent();
            ServerThread = new Thread(ServerLoop);
            ServerThread.IsBackground = true;
            ServerThread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void ServerLoop()
        {
            MBEServer Server = new MBEServer(9000);
            string Message;
            Bitmap image;
            int offsetX = 0;
            int offsetY = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            if (!Server.Start())
            {
                Log("Server failed to start");
                return;
            }
            Log("Server successfully started");

            while (true)
            {
                Log("Waiting for connection");
                Server.WaitForClient();
                Log("Connected Client");
                Server.SetReadTimeout(5000);
                while (running)
                {
                    try
                    {
                        int requete = Server.ReadRequest();
                        if (requete == -1) break;

                        switch (requete)
                        {
                            case 0x01:  // Screenshot
                                int screen = Server.ReadInt32();
                                offsetX = Screen.AllScreens[screen - 1].Bounds.X;
                                offsetY = Screen.AllScreens[screen - 1].Bounds.Y;
                                ThreadPool.QueueUserWorkItem(_ => {
                                    var img = CaptureMBE.Capture(screen);
                                    Server.SendImage(img);
                                    img.Dispose();
                                });
                                break;

                            case 0x02:
                                int x = Server.ReadInt32();
                                int y = Server.ReadInt32();
                                Log($"Mouse {x},{y} at {DateTime.Now:HH:mm:ss.fff}");
                                Cursor.Position = new Point(x + offsetX, y + offsetY);
                                break;

                            case 0x03:  // Le client demande s'il y a un fichier
                                string fichier;
                                lock (fileLock) { fichier = pendingFilePath; pendingFilePath = null; }
                                if (fichier != null && File.Exists(fichier))
                                {
                                    Server.SendFile(fichier);
                                    Log("Fichier envoyé : " + Path.GetFileName(fichier));
                                }
                                else
                                    Server.SendNoFile();
                                break;

                            case 0x10: MakeInputs.LeftDown(); break;
                            case 0x20: MakeInputs.LeftUp(); break;
                            case 0x11: MakeInputs.RightDown(); break;
                            case 0x21: MakeInputs.RightUp(); break;
                            case 0x30:
                                int KeyDown = Server.ReadInt32();
                                MakeInputs.KeyDown(KeyDown);
                                break;
                            case 0x40:
                                int KeyUp = Server.ReadInt32();
                                MakeInputs.KeyUp(KeyUp);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                Server.DisconnectClient();
                Log("Current client has disconnected");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            running = false;
            base.OnFormClosing(e);
        }

        private void textBox_Logs_TextChanged(object sender, EventArgs e)
        {

        }

        // === Bouton "Envoyer un fichier" (à câbler dans le designer) ===
        private void button_EnvoyerFichier_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Choisir un fichier à envoyer";
                dlg.Filter = "Tous les fichiers (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    lock (fileLock) { pendingFilePath = dlg.FileName; }
                    Log("Fichier en attente : " + Path.GetFileName(dlg.FileName));
                }
            }
        }

        void Log(string message)
        {
            if (textBox_Logs.InvokeRequired)
            {
                textBox_Logs.BeginInvoke(new Action<string>(Log), message);
                return;
            }
            textBox_Logs.AppendText(string.Format("[{0:HH:mm:ss}] {1}{2}",
                DateTime.Now, message, Environment.NewLine));
        }
    }
}