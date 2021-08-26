using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace gecis
{
    class Program
    {
        public struct Color64
        {
            public ushort b;
            public ushort g;
            public ushort r;
            public ushort a;
        }
        public static Bitmap pixelsToBitmap(Color64[,] pixels)
        {
            try
            {
                var width = pixels.GetLength(1);
                var height = pixels.GetLength(0);
                var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                var bitmap = new Bitmap(width, height, width * 8, PixelFormat.Format64bppArgb, handle.AddrOfPinnedObject());

                return bitmap;
            }
            catch (System.Exception e) { }
            return null;
        }
        static void Main(string[] args)
        {
            #region ŞERİTLERİ HESAPLA
            int maxBit = 2147450880;
            var a = new int[255];
            var r = 1.07703508348;
            for (byte i = 0; i < 255; i++)
                a[i] = (int)Math.Pow(r, i);
            #endregion
            #region BİTLERİ OLUŞTUR
            var k = 0;
            byte t = 1;
            var b = new byte[maxBit];
            for (byte i = 0; i < 255; i++)
            {
                for (var j = 0; j < a[i]; j++)
                    b[k++] = t;

                t = (byte)(1 - t);
            }
            #endregion            
            #region TONLARI HESAPLA
            var c = new ushort[32768];
            for (var i = 0; i < maxBit; i++)
                c[i / 65535] += b[i];            
            #endregion 
            #region 64bit IMAGE OLARAK KAYDET
            var pixels = new Color64[8192, 32768];
            Parallel.For(0, 32768, i =>
            {
                for (var j = 0; j < 8192; j++)
                {
                    pixels[j, i].r = pixels[j, i].g = pixels[j, i].b = c[i];
                    pixels[j, i].a = 65535;
                }
            });
            var bitmap = pixelsToBitmap(pixels);
            bitmap.Save("1.png");            
            #endregion
        }
    }
}
