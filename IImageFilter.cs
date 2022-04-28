using System.Collections.Generic;
using System.Drawing;

namespace MyOpenCv.API
{
    public interface IImageFilter
    {
        #region 灰度化
        Bitmap Graying(Bitmap bitmap);
        ICollection<Bitmap> Graying(ICollection<Bitmap> bitmaps);
        
        #endregion

        #region 二值化
        Bitmap Binaryzation(Bitmap bitmap);
        ICollection<Bitmap> Binaryzation(ICollection<Bitmap> bitmaps);
        #endregion

        #region 校正
        Bitmap Rectify(Bitmap bitmap);
        ICollection<Bitmap> Rectify(ICollection<Bitmap> bitmaps);
        #endregion

        #region 比对
        double CompareImage(Bitmap b1, Bitmap b2);
        #endregion
    }
}
