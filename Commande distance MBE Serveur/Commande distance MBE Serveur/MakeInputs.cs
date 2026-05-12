using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Commande_distance_MBE_Serveur
{
    static class MakeInputs
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
        public static void LeftDown() { mouse_event(0x0002, 0, 0, 0, 0); }
        public static void LeftUp() { mouse_event(0x0004, 0, 0, 0, 0); }
        public static void RightDown() { mouse_event(0x0008, 0, 0, 0, 0); }
        public static void RightUp() { mouse_event(0x0010, 0, 0, 0, 0); }
        public static void MiddleDown() { mouse_event(0x0020, 0, 0, 0, 0); }
        public static void MiddleUp() { mouse_event(0x0040, 0, 0, 0, 0); }


        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        public static void KeyDown(int keyCode)
        {
            keybd_event((byte)keyCode, 0, 0x0000, 0);
        }

        public static void KeyUp(int keyCode)
        {
            keybd_event((byte)keyCode, 0, 0x0002, 0);
        }

    }
}
