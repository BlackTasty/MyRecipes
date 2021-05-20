
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Media.Imaging;

namespace MyRecipes.Core
{
    static class Utils
    {

        public static BitmapImage FileToBitmapImage(string path, Size? desiredSize = null)
        {
            if (File.Exists(path))
            {
                BitmapImage bitmap;
                if (desiredSize == null)
                {
                    bitmap = BitmapToBitmapImage(new Bitmap(path));
                }
                else
                {
                    using (Bitmap bmp = new Bitmap(path))
                    {
                        int diffWidth = bmp.Width - desiredSize.Value.Width;
                        int diffHeight = bmp.Height - desiredSize.Value.Height;

                        if (diffHeight > 0 && diffWidth > 0)
                        {
                            bitmap = BitmapToBitmapImage(new Bitmap(bmp, new Size(bmp.Width - diffWidth, bmp.Height - diffHeight)));
                        }
                        else
                            bitmap = BitmapToBitmapImage(bmp);
                    }
                }
                bitmap?.Freeze();
                return bitmap;
            }
            else
                return null;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

                    stream.Position = 0;
                    BitmapImage result = new BitmapImage();
                    result.BeginInit();
                    // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                    // Force the bitmap to load right now so we can dispose the stream.
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    //result.CacheOption = forceLoad ? BitmapCacheOption.OnLoad : BitmapCacheOption.OnDemand;
                    result.StreamSource = stream;
                    result.EndInit();
                    result.Freeze();
                    return result;
                }
            }
            else
                return null;
        }

        public static string GetLocalIPAddress()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                Dns.GetHostAddresses(Dns.GetHostName());
            }

            return "-";
        }

        public static List<string> GetAllLocalIPAddresses()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                List<string> addresses = new List<string>();
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        addresses.Add(ip.ToString());
                    }
                }

                return addresses;
            }

            return new List<string>() { "-" };
        }
    }
}
