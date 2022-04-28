using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.API
{
    /// <summary>
    /// 控制Word和Excel进程的工厂
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// 提交Excel任务
        /// </summary>
        /// <param name="user"></param>
        /// <param name="searches"></param>
        /// <returns></returns>
        Task<Dictionary<SearchBase, CancellationTokenSource>> SubmitWork(User user,ref List<ExcelSearch> searches);
        /// <summary>
        /// 提交Word任务
        /// </summary>
        /// <param name="user"></param>
        /// <param name="searches"></param>
        /// <returns></returns>
        Task<Dictionary<SearchBase, CancellationTokenSource>> SubmitWork(User user,ref List<WordSearch> searches);
        /// <summary>
        /// 提交图片任务
        /// </summary>
        /// <param name="user"></param>
        /// <param name="searches"></param>
        /// <returns></returns>
        Task<Dictionary<SearchBase, CancellationTokenSource>> SubmitWork(User user,ref List<PictureSearch> searches);

        void Cancel(User user,进程模型 模型);
        void Cancel(User user);

        DocxUnit NewDocx(User user);
        ExcelUnit NewExcel(User user);

        void ReturnUnit(User user,UnitBase unit);
    }
}
