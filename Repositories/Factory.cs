using ModelLib;
using Repositories.API;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories
{
    public class Factory : IFactory
    {
        private static readonly Dictionary<User, WorkLinq> ExcelTasks = new Dictionary<User, WorkLinq>();

        private static readonly Dictionary<User, WorkLinq> WordTasks = new Dictionary<User, WorkLinq>();

        public Factory()
        {

        }

        #region 接口区

        public Task<Dictionary<SearchBase,CancellationTokenSource>> SubmitWork(User user,ref List<ExcelSearch> searches)
        {
            //查找到工作
            if (ExcelTasks.TryGetValue(user, out WorkLinq linq))
            {
                lock (linq)
                {
                    if (!linq.status.Equals(工作状态.完成))
                    {
                        //中断任务
                        linq.中断器[WorkLinq.中断模式.延续].Cancel();
                        安排任务(user,searches,linq);
                    }
                }
                return Task.FromResult(linq.执行列表.ToDictionary(x =>x.Key,x=>x.Value.Value)) ;
            }
            //未查找到工作
            else
            {
                //等待会发生在此处，可以尝试提交
                WorkLinq work = 制作任务(user,ref searches,GetExcel().Result);
                lock (ExcelTasks)
                {
                    ExcelTasks.Add(user, work);
                }
                work.Start();
                return Task.FromResult(linq.执行列表.ToDictionary(x => x.Key, x => x.Value.Value));
            }
        }
        public Task<Dictionary<SearchBase, CancellationTokenSource>> SubmitWork(User user,ref List<WordSearch> searches)
        {
            //查找到工作
            if (WordTasks.TryGetValue(user, out WorkLinq linq))
            {
                lock (linq)
                {
                    if (!linq.status.Equals(工作状态.完成))
                    {
                        //中断任务
                        linq.中断器[WorkLinq.中断模式.延续].Cancel();
                        安排任务(user, searches, linq);
                    }
                }
                return Task.FromResult(linq.执行列表.ToDictionary(x => x.Key, x => x.Value.Value));
            }
            //未查找到工作
            else
            {
                //等待会发生在此处，可以尝试提交
                WorkLinq work = 制作任务(user,ref searches, GetDocx().Result);
                lock (WordTasks)
                {
                    WordTasks.Add(user, work);
                }
                work.Start();
                return Task.FromResult(linq.执行列表.ToDictionary(x => x.Key, x => x.Value.Value));
            }
        }
        public Task<Dictionary<SearchBase, CancellationTokenSource>> SubmitWork(User user, ref List<PictureSearch> searches)
        {
            //查找到工作
            if (WordTasks.TryGetValue(user, out WorkLinq linq))
            {
                lock (linq)
                {
                    if (!linq.status.Equals(工作状态.完成))
                    {
                        //中断任务
                        linq.中断器[WorkLinq.中断模式.延续].Cancel();
                        安排任务(user, searches, linq);
                    }
                }
                return Task.FromResult(linq.执行列表.ToDictionary(x => x.Key, x => x.Value.Value));
            }
            //未查找到工作
            else
            {
                //等待会发生在此处，可以尝试提交
                WorkLinq work = 制作任务(user, ref searches, GetDocx().Result);
                lock (WordTasks)
                {
                    WordTasks.Add(user, work);
                }
                work.Start();
                return Task.FromResult(linq.执行列表.ToDictionary(x => x.Key, x => x.Value.Value));
            }
        }
        public void Cancel(User user)
        {
            new Task(() =>
            {
                if (ExcelTasks.TryGetValue(user, out WorkLinq linq))
                {
                    linq.中断器[WorkLinq.中断模式.终止].Cancel();
                }
            }).Start();
            new Task(() =>
            {
                if (WordTasks.TryGetValue(user, out WorkLinq linq1))
                {
                    linq1.中断器[WorkLinq.中断模式.终止].Cancel();
                }
            }).Start();
        }
        public void Cancel(User user,进程模型 模型)
        {
            switch (模型)
            {
                case 进程模型.Word:
                    if (WordTasks.TryGetValue(user, out WorkLinq linq))
                    {
                        linq.中断器[WorkLinq.中断模式.终止].Cancel();
                    }
                    break;
                case 进程模型.Excel:
                    if (ExcelTasks.TryGetValue(user, out WorkLinq linq1))
                    {
                        linq1.中断器[WorkLinq.中断模式.终止].Cancel();
                    }
                    break;
            }
        }
        #endregion


        #region 私有区
        /// <summary>
        /// 一个任务对象，包含一组任务和相关的中断器
        /// </summary>
        private class WorkLinq
        {
            public WorkLinq(UnitBase unit, Dictionary<中断模式, CancellationTokenSource> Tokens)
            {
                工作单元 = unit;
                中断器 = Tokens;
                执行列表 = new List<KeyValuePair<SearchBase, KeyValuePair<Task, CancellationTokenSource>>>();
                status = 工作状态.创建;
            }

            public enum 中断模式
            {
                终止,
                延续
            }

            public readonly UnitBase 工作单元;

            /// <summary>
            /// 核心中断器  
            /// 调用终止会停止接下来的全部任务
            /// 调用延续会阻止回收，可以配置延续任务
            /// </summary>
            public readonly Dictionary<中断模式, CancellationTokenSource> 中断器;

            /// <summary>
            /// 任务/中断器
            /// </summary>
            public readonly List<KeyValuePair<SearchBase,KeyValuePair<Task,CancellationTokenSource>>> 执行列表;

            public 工作状态 status;

            public void Start()
            {
                if (执行列表.Count != 0 && 执行列表[0].Value.Key.Status == TaskStatus.Created )
                {
                    执行列表[0].Value.Key.Start();
                }
            }
        }

        #region 获取工作单元
        private static Task<DocxUnit> GetDocx()
        {
            return DocxUnit.GetInstance();
        }
        private static Task<ExcelUnit> GetExcel()
        {
            return ExcelUnit.GetInstance();
        }


        private static readonly Dictionary<User, DocxUnit> 用户Docx = new Dictionary<User, DocxUnit>();
        private static readonly Dictionary<User, ExcelUnit> 用户Excel = new Dictionary<User, ExcelUnit>();
        public DocxUnit NewDocx(User user)
        {
            if (用户Docx.TryGetValue(user, out DocxUnit docx))
            {
                return docx;
            }
            else
            {
                docx = DocxUnit.NewInstance();
                lock (用户Docx)
                {
                    用户Docx.Add(user, docx);
                }
            }
            return docx;
        }
        public ExcelUnit NewExcel(User user)
        {
            if (用户Excel.TryGetValue(user, out ExcelUnit excel))
            {
                return excel;
            }
            else
            {
                excel = ExcelUnit.NewInstance();
                lock (用户Docx)
                {
                    用户Excel.Add(user, excel);
                }
            }
            return excel;
        }
        public void ReturnUnit(User user,UnitBase unit)
        {
            switch (unit.进程模型)
            {
                case 进程模型.Word:
                    ((DocxUnit)unit).进程退出();
                    用户Docx.Remove(user);
                    break;
                case 进程模型.Excel:
                    ((ExcelUnit)unit).进程退出();
                    用户Excel.Remove(user);
                    break;
            }
        }

        #endregion

        private static WorkLinq 制作任务(User user,List<SearchBase> searches, UnitBase 工作单元)
        {
            CancellationTokenSource over = new CancellationTokenSource();
            CancellationTokenSource resume = new CancellationTokenSource();
            Dictionary<WorkLinq.中断模式, CancellationTokenSource> 中断器 = new Dictionary<WorkLinq.中断模式, CancellationTokenSource>()
            {
                { WorkLinq.中断模式.终止, over },
                { WorkLinq.中断模式.延续, resume }
            };
            WorkLinq Linq = new WorkLinq(工作单元,中断器);
            switch (工作单元.进程模型)
            {
                case 进程模型.Excel:
                    List<ExcelSearch> excels = new List<ExcelSearch>();
                    searches.ForEach(x => excels.Add((ExcelSearch)x));
                    安排任务(user, excels, Linq);
                    break;
                case 进程模型.Word:
                    List<WordSearch> words = new List<WordSearch>();
                    searches.ForEach(x => words.Add((WordSearch)x));
                    安排任务(user, words, Linq);
                    break;
            }
            return Linq;
        }
        private static WorkLinq 制作任务(User user,ref List<ExcelSearch> searches,UnitBase 工作单元)
        {
            List<SearchBase> bases = new List<SearchBase>();
            lock (searches)
            {
                searches.ForEach(x => bases.Add(x));
            }
            return 制作任务(user, bases, 工作单元);
        }
        private static WorkLinq 制作任务(User user,ref List<WordSearch> searches, UnitBase 工作单元)
        {
            List<SearchBase> bases = new List<SearchBase>();
            lock (searches)
            {
                searches.ForEach(x => bases.Add(x));
            }
            return 制作任务(user, bases,工作单元);
        }
        private static WorkLinq 制作任务(User user, ref List<PictureSearch> searches, UnitBase 工作单元)
        {
            CancellationTokenSource over = new CancellationTokenSource();
            CancellationTokenSource resume = new CancellationTokenSource();
            Dictionary<WorkLinq.中断模式, CancellationTokenSource> 中断器 = new Dictionary<WorkLinq.中断模式, CancellationTokenSource>()
            {
                { WorkLinq.中断模式.终止, over },
                { WorkLinq.中断模式.延续, resume }
            };
            WorkLinq Linq = new WorkLinq(工作单元, 中断器);
            List<PictureSearch> words = new List<PictureSearch>();
            searches.ForEach(x => words.Add(x));
            安排任务(user, words, Linq);
            return Linq;
        }

        #region 封装任务
        /// <summary>
        /// 生成Word队列
        /// </summary>
        /// <param name="user"></param>
        /// <param name="searches"></param>
        /// <param name="Linq"></param>
        private static void 安排任务(User user,List<WordSearch> searches,WorkLinq Linq)
        {
            Task t = null;
            CancellationTokenSource over = Linq.中断器[WorkLinq.中断模式.终止];
            CancellationTokenSource resume = Linq.中断器[WorkLinq.中断模式.延续];
            foreach (WordSearch search in searches)
            {
                //安排任务
                search.计划();
                //私有中断
                CancellationTokenSource source = new CancellationTokenSource();
                if (Linq.执行列表.Count == 0)
                {
                    //首个任务
                    t = new Task(() => 
                    {
                        Linq.status = 工作状态.处理中;
                        over.Token.ThrowIfCancellationRequested();
                        source.Token.ThrowIfCancellationRequested();
                        source.Dispose();
                        search.获取结果((DocxUnit)Linq.工作单元);
                    }, over.Token);
                }
                else
                {
                    if (t == null)
                    {
                        t = Linq.执行列表[^1].Value.Key;
                    }
                    //延续任务
                    t = t.ContinueWith((T) =>
                    {
                        over.Token.ThrowIfCancellationRequested();
                        source.Token.ThrowIfCancellationRequested();
                        source.Dispose();
                        search.获取结果((DocxUnit)Linq.工作单元);
                    }, over.Token);
                }
                Linq.执行列表.Add(item: new  KeyValuePair<SearchBase, KeyValuePair<Task, CancellationTokenSource>>(search,new KeyValuePair<Task, CancellationTokenSource>( t, source))); 
            }
            //制作终止任务
            if (Linq.执行列表.Count != 0)
            {
                Task FINAL = t.ContinueWith((T) =>
                {
                    //延续中断
                    resume.Token.ThrowIfCancellationRequested();
                    lock (Linq)
                    {
                        resume.Token.ThrowIfCancellationRequested();
                        //释放资源
                        resume.Dispose();
                        over.Dispose();
                        ((DocxUnit)Linq.工作单元).结束工作();
                        WordTasks.Remove(user);
                        Linq.执行列表.RemoveRange(0, Linq.执行列表.Count);
                        Linq.status = 工作状态.完成;
                    }
                }, resume.Token);
            }
        }
        /// <summary>
        /// 生成Excel队列
        /// </summary>
        /// <param name="user"></param>
        /// <param name="searches"></param>
        /// <param name="Linq"></param>
        private static void 安排任务(User user,List<ExcelSearch> searches,WorkLinq Linq)
        {
            Task t = null;
            CancellationTokenSource over = Linq.中断器[WorkLinq.中断模式.终止];
            CancellationTokenSource resume = Linq.中断器[WorkLinq.中断模式.延续];
            foreach (ExcelSearch search in searches)
            {
                //安排任务
                search.计划();
                //私有中断
                CancellationTokenSource source = new CancellationTokenSource();
                if (Linq.执行列表.Count == 0)
                {
                    //首个任务
                    t = new Task(() =>
                    {
                        Linq.status = 工作状态.处理中;
                        over.Token.ThrowIfCancellationRequested();
                        source.Token.ThrowIfCancellationRequested();
                        source.Dispose();
                        search.获取结果((ExcelUnit)Linq.工作单元);
                        
                    }, over.Token);
                }
                else
                {
                    if (t == null)
                    {
                        t = Linq.执行列表[^1].Value.Key;
                    }
                    //延续任务
                    t = t.ContinueWith((T) =>
                    {
                        over.Token.ThrowIfCancellationRequested();
                        source.Token.ThrowIfCancellationRequested();
                        source.Dispose();
                        search.获取结果((ExcelUnit)Linq.工作单元);
                    }, over.Token);
                }
                Linq.执行列表.Add(item: new KeyValuePair<SearchBase, KeyValuePair<Task, CancellationTokenSource>>( search,new KeyValuePair<Task, CancellationTokenSource>(t, source))); ;
            }
            //制作终止任务
            if (Linq.执行列表.Count != 0)
            {
                Task FINAL = t.ContinueWith((T) =>
                {
                    //延续中断
                    resume.Token.ThrowIfCancellationRequested();
                    lock (Linq)
                    {
                        resume.Token.ThrowIfCancellationRequested();
                        //释放资源
                        resume.Dispose();
                        over.Dispose();
                        ((ExcelUnit)Linq.工作单元).结束工作();
                        ExcelTasks.Remove(user);
                        Linq.执行列表.RemoveRange(0,Linq.执行列表.Count);
                        Linq.status = 工作状态.完成;
                    }
                }, resume.Token);
            }
        }

        private static void 安排任务(User user, List<PictureSearch> searches, WorkLinq Linq)
        {
            Task t = null;
            CancellationTokenSource over = Linq.中断器[WorkLinq.中断模式.终止];
            CancellationTokenSource resume = Linq.中断器[WorkLinq.中断模式.延续];
            foreach (PictureSearch search in searches)
            {
                //安排任务
                search.计划();
                //私有中断
                CancellationTokenSource source = new CancellationTokenSource();
                if (Linq.执行列表.Count == 0)
                {
                    //首个任务
                    t = new Task(() =>
                    {
                        Linq.status = 工作状态.处理中;
                        over.Token.ThrowIfCancellationRequested();
                        source.Token.ThrowIfCancellationRequested();
                        source.Dispose();
                        search.获取结果((DocxUnit)Linq.工作单元);

                    }, over.Token);
                }
                else
                {
                    if (t == null)
                    {
                        t = Linq.执行列表[^1].Value.Key;
                    }
                    //延续任务
                    t = t.ContinueWith((T) =>
                    {
                        over.Token.ThrowIfCancellationRequested();
                        source.Token.ThrowIfCancellationRequested();
                        source.Dispose();
                        search.获取结果((DocxUnit)Linq.工作单元);
                    }, over.Token);
                }
                Linq.执行列表.Add(item: new KeyValuePair<SearchBase, KeyValuePair<Task, CancellationTokenSource>>(search, new KeyValuePair<Task, CancellationTokenSource>(t, source))); ;
            }
            //制作终止任务
            if (Linq.执行列表.Count != 0)
            {
                Task FINAL = t.ContinueWith((T) =>
                {
                    //延续中断
                    resume.Token.ThrowIfCancellationRequested();
                    lock (Linq)
                    {
                        resume.Token.ThrowIfCancellationRequested();
                        //释放资源
                        resume.Dispose();
                        over.Dispose();
                        ((DocxUnit)Linq.工作单元).结束工作();
                        ExcelTasks.Remove(user);
                        Linq.执行列表.RemoveRange(0, Linq.执行列表.Count);
                        Linq.status = 工作状态.完成;
                    }
                }, resume.Token);
            }
        }
        #endregion

        #endregion
    }

} 
