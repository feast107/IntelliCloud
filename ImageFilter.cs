using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenCvSharp;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices.ComTypes;

namespace  MyOpenCv.API
{
    public class ImageFilter : IImageFilter
    {
        #region 二值化
        public Bitmap Binaryzation(Bitmap bitmap)
        {
            for (int r = 0; r < bitmap.Width; r++)
            {
                for (int c = 0; c < bitmap.Height; c++)
                {
                    var color = bitmap.GetPixel(r, c);
                    if (color.G > 150)
                    {
                        color = Color.FromArgb(255, 255, 255);
                    }
                    else
                    {
                        color = Color.FromArgb(0, 0, 0);
                    }
                    bitmap.SetPixel(r, c, color);
                }
            }
            return bitmap;
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
            for(int r = 0; r < bitmap.Width; r++)
            {
                for(int c = 0; c < bitmap.Height; c++)
                {
                    var color = bitmap.GetPixel(r, c);
                    int gray = (color.R + color.G + color.B)/3;
                    color = Color.FromArgb(gray, gray, gray);
                    bitmap.SetPixel(r, c, color);
                }
            }
            return bitmap;
        }
        public ICollection<Bitmap> Graying(ICollection<Bitmap> bitmaps)
        {
            var b = bitmaps.ToArray();
            for(int i = 0; i < b.Length; i++)
            {
                b[i] = Graying(b[i]);
            }
            return b;
        }
        #endregion

