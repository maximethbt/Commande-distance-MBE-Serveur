using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Capture_ecran_MBE
{
    static class CaptureMBE
    {
        private static Rectangle bounds;
        private static Bitmap screenshot;
        private static Graphics g;

        public static Bitmap Capture(int ScreenNum)
        {
            bounds = Screen.AllScreens[ScreenNum-1].Bounds;
            screenshot = new Bitmap(bounds.Width, bounds.Height);
            g = Graphics.FromImage(screenshot);
            g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
            return new Bitmap(screenshot);
            g.Dispose();
        }
        

    }
}
