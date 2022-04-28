using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Tesseract;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using ModelLib;
using Point = System.Drawing.Point;
using Microsoft.Office.Interop.Word;
using System.Linq;
using IntelliHttpClient;

namespace Test
{
	public class Program
	{
        public const string PATH = @"H:\IntelliCloud\Test\Sample.jpg";

        private static string res=string.Empty;

        public static string[] packs = { "chi_sim", "chi_sim_vert", "chi_tra", "chi_tra_vert" };

        const string API_KEY = "Bu2kIAeNz7D57lzu3XUCNiS9";
        const string SECRET_KEY = "vLdTvzZUxHGlU66XX2yIA2xn1U0z4biN";

        private static void Test()
        {
            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            // bai.GeneralBasicUrlDemo(client);
            //图片URL地址调用
            var result = GeneralBasicDemo(client, PATH);
            WriteInFile(result,PATH);
        }

        static void Generate()
        {
            FileStream s = File.OpenRead(@"H:\IntelliCloud\Test\2021年11月26日23时39分33秒-Sample.txt");
            byte[] by = new byte[s.Length];
            s.Read(by);
            string targ = System.Text.Encoding.UTF8.GetString(by);
            //List<FormCell> cells = FormSheet.GetAllCells(ref targ);
            FormSheet form = FormSheet.GetFormResult(targ);
            FormCell[,] matr = form.GenerateMatrix();
            var o = Newtonsoft.Json.JsonConvert.DeserializeObject(targ);
            JObject jo = JObject.Parse(targ);
            var d = DocxUnit.NewInstance().创建Doc();
            Point rc = Measure(matr);
            var pack = 单元格整合(matr, rc);
            var t = d.添加表格(rc.X + 1, rc.Y + 1);
            t.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            t.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            for (int r = 0; r < rc.X + 1; r++)
            {
                for (int c = 0; c < rc.Y + 1; c++)
                {
                    string content = matr[r, c] != null ? matr[r, c].内容 : "";
                    string target = "";
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (i + 1 < content.Length && content[i] == '\\')
                        {
                            switch (content[i + 1])
                            {
                                case 'n':
                                    target += '\n';
                                    break;
                                case 'r':
                                    target += '\r';
                                    break;
                                case 'a':
                                    target += '\a';
                                    break;
                            }
                            i++;
                        }
                        else
                        {
                            target += content[i];
                        }
                    }
                    t.Cell(r + 1, c + 1).Range.Text = target;
                    t.Cell(r + 1, c + 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                }
            }
            foreach (var p in pack)
            {
                t.Cell(p.Value.X + 1, p.Value.Y + 1).Merge(t.Cell(p.Key.X + 1, p.Key.Y + 1));
            }
            d.保存Doc(@"H:\IntelliCloud\Test\Sample.docx");
            d.Dispose();
        }

        static void Main(string[] args)
        {
            //     IHttp http = new Http();

            /* http.Get("https://localhost:44344/api/Access/Regist?UserName=321&Password=312");
             System.Net.Http.MultipartFormDataContent content = new System.Net.Http.MultipartFormDataContent();
             content.Add(new System.Net.Http.ByteArrayContent(File.ReadAllBytes(@"H:\IntelliCloud\Test\1637822077(1).jpg")), "picture", "p");
             Console.WriteLine(content.ToString());
             http.Post("https://localhost:44344/api/Picture/Upload",content);
             */
       //     var s = http.Download("https://106.14.32.120:5000/favicon.ico");
      //      ((MemoryStream)s).CopyTo(File.Create(@"H:\IntelliCloud\Test\pic.jpg"));
      //      Console.WriteLine(s);
            //var v =GetRET();
            //v.Start();
            //Console.WriteLine("START");
            //v.Wait();
            //Console.WriteLine("OVER" +v.Result);


            //Image i = Image.FromFile(@"J:\IntelliCloud\Test\DEMO.jpg");
            //Image i2 = Image.FromFile(@"J:\IntelliCloud\Test\DEMO1.jpg");
            //var b = new MyOpenCv.API.Cv2ImageFilter().CompareImage((Bitmap)i,(Bitmap)i2);
            //Console.WriteLine(b);
        }
        public static Dictionary<Point, Point> 单元格整合(FormCell[,] form,Point p)
        {
            Dictionary<KeyValuePair<Point, Point>, bool> ret = new Dictionary<KeyValuePair<Point, Point>, bool>();
            for(int r = p.X; r >= 0; r--)
            {
                for(int c = p.Y; c >= 0; c--)
                {
                    if(form[r,c]?.形态 == FormCell.Formation.右增生|| form[r, c]?.形态 == FormCell.Formation.下增生)
                    {
                        Point pt = new Point(r, c);
                        foreach (var pair in ret)
                        {
                            if (pair.Key.Key.X >= r && pair.Key.Key.Y >= c && pair.Key.Value.X <= r && pair.Key.Value.Y <= c)
                            {
                                c = pair.Key.Value.Y;
                                goto OUT;
                            }
                        }
                        bool clear = true;
                        var targ = 查重(form, pt,ref clear);
                        if (targ.X != pt.X || targ.Y != pt.Y)
                        {
                            ret.Add(new KeyValuePair<Point, Point>(pt, targ), clear);
                        }
                    }
                OUT:;
                }
            }
            var rs = new Dictionary<Point, Point>();
            foreach(var kv in ret)
            {
                if (kv.Value)
                {
                    rs.Add(kv.Key.Key, kv.Key.Value);
                }
            }
            return rs;
        }

