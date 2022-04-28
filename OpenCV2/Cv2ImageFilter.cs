using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace MyOpenCv.API
{
    public partial class Cv2ImageFilter :I2ImageFilter
    {
        #region 二值化
        public Bitmap Binaryzation(Bitmap bitmap)
        {
            return Stream2Bitmap(Binaryzation(Bitmap2Stream(bitmap)));
        }
        public MemoryStream Binaryzation(MemoryStream stream)
        {
            var src = Stream2Mat(stream, ImreadModes.Grayscale);
            Mat result = src.Threshold(150, 255, ThresholdTypes.Binary);
            return Mat2Stream(result);
        }
        public ICollection<Bitmap> Binaryzation(ICollection<Bitmap> bitmaps)
        {
            var b = bitmaps.ToArray();
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Binaryzation(b[i]);
            }
            return b;
        }
        #endregion

        #region 灰度
        public Bitmap Graying(Bitmap bitmap)
        {
            return Stream2Bitmap(Binaryzation(Bitmap2Stream(bitmap)));
        }
        public MemoryStream Graying(MemoryStream stream)
        {
            return Mat2Stream(Stream2Mat(stream, ImreadModes.Grayscale));
        }
        public ICollection<Bitmap> Graying(ICollection<Bitmap> bitmaps)
        {
            var b = bitmaps.ToArray();
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Graying(b[i]);
            }
            return b;
        }
        #endregion

        #region 矫正
        public Bitmap Rectify(Bitmap bitmap)
        {
            return Stream2Bitmap(Rectify(Bitmap2Stream(bitmap)));
        }
        public MemoryStream Rectify(MemoryStream stream)
        {
            var src = Stream2Mat(stream, ImreadModes.Grayscale);
            var dst = Switch(src);
            return Mat2Stream(dst);
        }
        public ICollection<Bitmap> Rectify(ICollection<Bitmap> bitmaps)
        {
            var b = bitmaps.ToArray();
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Rectify(b[i]);
            }
            return b;
        }
        #endregion

        #region 比对
        /// <summary>
        /// 灰度直方图
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public double HistG_Compare(Mat m1, Mat m2)
        { 
            double result = 0;
            try
            {
                Mat[] mat1 = new Mat[] { m1 };
                Mat[] mat2 = new Mat[] { m2 };
                //灰度直方图
                Mat histMat1 = new Mat();//用来接收直方图
                Mat histMat2 = new Mat();//用来接收直方图
                int[] channels = new int[] { 0 };//一个通道,初始化为通道0
                int[] histsize = new int[] { 256 };//一个通道，初始化为256箱子
                Rangef[] range = new Rangef[1];//一个通道，值范围
                range[0].Start = 0.0F;//从0开始（含）
                range[0].End = 256.0F;//到256结束（不含）
                Mat mask = new Mat();//不做掩码
                Cv2.CalcHist(mat1, channels, mask, histMat1, 1, histsize, range);//计算灰度图，dim为1 1维
                Cv2.CalcHist(mat2, channels, mask, histMat2, 1, histsize, range);//计算灰度图，dim为1 1维
                result = Cv2.CompareHist(histMat1, histMat2, HistCompMethods.Correl);
                Cv2.PutText(m2, result.ToString(), new Point(0, 100), HersheyFonts.HersheyComplex, 5, Scalar.Black);
               
            }
            catch{} 
            return result;
        }

        /// <summary>
        /// RGB直方图
        /// </summary>
        /// <param name="m1">Mat1</param>
        /// <param name="m2">Mat2</param>
        /// <returns></returns>
        public double[] HistC_Compare(Mat m1, Mat m2)
        {
            try
            {
                double[] resultS;
                double result = 0;
                double[] compare = new double[3];
                //Cv2.ImShow("3", mat);
                Mat[] mat1 = Cv2.Split(m1);
                //Cv2.ImShow("4", mat);
                Mat[] mat2 = Cv2.Split(m2);
                //彩色直方图
                Mat histMat1 = new Mat();//用来接收直方图
                Mat histMat2 = new Mat();//用来接收直方图
                int[] channels = new int[] { 0 };//一个通道,初始化为通道0
                int[] histsize = new int[] { 256 };//一个通道，初始化为256箱子
                Rangef[] range = new Rangef[1];//一个通道，值范围
                range[0].Start = 0.0F;//从0开始（含）
                range[0].End = 256.0F;//到256结束（不含）
                Mat mask = new Mat();//不做掩码
                for (int i = 0; i < compare.Length; i++)
                {
                    Cv2.CalcHist(new Mat[] { mat1[i] }, channels, mask, histMat1, 1, histsize, range);//dim为3 3维
                    Cv2.CalcHist(new Mat[] { mat2[i] }, channels, mask, histMat2, 1, histsize, range);//dim为3 3维
                    compare[i] = Cv2.CompareHist(histMat1, histMat2, HistCompMethods.Correl);
                }
                result = compare.Min();
                Cv2.PutText(m1, result.ToString(), new Point(0, 100), HersheyFonts.HersheyComplex, 5, Scalar.Black);
                resultS = new double[] { compare[0], compare[1], compare[2] };
                return resultS;
            }
            catch
            {
                return new double[] { };
            }
        }

        /// <summary>
        /// PSNR峰值信噪比
        /// </summary>
        /// <param name="m1">Mat1</param>
        /// <param name="m2">Mat2</param>
        /// <returns></returns>
        public double PSNR_Compare(Mat m1, Mat m2)
        {
            try
            {
                // 确保它们的大小是一致的
                Mat Diff = new Mat();
                Cv2.Resize(m2, m2, m1.Size());

                m1.ConvertTo(m1, MatType.CV_32F);
                m2.ConvertTo(m2, MatType.CV_32F);
                // compute PSNR
                // Diff一定要提前转换为32F，因为uint8格式的无法计算成平方
                Diff.ConvertTo(Diff, MatType.CV_32F);
                Cv2.Absdiff(m1, m2, Diff);

                Diff = Diff.Mul(Diff);
                Scalar scalar = Cv2.Sum(Diff);

                double sse;   // square error
                if (Diff.Channels() == 3)
                    sse = scalar.Val0 + scalar.Val1 + scalar.Val2;  // sum of all channels
                else
                    sse = scalar.Val0;

                int nTotalElement = m1.Channels() * (int)m1.Total();

                double mse = (sse / (double)nTotalElement);  // 

                // 加上0.0000001作为偏置，不至于发生除了的错误
                double psnr = 10.0 * Math.Log10(255 * 255 / (mse + 0.0000001));
                return psnr;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// SSIM结构相似性
        /// </summary>
        /// <param name="m1">Mat1</param>
        /// <param name="m2">Mat2</param>
        /// <returns></returns>
        public double[] SSIM_Compare(Mat m1,Mat m2)
        {
            double[] mssim = new double[3] { 0, 0, 0 };
            try
            {
                double C1 = 6.5025, C2 = 58.5225;

                Mat I1 = new Mat();
                Mat I2 = new Mat();
                m1.ConvertTo(I1, MatType.CV_32F);
                m2.ConvertTo(I2, MatType.CV_32F);

                Mat I1_2 = I1.Mul(I1);
                Mat I2_2 = I2.Mul(I2);
                Mat I1_I2 = I1.Mul(I2);

                Mat mu1 = new Mat();
                Mat mu2 = new Mat();
                Size size = new Size(11, 11);
                Cv2.GaussianBlur(I1, mu1, size, 1.5);
                Cv2.GaussianBlur(I2, mu2, size, 1.5);
                Mat mu1_2 = mu1.Mul(mu1);
                Mat mu2_2 = mu2.Mul(mu2);
                Mat mu1_mu2 = mu1.Mul(mu2);
                Mat sigma1_2 = new Mat();
                Mat sigma2_2 = new Mat();
                Mat sigma12 = new Mat();
                Cv2.GaussianBlur(I1_2, sigma1_2, size, 1.5);
                sigma1_2 -= mu1_2;
                Cv2.GaussianBlur(I2_2, sigma2_2, size, 1.5);
                sigma2_2 -= mu2_2;
                Cv2.GaussianBlur(I1_I2, sigma12, size, 1.5);
                sigma12 -= mu1_mu2;
                Mat t1, t2, t3;
                t1 = 2 * mu1_mu2 + C1;
                t2 = 2 * sigma12 + C2;
                t3 = t1.Mul(t2);
                t1 = mu1_2 + mu2_2 + C1;
                t2 = sigma1_2 + sigma2_2 + C2;
                t1 = t1.Mul(t2);
                Mat ssim_map = new Mat();
                Cv2.Divide(t3, t1, ssim_map);
                Scalar scalar = Cv2.Mean(ssim_map);
                if (ssim_map.Channels() == 3)
                {
                    mssim[0] = scalar.Val0;
                    mssim[1] = scalar.Val1;
                    mssim[2] = scalar.Val2;
                }
                else
                    mssim[0] = scalar.Val0;
                return mssim;
            }
            catch
            {
                return mssim;
            }
        }

        /// <summary>
        /// 图像比对
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public double[] CompareImage(Bitmap b1, Bitmap b2)
        {
            return CompareImage(Bitmap2Stream(b1), Bitmap2Stream(b2));
        }

        public double[] CompareImage(MemoryStream mo1, MemoryStream mo2)
        {
            Mat m1 = Stream2Mat(mo1, ImreadModes.Grayscale);
            Mat m2 = Stream2Mat(mo2, ImreadModes.Grayscale);
            double ret1 = HistG_Compare(m1, m2);
            double[] ret2 = HistC_Compare(m1, m2);
            double ret3 = PSNR_Compare(m1, m2);
            double[] ret4 = SSIM_Compare(m1, m2);
            List<double> ret = new List<double>() { ret1 };
            ret.AddRange(ret2);
            ret.Add(ret3);
            ret.AddRange(ret4);
            return ret.ToArray();
        }
        #endregion

        #region 私有
        /// <summary>
        /// Stream转化为Mat
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private static Mat Stream2Mat(MemoryStream stream,ImreadModes mode)
        {
            return Mat.FromStream(stream, mode);
        }

        /// <summary>
        /// Mat转化为Stream
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        private static MemoryStream Mat2Stream(Mat mat)
        {
            return mat.ToMemoryStream();
        }

        /// <summary>
        /// Bitmap转化为Stream
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static MemoryStream Bitmap2Stream(Bitmap image)
        {
            MemoryStream instream = new MemoryStream();
            image.Save(instream, System.Drawing.Imaging.ImageFormat.Bmp);
            return instream;
        }

        /// <summary>
        /// Stream转化为Bitmap
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static Bitmap Stream2Bitmap(MemoryStream stream)
        {
            return (Bitmap)Image.FromStream(stream);
            
        }


        /// <summary>
		/// 将低频部分移动到图像中心
		/// </summary>
		/// <param name="image"></param>
		/// <remarks>
		///  0 | 3         2 | 1
		/// -------  ===> -------
		///  1 | 2         3 | 0
		/// </remarks>
		protected static void ShiftDFT(Mat image)
        {
            int row = image.Height;
            int col = image.Width;
            int cy = row / 2;
            int cx = col / 2;

            var q0 = image.Clone(new Rect(0, 0, cx, cy));//左上
            var q1 = image.Clone(new Rect(0, cy, cx, cy));//左下
            var q2 = image.Clone(new Rect(cx, cy, cx, cy));//右下
            var q3 = image.Clone(new Rect(cx, 0, cx, cy));//右上

            image.AdjustROI(0,0,cx,cy);
            q2.CopyTo(image);
            image.AdjustROI(0,row,0,col);

            image.AdjustROI(0, cy, cx, cy);
            q3.CopyTo(image);
            image.AdjustROI(0, row, 0, col);

            image.AdjustROI(cx, cy, cx, cy);
            q0.CopyTo(image);
            image.AdjustROI(0, row, 0, col);

            image.AdjustROI(cx, 0, cx, cy);
            q1.CopyTo(image);
            image.AdjustROI(0, row, 0, col);
        }
        /// <summary>
        /// 获取后缀
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        protected static string GetExtension(System.Drawing.Imaging.ImageFormat format)
        {
            string ret = string.Empty;
            if (format == System.Drawing.Imaging.ImageFormat.Bmp)
            {
                ret = ".bmp";
            }
            if (format == System.Drawing.Imaging.ImageFormat.Gif)
            {
                ret = ".gif";
            }
            if (format == System.Drawing.Imaging.ImageFormat.Jpeg)
            {
                ret = ".jpg";
            }
            if (format == System.Drawing.Imaging.ImageFormat.Png)
            {
                ret = ".png";
            }
            return ret;
        }
        /// <summary>
        /// 虚实处理
        /// </summary>
        /// <param name="padded">变换后的图像</param>
        /// <returns>灰度图片</returns>
        protected static Mat Un_Real_Format(Mat padded)
        {
            Cv2.CvtColor(padded, padded, ColorConversionCodes.BGR2GRAY);
            //实部、虚部(单通道)
            var real = new Mat(padded.Size(), MatType.CV_32F,1);
            var imaginary = new Mat(padded.Size(), MatType.CV_32F, 1);
            //合并(双通道)
            var fourier = new Mat(padded.Size(), MatType.CV_32F, 2);

            //图像复制到实部，虚部清零
            Cv2.ConvertScaleAbs(padded, real);
            imaginary = new Mat();

            int r = real.Depth();
            int m = imaginary.Depth();
            //合并、变换、再分解
            Cv2.Merge(new Mat[] { real, imaginary }, fourier);
            Cv2.Dft(fourier, fourier, DftFlags.None);
            var ms = new Mat[] { real, imaginary };
            Cv2.Split(fourier, out ms);

            //计算sqrt(re^2+im^2)，再存回re
            Cv2.Pow(real, 2.0, real);
            Cv2.Pow(imaginary, 2.0,imaginary);
            //Cv2.Add(imaginary, real, real);
            //Cv2.Pow(real, 0.5, real);

            //计算log(1+re)，存回re
            Cv2.Add(real, Scalar.All(1), real);
            int i = real.Depth();
            real = real.Log();
            //归一化，落入0-255范围
            Cv2.Normalize(real, real, 0, 255, NormTypes.MinMax);

            //把低频移动到中心
            ShiftDFT(real);

            //二值化，以150作为分界点，经验值，需要根据实际情况调整
            //Cv.Threshold(real, real, 150, 255, ThresholdType.Binary);
            Cv2.Threshold(real, real, 150, 255, ThresholdTypes.Binary);

            //由于HoughLines2方法只接受8UC1格式的图片，因此进行转换
            var gray = new Mat(real.Size(), MatType.CV_32F);
            Cv2 .ConvertScaleAbs(real, gray);

            return gray;
        }
        /// <summary>
        /// 变形
        /// </summary>
        /// <param name="src">原图片</param>
        /// <param name="gray">灰度图片</param>
        /// <returns></returns>
        protected static Mat Transform(Mat src, Mat gray)
        {
            
            var lines = Cv2.HoughLines(gray, 1, Cv2.PI / 180, 100);

            //找到符合条件的那条斜线
            float angel = 0f;
            float piThresh = (float)Cv2.PI / 90;
            float pi2 = (float)Cv2.PI / 2;
            for (int i = 0; i < lines.Length; ++i)
            {
                //极坐标下的点，X是极径，Y是夹角，我们只关心夹角
                var p = lines[i];
                float theta = p.Theta;

                if (Math.Abs(theta) >= piThresh && Math.Abs(theta - pi2) >= piThresh)
                {
                    angel = theta;
                    break;
                }
            }
            angel = angel < pi2 ? angel : (angel - (float)Cv2.PI);

            //Cv2.CompareHist(src, gray, HistCompMethods.Correl);
            //转换角度
            if (angel != pi2)
            {
                float angelT = (float)(src.Height * Math.Tan(angel) * 1.0 / src.Width);
                angel = (float)Math.Atan(angelT);
            }
            float angelD = angel * 180 / (float)Cv2.PI;

            //旋转
            var center = new Point2f(src.Width / 2, src.Height / 2);
            var rotMat = Cv2.GetRotationMatrix2D(center, angelD, 1.0);
            var dst = new Mat(src.Size(), MatType.CV_8U,1);
            Cv2.WarpAffine(src, dst, rotMat,new OpenCvSharp.Size(dst.Width,dst.Height), InterpolationFlags.Cubic | InterpolationFlags.WarpFillOutliers,BorderTypes.Default, Scalar.All(255));

            return dst;
        }

        /// <summary>
        /// 比对
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        protected double Compare(Mat p1, Mat p2)
        {
            return Cv2.CompareHist(p1, p2, HistCompMethods.Correl);
        }

        protected Mat Switch(Mat src)
        {
            Mat dest = new Mat();
            Mat origin = src.Clone();
            src = src.Canny(50, 200, 3);
            //Cv2.ImShow("边缘检测效果", src);

            Cv2.CvtColor(src, dest, ColorConversionCodes.GRAY2BGR);

            OpenCvSharp.Size size;
            size.Width = 3;
            size.Height = 3;
            src = src.MorphologyEx(MorphTypes.Dilate, Cv2.GetStructuringElement(MorphShapes.Rect, size));
           /*
            *  HoughLinesP:使用概率霍夫变换查找二进制图像中的线段。
            *  参数：
            *      1； image: 输入图像 （只能输入单通道图像）
            *      2； rho:   累加器的距离分辨率(以像素为单位) 生成极坐标时候的像素扫描步长
            *      3； theta: 累加器的角度分辨率(以弧度为单位)生成极坐标时候的角度步长，一般取值CV_PI/180 ==1度
            *      4； threshold: 累加器阈值参数。只有那些足够的行才会返回 投票(>阈值)；设置认为几个像素连载一起才能被看做是直线。
            *      5； minLineLength: 最小线长度，设置最小线段是有几个像素组成。
            *      6；maxLineGap: 同一条线上的点之间连接它们的最大允许间隙。(默认情况下是0）：设置你认为像素之间间隔多少个间隙也能认为是直线
            *      返回结果:
            *      输出线。每条线由一个4元向量(x1, y1, x2，y2)
            */
            LineSegmentPoint[] linepiont = Cv2.HoughLinesP(src, 1, Cv2.PI / 180, 300, 0, 0);

            if (linepiont.Count() == 0)
            {
                linepiont = Cv2.HoughLinesP(src, 1, Cv2.PI / 180, 200, 0, 0);
            }

            if (linepiont.Count() == 0)
            {
                linepiont = Cv2.HoughLinesP(src, 1, Cv2.PI / 180, 150, 0, 0);
            }

            double[] lenArr = new double[linepiont.Count()];
            for (int i = 0; i < linepiont.Count(); i++)
            {
                OpenCvSharp.Point p1 = linepiont[i].P1;
                OpenCvSharp.Point p2 = linepiont[i].P2;
                lenArr[i] = GetTwoPointDistance(p1, p2);
                //rtbResult.Text += "point" + (i + 1).ToString() + ":p1 x=" + p1.X.ToString() + ",p1 y=" + p1.Y.ToString() + ":p2 x=" + p2.X.ToString() + ",p2 y=" + p2.Y.ToString() + "\r\n";
                Cv2.Line(dest, p1, p2, Scalar.Red, 2, LineTypes.Link8);

            }
            Cv2.ImShow("直线探测效果", dest);
            double max = lenArr.Max();
            int j = lenArr.ToList().IndexOf(max);//用最长的那条直线来计算旋转角度
            OpenCvSharp.Point pp1 = linepiont[j].P1;
            OpenCvSharp.Point pp2 = linepiont[j].P2;
            //Cv2.Line(dest, pp1, pp2, Scalar.Red, 2, LineTypes.Link8);
            //Cv2.ImShow("直线探测效果", dest);
            double angel = GetAngel(pp1, pp2);
            Mat final = new Mat();
            Rotate(origin, final, angel);
            Console.WriteLine("倾斜校正成功,旋转了" + angel.ToString("F2") + "°", "成功");
            return final;
        }
        private double GetTwoPointDistance(Point p1, Point p2)
        {
            int dx, dy;
            dx = Math.Abs(p1.X - p2.X);
            dy = Math.Abs(p1.Y - p2.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private double GetAngel(Point p1, Point p2)
        {
            int dx, dy;
            double angel;
            dx = Math.Abs(p1.X - p2.X);
            dy = Math.Abs(p1.Y - p2.Y);
            double atan = Math.Atan(((double)dy) / ((double)dx));
            if (p1.Y < p2.Y)
            {
                angel = atan * 180 / Cv2.PI;
            }
            else
            {
                angel = atan * 180 / Cv2.PI;
                angel = 0 - angel;
            }
            return angel;
        }

        //旋转图像degree角度（原尺寸）, degree > 0,逆时针, degree < 0,顺时针    
        private void Rotate(Mat src, Mat dest, double degree)
        {
            //旋转中心为图像中心    
            Point2f center = new Point2f();
            center.X = (float)(src.Cols / 2.0);
            center.Y = (float)(src.Rows / 2.0);
            //int length = 0;
            //length = (int)Math.Sqrt(src.Cols * src.Cols + src.Rows * src.Rows);
            //计算二维旋转的仿射变换矩阵  
            Mat M = Cv2.GetRotationMatrix2D(center, degree, 1);
            OpenCvSharp.Size size;
            size.Width = src.Cols;
            size.Height = src.Rows;
            Cv2.WarpAffine(src, dest, M, size, InterpolationFlags.Linear, 0, Scalar.Black);//仿射变换，背景色填充为黑 
        }

        protected Mat Twist(Mat mat)
        {
            Mat tmp = new Mat();
            Cv2.CvtColor(mat, tmp, ColorConversionCodes.RGB2GRAY);
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(20, 20));
            Cv2.MorphologyEx(tmp, tmp, MorphTypes.Open, kernel);
            Cv2.MorphologyEx(tmp, tmp, MorphTypes.Close, kernel);
            Cv2.Threshold(tmp, tmp, 80, 255, ThresholdTypes.Binary);
            Mat edges = new Mat();
            Cv2.Canny(tmp, edges, 20, 160);
            var lines =  Cv2.HoughLines(edges, 1, Math.PI / 180, 45, 20, 20);
            //找到符合条件的那条斜线
            float angel = 0f;
            float piThresh = (float)Cv2.PI / 90;
            float pi2 = (float)Cv2.PI / 2;
            for (int i = 0; i < lines.Length; ++i)
            {
                //极坐标下的点，X是极径，Y是夹角，我们只关心夹角
                var p = lines[i];
                float theta = p.Theta;

                if (Math.Abs(theta) >= piThresh && Math.Abs(theta - pi2) >= piThresh)
                {
                    angel = theta;
                    break;
                }
            }
            angel = angel < pi2 ? angel : (angel - (float)Cv2.PI);

            //Cv2.CompareHist(src, gray, HistCompMethods.Correl);
            //转换角度
            if (angel != pi2)
            {
                float angelT = (float)(edges.Height * Math.Tan(angel) * 1.0 / edges.Width);
                angel = (float)Math.Atan(angelT);
            }
            float angelD = angel * 180 / (float)Cv2.PI;

            //旋转
            var center = new Point2f(edges.Width / 2, edges.Height / 2);
            var rotMat = Cv2.GetRotationMatrix2D(center, angelD, 1.0);
            var dst = new Mat(edges.Size(), MatType.CV_8U, 1);
            Cv2.WarpAffine(edges, tmp, rotMat, new OpenCvSharp.Size(tmp.Width, tmp.Height), InterpolationFlags.Cubic | InterpolationFlags.WarpFillOutliers, BorderTypes.Default, Scalar.All(255));

            return dst;
        }









        #endregion
    }
}
