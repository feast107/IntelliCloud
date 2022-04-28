using ModelLib;
using Repositories.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.Configuration;
using System.Threading;
using System.Linq;
using System.IO;

namespace Repositories
{
    public class WorkShop
    {
        #region 静态区
        /// <summary>
        /// 仓储计数
        /// </summary>
        private static int Count;
        private static readonly string[] ResultsName = new[] { "Excel表", "Word收集表" };
        #endregion

        #region 构造器
        internal WorkShop(IFactory factory, IAnalyzer analyzer, User user)
        {
            _analyzer = analyzer;
            _factory = factory;
            Id = Count++;
            Owner = user;
            status = 进程状态.闲置;
        }
        #endregion

        #region 实例区
        private readonly IAnalyzer _analyzer;
        private readonly IFactory _factory;
        private readonly int Id;
        public int 编号 => Id;


        private readonly User Owner;
        public User 所有者 => Owner;



        private readonly DateTime CreateTime = DateTime.Now;
        public DateTime 创建时间 => CreateTime;



        private 进程状态 status;
        public 进程状态 状态 => status;



        public readonly List<SearchBase> 任务列表 = new List<SearchBase>();


        #region Excel工作区
        public readonly Dictionary<SearchBase, CancellationTokenSource> Excel执行列表 = new Dictionary<SearchBase, CancellationTokenSource>();
        private List<ExcelSearch> Excel提交队列 = new List<ExcelSearch>();
        private bool ExcelSubmitting = false;
        #endregion


        #region Word工作区
        public readonly Dictionary<SearchBase, CancellationTokenSource> Word执行列表 = new Dictionary<SearchBase, CancellationTokenSource>();
        private List<WordSearch> Word提交队列 = new List<WordSearch>();
        private bool WordSubmmiting = false;
        #endregion

        #region 识别工作区
        public readonly Dictionary<PictureSearch, CancellationTokenSource> 图片执行列表 = new Dictionary<PictureSearch, CancellationTokenSource>();
        private List<PictureSearch> 识别提交队列 = new List<PictureSearch>();
        private bool PictureSubmming = false;
        #endregion


        private string Excel结果;

        private string Word结果;
        #endregion



        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="任务"></param>
        /// <returns></returns>
        public bool[] 提交(List<SearchBase> 任务)
        {
            bool[] bools = new bool[任务.Count];
            int index = 0;
            任务.ForEach((x =>
            {
                if (!任务列表.Contains(x))
                {
                    任务列表.Add(x);
                    bools[index] = true;
                }
                else
                {
                    bools[index] = false;
                }
                index++;
            }));
            return bools;
        }
        public bool[] 提交(List<ExcelSearch> 任务)
        {
            List<SearchBase> searches = new List<SearchBase>();
            任务.ForEach(x => searches.Add(x));
            return 提交(searches);
        }
        public bool[] 提交(List<WordSearch> 任务)
        {
            List<SearchBase> searches = new List<SearchBase>();
            任务.ForEach(x => searches.Add(x));
            return 提交(searches);
        }
        public int 提交(PictureSearch 任务)
        {
            if (识别提交队列.Count != 0)
            {
                lock (识别提交队列)
                {
                    int 重复 = _analyzer.查重(任务, 识别提交队列.ToArray());
                    if (-1 == 重复)
                    {
                        识别提交队列.Add(任务);
                        return 任务.编号;
                    }
                    else
                    {
                        return 重复;
                    }
                }
            }
            else
            {
                识别提交队列.Add(任务);
                return 任务.编号;
            }
        }



        public void 中断(int[] 编号)
        {
            foreach (int i in 编号)
            {
                中断(i);
            }
        }
        public void 中断(int 编号)
        {
            SearchBase search = 任务列表.Find(x => x.编号 == 编号);
            if (search != null && search.状态 == 工作状态.待处理)
            {
                switch (search.类型)
                {
                    case 文件类型.Word:
                        Word执行列表.TryGetValue(search, out CancellationTokenSource dos);
                        dos.Cancel();
                        break;
                    case 文件类型.Excel主表:
                    case 文件类型.Excel子表:
                        Excel执行列表.TryGetValue(search, out CancellationTokenSource exs);
                        exs.Cancel();
                        break;
                }
            }
        }