        public static Point 查重(FormCell[,] form,Point basic,ref bool f)
        {
            var cell = form[basic.X,basic.Y];
            int r = basic.X;
            int c = basic.Y;
            switch (cell.形态)
            {
                case FormCell.Formation.下增生:
                    return 查重(form, new Point(r - 1, c), ref f);
                case FormCell.Formation.右增生:
                    return 查重(form, new Point(r, c - 1), ref f);
                case FormCell.Formation.原生:
                    if (cell.内容.Equals(""))
                    {
                        f = false;
                    }
                    else
                    {
                        f = true;
                    }
                    break;
            }
            return new Point(r, c);
        }

        public static Point Measure(FormCell[,] Matrix)
        {
            int c = (int)Matrix.GetLongLength(1) - 1;
            int r = (int)Matrix.GetLongLength(0) - 1;
            while ((Matrix[r, c] == null || FormCell.判断空或增生单元(Matrix[r,c].内容)) && r >= 0)
            {
                if (c == 0)
                {
                    r--;
                    c = (int)Matrix.GetLongLength(1) - 1;
                }
                else
                {
                    c--;
                }
            }
            int retr = r;
            if (r > 0)
            {
                c = (int)Matrix.GetLongLength(1) - 1;
                while ((Matrix[r, c] == null || FormCell.判断空或增生单元(Matrix[r, c].内容))  && c >= 0 )
                {
                    if (r == 0)
                    {
                        c--;
                        r = retr;
                    }
                    else
                    {
                        r--;
                    }
                }
            }
            return new Point(retr, c);
        }

        public static Task<bool> GetRET()
        {
            return 
                new Task<bool>(() =>
                {
                    Thread.Sleep(3000); return true;
                });
        }


        public static JObject GeneralBasicDemo(Ocr client,string PATH)
        {
            var image = File.ReadAllBytes(PATH);
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

        public void GeneralBasicUrlDemo(Ocr client)
        {
            var url = "http://p0.so.qhimgs1.com/bdr/_240_/t017cab315ee6c04600.jpg";

            // 调用通用文字识别, 图片参数为远程url图片，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.GeneralBasicUrl(url);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
        {"language_type", "CHN_ENG"},
        {"detect_direction", "true"},
        {"detect_language", "true"},
        {"probability", "true"}
    };
            // 带参数调用通用文字识别, 图片参数为远程url图片
            result = client.GeneralBasicUrl(url, options);
            Console.WriteLine(result);
            Console.Read();
        }

        private static void Ping_PingCompleted(object sender, System.Net.NetworkInformation.PingCompletedEventArgs e)
        {
            res = e.Reply.ToString();
        }

        private static byte[] StrToBytes(string str)
        {
            byte[] ret = new byte[str.Length];
            for(int i = 0; i < str.Length; i++)
            {
                ret[i] = ((byte)str[i]);
            }
            return ret;
        }
        //调用tesseract实现OCR识别
        public static string ImageToText(string imgPath)
        {
            string dire = @"H:\IntelliCloud\Test\tessdata";
            string file = packs[0];
            if (!File.Exists(Path.Combine(dire,file+".traineddata")))
            {
                throw new Exception("NOT FOUND");
            }
            using var engine = new TesseractEngine(dire, file, EngineMode.Default);
            using (var img = Pix.LoadFromFile(imgPath))
            {
                using (var page = engine.Process(img))
                {
                    return page.GetText();
                }
            }
        }

        public static void WriteInFile(JObject obj,string filename)
        {
            string dir = Path.Combine(Environment.CurrentDirectory, "Results");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string now= DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒");
            string name = "-"+Path.GetFileNameWithoutExtension(filename);
            string extension = ".txt";
            string path = Path.Combine(dir, now + name + extension);
            FileStream fs = File.Create(path);
            byte[] targ = System.Text.Encoding.UTF8.GetBytes(obj.ToString());
            fs.Write(targ);
            fs.Close();
        }
  
	}
}
