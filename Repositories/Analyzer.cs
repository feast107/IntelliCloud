using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Baidu.Aip.Ocr;
using IntelliHttpClient;
using ModelLib;
using MyOpenCv.API;
using Newtonsoft.Json.Linq;
using OpenCvSharp.Cuda;
using Repositories.API;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
using Stream = System.IO.Stream;
namespace Repositories
{
    public class Analyzer : IAnalyzer
    {
        private const string API_KEY = "Bu2kIAeNz7D57lzu3XUCNiS9";
        private const string SECRET_KEY = "vLdTvzZUxHGlU66XX2yIA2xn1U0z4biN";
        private readonly Ocr client;
        private readonly IImageFilter _filter1;
        private readonly I2ImageFilter _filter2;
        private readonly Dictionary<int, Dictionary<Bitmap, string>> Results = new Dictionary<int, Dictionary<Bitmap, string>>();
        public Analyzer(IImageFilter filter1, I2ImageFilter filter2)
        {
            _filter1 = filter1;
            _filter2 = filter2;
            client = new Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;
        }

        private static JObject GeneralBasicDemo(Ocr client, byte[] image)
        {
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            // 如果有可选参数
            var options = new Dictionary<string, object>
            {
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "true"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别, 图片参数为本地图片
            return client.Form(image, options);
        }

        public string ImageToResult(int id, Stream  image)
        {
            var pic = Image.FromStream(image);
            pic = 图像工序((Bitmap)pic);
            foreach(var img in GetImages(id))
            {
                if(_filter2.CompareImage((Bitmap)pic, img.Key)[0]>0.8)
                {
                    return img.Value;
                }
            }
            GetImages(id).TryAdd((Bitmap)pic, "");
            var res = GeneralBasicDemo(client, StreamToByte(image)).ToString();
            GetImages(id)[(Bitmap)pic]=res;
            return res;
        }
         
        private Bitmap 图像工序(Bitmap source)
        {
            return _filter1.Rectify(_filter1.Graying(_filter1.Binaryzation(source)));
        }

        private static byte[] StreamToByte(Stream stream)
        {
            /*Image img = Image.FromStream((MemoryStream)stream);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] BPicture = ms.GetBuffer();*/

            byte[] BPicture = ((MemoryStream)stream).GetBuffer();
            return BPicture;
        }

        public Task<string> ImageToResultAsync(int id, Stream image)
        {
            return Task.Run(() => { return ImageToResult(id, image); });
        }

        private Dictionary<Bitmap,string> GetImages(int id)
        {
            if (Results.TryGetValue(id , out var values))
            {
                return values;
            }
            else
            {
                Dictionary<Bitmap,string> result = new Dictionary<Bitmap,string>();
                Results.TryAdd(id, result);
                return result;
            }
        }

        public int 查重(PictureSearch targ, PictureSearch[] srcs)
        {
            foreach(var s in srcs)
            {
                if (_filter2.CompareImage((MemoryStream)targ._img, (MemoryStream)s._img)[0] > 0.8)
                {
                    return s.编号;
                }
            }
            return -1;
        }

        public Task 识别(PictureSearch search)
{
            return Task.Run(()=> search.JSON = GeneralBasicDemo(client, StreamToByte(search._img)).ToString());
        }
    }
}
