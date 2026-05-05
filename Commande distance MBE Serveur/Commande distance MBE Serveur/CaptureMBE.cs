using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Commande_distance_MBE_Serveur
{
    static class CaptureMBE
    {
        private static Rectangle bounds;
        private static Bitmap screenshot;
        private static Graphics g;

        public static Bitmap Capture(int ScreenNum)
        {
            Rectangle bounds = Screen.AllScreens[ScreenNum - 1].Bounds;

            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
            }
            return screenshot;
            screenshot.Dispose();

        }
        

    }
}