        public void 终止()
        {
            _factory.Cancel(所有者);
        }
        public void 终止(进程模型 模型)
        {
            _factory.Cancel(所有者, 模型);
        }

        public Task 识别(int 编号)
        {
            foreach (var t in 识别提交队列)
            {
                if (t.编号 == 编号)
                {
                    return Task.Run(() =>
                    {
                        _analyzer.识别(t).Wait();
                        List<PictureSearch> ts = new List<PictureSearch>() { t };
                        var dic = _factory.SubmitWork(Owner, ref ts).Result;
                        foreach (var p in dic)
                        {
                            图片执行列表.Add((PictureSearch)p.Key, p.Value);
                        }
                    });
                }
            }
            return Task.FromException(new Exception("Search Not Found"));
        }


        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <param name="编号"></param>
        /// <returns></returns>
        public Task 执行(List<int> 编号)
        {
            List<WordSearch> words = new List<WordSearch>();
            List<ExcelSearch> excels = new List<ExcelSearch>();
            编号.ForEach((x) =>
            {
                foreach (SearchBase search in 任务列表)
                {
                    if (search.编号.Equals(x) && (search.状态 == 工作状态.创建 || search.状态 == 工作状态.失败))
                    {
                        switch (search.类型)
                        {
                            case 文件类型.Word:
                                words.Add((WordSearch)search);
                                break;
                            case 文件类型.Excel主表:
                            case 文件类型.Excel子表:
                                excels.Add((ExcelSearch)search);
                                break;
                        }
                    }
                }
            });
            if (words.Count != 0)
            {
                if (WordSubmmiting)
                {
                    lock (Word提交队列)
                    {
                        words.ForEach(x =>
                        {
                            if (!Word提交队列.Contains(x))
                            {
                                Word提交队列.Add(x);
                            }

                        });
                    }
                }
                else
                {
                    new Task(() =>
                    {
                        WordSubmmiting = true;
                        Dictionary<SearchBase, CancellationTokenSource> pairs = _factory.SubmitWork(Owner, ref words).Result;
                        Word提交队列 = new List<WordSearch>();
                        WordSubmmiting = false;
                        lock (Word执行列表)
                        {
                            foreach (KeyValuePair<SearchBase, CancellationTokenSource> pair in pairs)
                            {
                                Word执行列表.TryAdd(pair.Key, pair.Value);
                            }
                        }
                    }).Start();
                }
            }
            if (excels.Count != 0)
            {
                if (ExcelSubmitting)
                {
                    lock (Excel提交队列)
                    {
                        excels.ForEach(x =>
                        {
                            if (!Excel提交队列.Contains(x))
                            {
                                Excel提交队列.Add(x);
                            }
                        });
                    }
                }
                else
                {
                    new Task(() =>
                    {
                        ExcelSubmitting = true;
                        Dictionary<SearchBase, CancellationTokenSource> pairs = _factory.SubmitWork(Owner, ref words).Result;
                        Excel提交队列 = new List<ExcelSearch>();
                        ExcelSubmitting = false;
                        lock (Excel执行列表)
                        {
                            foreach (KeyValuePair<SearchBase, CancellationTokenSource> pair in pairs)
                            {
                                Excel执行列表.TryAdd(pair.Key, pair.Value);
                            }
                        }
                    }).Start();
                }
            }
            return Task.CompletedTask;
        }

        public void 重置工坊(进程模型 模型)
        {
            status = 进程状态.待重置;
            终止(模型);
            switch (模型)
            {
                case 进程模型.Excel:
                    while (ExcelSubmitting) { }
                    lock (任务列表)
                    {
                        任务列表.RemoveAll(x => 类型比对(模型, x.类型) && !x.状态.Equals(工作状态.处理中) && x.销毁());
                    }
                    break;
                case 进程模型.Word:
                    while (WordSubmmiting) { }
                    lock (任务列表)
                    {
                        任务列表.RemoveAll(x => 类型比对(模型, x.类型) && !x.状态.Equals(工作状态.处理中) && x.销毁());
                    }
                    break;
            }
            status = 进程状态.闲置;
        }
        public void 重置工坊()
        {
            status = 进程状态.待重置;
            终止();
            while (ExcelSubmitting || WordSubmmiting) { }
            lock (任务列表)
            {
                任务列表.RemoveAll(x => !x.状态.Equals(工作状态.处理中) && x.销毁());
            }
            status = 进程状态.闲置;
        }

