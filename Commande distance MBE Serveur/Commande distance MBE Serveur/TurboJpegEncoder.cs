using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Commande_distance_MBE_Serveur
{
    public static class TurboJpegEncoder
    {
        private const int TJPF_BGRA = 8;
        private const int TJSAMP_420 = 2;
        private const int TJFLAG_FASTDCT = 2048;

        [DllImport("turbojpeg.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr tjInitCompress();

        [DllImport("turbojpeg.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int tjDestroy(IntPtr handle);

        [DllImport("turbojpeg.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int tjCompress2(
            IntPtr handle,
            IntPtr srcBuf,
            int width,
            int pitch,
            int height,
            int pixelFormat,
            ref IntPtr jpegBuf,
            ref uint jpegSize,
            int jpegSubsamp,
            int jpegQual,
            int flags);

        [DllImport("turbojpeg.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int tjFree(IntPtr buffer);

        private static readonly IntPtr _handle = tjInitCompress();

        public static byte[] Encode(Bitmap bmp, int quality)
        {
            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                IntPtr jpegBuf = IntPtr.Zero;
                uint jpegSize = 0;

                int result = tjCompress2(
                    _handle,
                    data.Scan0,
                    bmp.Width,
                    data.Stride,
                    bmp.Height,
                    TJPF_BGRA,
                    ref jpegBuf,
                    ref jpegSize,
                    TJSAMP_420,
                    quality,
                    TJFLAG_FASTDCT);

                if (result != 0)
                    throw new Exception("tjCompress2 failed");

                byte[] jpeg = new byte[jpegSize];
                Marshal.Copy(jpegBuf, jpeg, 0, (int)jpegSize);

                tjFree(jpegBuf);

                return jpeg;
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }
    }
}
