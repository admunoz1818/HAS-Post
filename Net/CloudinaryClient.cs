using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HAS_Post.Net
{
    public class CloudinaryClient
    {
        public interface ICloudinary
        {

            void urlImg(String urlI);
        }

        private String url;
        private RestClient client;
        ICloudinary iCloudinary;

        public CloudinaryClient(ICloudinary iCloudinary, String url)
        {

            this.url = url;
            client = new RestClient(url);
            this.iCloudinary = iCloudinary;
        }

        public void updateImage(BitmapImage image, String name)
        {
            byte[] data = bitmapImageToBytes(image);

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan ts = (DateTime.Now - dt);
            var request = new RestRequest(Method.POST);
            request.AddParameter("public_id", name);
            request.AddParameter("api_key", "646747527977466");
            request.AddParameter("timestamp", "" + ts.TotalSeconds);
            String sha = "public_id=" + name + "&timestamp=" + ts.TotalSeconds + "XdVUGnNaxd1En1TlPf9ocJ14WZM";

            request.AddParameter("signature", calculteSHA1(sha));
            request.AddFile("file", data, name + ".jpg", "image/jpeg");
            client.ExecuteAsync(request, response =>
            {
                JObject obj = JObject.Parse(response.Content);
                String urlImg = (String)obj["url"];
                iCloudinary.urlImg(urlImg);
            });
        }

        private byte[] bitmapImageToBytes(BitmapImage image)
        {

            using (MemoryStream stream = new MemoryStream())
            {
                WriteableBitmap wBitmap = new WriteableBitmap(image);
                wBitmap.SaveJpeg(stream, 420, 580, 0, 80);
                return stream.ToArray();
            }
        }


        private String calculteSHA1(String text)
        {
            SHA1Managed s = new SHA1Managed();
            UTF8Encoding enc = new UTF8Encoding();
            s.ComputeHash(enc.GetBytes(text.ToCharArray()));

            return BitConverter.ToString(s.Hash).Replace("-", "");
        }
    }
}