        /// <summary>
        /// 返回结果文件路径，false则为有任务还未完成或类型不一致
        /// </summary>
        /// <param name="编号"></param>
        /// <param name="模型"></param>
        /// <returns></returns>
        public Task<string> 生成结果(List<int> 编号, 进程模型 模型)
        {
            List<SearchBase> searches = new List<SearchBase>();
            foreach (SearchBase search in 任务列表)
            {
                if (编号.Contains(search.编号))
                {
                    if (!search.状态.Equals(工作状态.完成) || !类型比对(模型, search.类型))
                    {
                        return Task.FromResult(false.ToString());
                    }
                    else
                    {
                        searches.Add(search);
                    }
                }
            }
            string ret = string.Empty;
            switch (模型)
            {
                case 进程模型.Word:
                    List<WordSearch> words = new List<WordSearch>();
                    searches.ForEach(x => words.Add((WordSearch)x));
                    string wordresp = 获取全路径(文件类型.Word);
                    ExcelUnit unit1 = _factory.NewExcel(所有者);
                    unit1.创建Excel(wordresp);
                    Logics.写入表(unit1, words);
                    unit1.关闭Excel();
                    _factory.ReturnUnit(所有者, unit1);
                    Word结果 = ret = 获取Src路径(文件类型.Word);
                    break;
                case 进程模型.Excel:
                    List<ExcelSearch> excels = new List<ExcelSearch>();
                    searches.ForEach(x => excels.Add((ExcelSearch)x));
                    string excelresp = 获取全路径(文件类型.Excel主表);
                    ExcelUnit unit = _factory.NewExcel(所有者);
                    unit.创建Excel(excelresp);
                    Logics.写入表(unit, excels);
                    unit.关闭Excel();
                    _factory.ReturnUnit(所有者, unit);
                    Excel结果 = ret = 获取Src路径(文件类型.Excel主表);
                    break;
            }
            return Task.FromResult(ret);
        }

        #region 内部方法
        /// <summary>
        /// 相对路径
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        private string 获取全路径(文件类型 类型)
        {
            string ret = string.Empty;
            switch (类型)
            {
                case 文件类型.Word:
                    ret = Path.GetFullPath(Path.Combine(所有者.用户wwwroot路径(), ResultsName[1] + 获取后缀(文件类型.Excel子表)[0]));
                    break;
                case 文件类型.Excel主表:
                case 文件类型.Excel子表:
                    ret = Path.GetFullPath(Path.Combine(所有者.用户wwwroot路径(), ResultsName[0] + 获取后缀(文件类型.Excel主表)[0]));
                    break;
            }
            return ret;
        }
        private string 获取Src路径(文件类型 类型)
        {
            string ret = string.Empty;
            switch (类型)
            {
                case 文件类型.Word:
                    ret = Path.Combine(所有者.用户Src路径(), ResultsName[1] + 获取后缀(文件类型.Excel主表)[0]);
                    break;
                case 文件类型.Excel主表:
                case 文件类型.Excel子表:
                    ret = Path.Combine(所有者.用户Src路径(), ResultsName[0] + 获取后缀(文件类型.Excel主表)[0]);
                    break;
            }
            return ret;
        }
        #endregion

        public ExcelSearch[] 获取Excel任务()
        {
            List<ExcelSearch> ret = new List<ExcelSearch>();
            string Mode = 模型转str(进程模型.Excel);
            foreach (SearchBase search in 任务列表)
            {
                if (search.GetType().Name.Equals(Mode))
                {
                    ret.Add((ExcelSearch)search);
                }
            }
            return ret.OrderByDescending(x => x.创建时间).ToArray();
        }

        public WordSearch[] 获取Word任务()
        {
            List<WordSearch> ret = new List<WordSearch>();
            string Mode = 模型转str(进程模型.Word);
            foreach (SearchBase search in 任务列表)
            {
                if (search.GetType().Name.Equals(Mode))
                {
                    ret.Add((WordSearch)search);
                }
            }
            return ret.OrderByDescending(x => x.创建时间).ToArray();
        }

        public List<PictureSearch> 获取识别任务()
        {
            return 识别提交队列;
        }
    }
}

