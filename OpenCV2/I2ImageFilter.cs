using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using OpenCvSharp;
namespace MyOpenCv.API
{
    public interface I2ImageFilter : IImageFilter
    {
        /// <summary>
        /// 灰度直方图
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        double HistG_Compare(Mat m1, Mat m2);

        /// <summary>
        /// RGB直方图
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        double[] HistC_Compare(Mat m1, Mat m2);

        /// <summary>
        /// PSNR峰值信噪比
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        double PSNR_Compare(Mat m1, Mat m2);

        /// <summary>
        /// SSIM结构相似性
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        double[] SSIM_Compare(Mat m1, Mat m2);

        /// <summary>
        /// 图像比对
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        double[] CompareImage(Bitmap b1, Bitmap b2);

        double[] CompareImage(MemoryStream m1, MemoryStream m2);
    }
}
