using OpenCvSharp;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MyOpenCv.API
{
    public interface IImageFilter
    {
        #region 灰度化
        Bitmap Graying(Bitmap bitmap);
        MemoryStream Graying(MemoryStream stream);
        ICollection<Bitmap> Graying(ICollection<Bitmap> bitmaps);

        #endregion

        #region 二值化
        Bitmap Binaryzation(Bitmap bitmap);

        MemoryStream Binaryzation(MemoryStream stream);
        ICollection<Bitmap> Binaryzation(ICollection<Bitmap> bitmaps);
        #endregion

        #region 校正
        Bitmap Rectify(Bitmap bitmap);
        MemoryStream Rectify(MemoryStream stream);
        ICollection<Bitmap> Rectify(ICollection<Bitmap> bitmaps);
        #endregion

    }
}