        #region 矫正
        public Bitmap Rectify(Bitmap bitmap)
        {
            var src = Bm2Ipl(bitmap);

            //转换到合适的大小，以适应快速变换
            var padded = new IplImage(bitmap.Width, bitmap.Height, BitDepth.U8, 1);

            Cv.CopyMakeBorder(src, padded, new CvPoint(0, 0), BorderType.Constant, CvScalar.ScalarAll(0));

            var gray = Un_Real_Format(padded);
            //找直线，threshold参数取100，经验值，需要根据实际情况调整

            var dst = Transform(src, gray);

            return  Ipl2Bm(dst);
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
 
        public double CompareImage(Bitmap b1,Bitmap b2)
        {
            return Compare(Bm2Ipl(b1),Bm2Ipl(b2));
        }

        /// <summary>
        /// 将bitmap转化为Ipl
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static IplImage Bm2Ipl(Bitmap bitmap)
        {
            MemoryStream instream = new MemoryStream();
            bitmap.Save(instream, System.Drawing.Imaging.ImageFormat.Bmp);
            var ret = IplImage.FromStream(instream, LoadMode.GrayScale);
            Mat m = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
            Task.Run(()=> instream.Close());
            return ret;
        }

        /// <summary>
        /// 将Ipl转化为bitmap
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static Bitmap Ipl2Bm(IplImage image)
        {
            MemoryStream outstream = new MemoryStream();
            image.ToStream(outstream, GetExtension(System.Drawing.Imaging.ImageFormat.Bmp));
            var ret = new Bitmap((Bitmap)Image.FromStream(outstream));
            Task.Run(() => outstream.Close());
            return ret;
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
		protected static void ShiftDFT(IplImage image)
        {
            int row = image.Height;
            int col = image.Width;
            int cy = row / 2;
            int cx = col / 2;

            var q0 = image.Clone(new CvRect(0, 0, cx, cy));//左上
            var q1 = image.Clone(new CvRect(0, cy, cx, cy));//左下
            var q2 = image.Clone(new CvRect(cx, cy, cx, cy));//右下
            var q3 = image.Clone(new CvRect(cx, 0, cx, cy));//右上

            Cv.SetImageROI(image, new CvRect(0, 0, cx, cy));
            q2.Copy(image);
            Cv.ResetImageROI(image);

            Cv.SetImageROI(image, new CvRect(0, cy, cx, cy));
            q3.Copy(image);
            Cv.ResetImageROI(image);

            Cv.SetImageROI(image, new CvRect(cx, cy, cx, cy));
            q0.Copy(image);
            Cv.ResetImageROI(image);

            Cv.SetImageROI(image, new CvRect(cx, 0, cx, cy));
            q1.Copy(image);
            Cv.ResetImageROI(image);
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
            if(format == System.Drawing.Imaging.ImageFormat.Jpeg)
            {
                ret = ".jpg";
            }    
            if(format == System.Drawing.Imaging.ImageFormat.Png)
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
        protected static IplImage Un_Real_Format(IplImage padded)
        {
            //实部、虚部(单通道)
            var real = new IplImage(padded.Size, BitDepth.F32, 1);
            var imaginary = new IplImage(padded.Size, BitDepth.F32, 1);
            //合并(双通道)
            var fourier = new IplImage(padded.Size, BitDepth.F32, 2);

            //图像复制到实部，虚部清零
            Cv.ConvertScale(padded, real);
            Cv.Zero(imaginary);

            //合并、变换、再分解
            Cv.Merge(real, imaginary, null, null, fourier);
            Cv.DFT(fourier, fourier, DFTFlag.Forward);
            Cv.Split(fourier, real, imaginary, null, null);

            //计算sqrt(re^2+im^2)，再存回re
            Cv.Pow(real, real, 2.0);
            Cv.Pow(imaginary, imaginary, 2.0);
            Cv.Add(real, imaginary, real);
            Cv.Pow(real, real, 0.5);

            //计算log(1+re)，存回re
            Cv.AddS(real, CvScalar.ScalarAll(1), real);
            Cv.Log(real, real);

            //归一化，落入0-255范围
            Cv.Normalize(real, real, 0, 255, NormType.MinMax);

            //把低频移动到中心
            ShiftDFT(real);

            //二值化，以150作为分界点，经验值，需要根据实际情况调整
            //Cv.Threshold(real, real, 150, 255, ThresholdType.Binary);
            Cv.Threshold(real, real, 150, 255, ThresholdType.Binary);

            //由于HoughLines2方法只接受8UC1格式的图片，因此进行转换
            var gray = new IplImage(real.Size, BitDepth.U8, 1);
            Cv.ConvertScale(real, gray);

            return gray;
        }
       
        /// <summary>
        /// 变形
        /// </summary>
        /// <param name="src">原图片</param>
        /// <param name="gray">灰度图片</param>
        /// <returns></returns>
        protected static IplImage Transform(IplImage src,IplImage gray)
        {
            var storage = Cv.CreateMemStorage();
            var lines = Cv.HoughLines2(gray, storage, HoughLinesMethod.Standard, 1, Cv.PI / 180, 100);

            //找到符合条件的那条斜线
            float angel = 0f;
            float piThresh = (float)Cv.PI / 90;
            float pi2 = (float)Cv.PI / 2;
            for (int i = 0; i < lines.Total; ++i)
            {
                //极坐标下的点，X是极径，Y是夹角，我们只关心夹角
                var p = lines.GetSeqElem<CvPoint2D32f>(i);
                float theta = p.Value.Y;

                if (Math.Abs(theta) >= piThresh && Math.Abs(theta - pi2) >= piThresh)
                {
                    angel = theta;
                    break;
                }
            }
            angel = angel < pi2 ? angel : (angel - (float)Cv.PI);
            Cv.ReleaseMemStorage(storage);

            Cv.CompareHist(new CvHistogram(src.CvPtr), new CvHistogram(gray.CvPtr),HistogramComparison.Correl);
            //转换角度
            if (angel != pi2)
            {
                float angelT = (float)(src.Height * Math.Tan(angel) * 1.0 / src.Width);
                angel = (float)Math.Atan(angelT);
            }
            float angelD = angel * 180 / (float)Cv.PI;

            //旋转
            var center = new CvPoint2D32f(src.Width / 2.0, src.Height / 2.0);
            var rotMat = Cv.GetRotationMatrix2D(center, angelD, 1.0);
            var dst = new IplImage(src.Size, BitDepth.U8, 1);
            Cv.WarpAffine(src, dst, rotMat, Interpolation.Cubic | Interpolation.FillOutliers, CvScalar.ScalarAll(255));

            return dst;
        }

        /// <summary>
        /// 比对
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        protected double Compare(IplImage p1,IplImage p2)
        {
            var h1 = new CvHistogram();
            new CvMat(p1.CvPtr);
            Cv.CalcArrHist(p1, h1);
            var h2 = new CvHistogram();
            Cv.CalcHist(p2, h2);
            return h1.Compare( h2, HistogramComparison.Correl);
        }
    }
}
