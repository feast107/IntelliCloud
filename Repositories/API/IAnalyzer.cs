using ModelLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.API
{
    public interface IAnalyzer
    {
        string ImageToResult(int id,Stream image);

        Task<string> ImageToResultAsync(int id, Stream image);

        /// <summary>
        /// 查重通过返回-1，不通过返回编号
        /// </summary>
        /// <param name="targ">需要比较的目标图像</param>
        /// <param name="srcs">被比较图像</param>
        /// <returns></returns>
        int 查重(PictureSearch targ, PictureSearch[] srcs);

        Task 识别(PictureSearch search);
    }
}
