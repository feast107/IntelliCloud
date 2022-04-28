using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModelLib
{
    public abstract class UnitBase
    {
        protected enum 生命周期
        {
            托管,
            自管
        }

        /// <summary>
        /// 基础构造，填入生命周期和进程模型
        /// </summary>
        /// <param name="生命周期"></param>
        /// <param name="模型"></param>
        protected UnitBase(生命周期 生命周期, 进程模型 模型)
        {
            应用池.Add(this);
            进程计数[模型]++;
            LifeSpan = 生命周期;
            进程模型 = 模型;
            CreateTime = DateTime.Now;
        }
        
        /// <summary>
        /// 标识了容许进程的数量
        /// </summary>
        private static int 可用应用数 = Configuration.可用应用数;
        
        protected static readonly Dictionary<信号量, Semaphore> 信号量组 = Configuration.信号量组;
        
        private static readonly List<UnitBase> 应用池 = new  List<UnitBase>();

        protected static readonly Dictionary<进程模型, int> 进程计数 = new Dictionary<进程模型, int>() 
        {
            { 进程模型.Word,0 },
            { 进程模型.Excel,0 }
        };


        #region 进程控制
        /// <summary>
        /// 显式调用
        /// </summary>
        /// <param name="进程信号量"></param>
        /// <param name="进程计数"></param>
        /// <param name="工作信号量"></param>
        /// <param name="工作计数"></param>
        protected virtual void 创建进程(信号量 进程信号量,ref int 进程计数,信号量 工作信号量,ref int 工作计数)
        {
            if(LifeSpan == 生命周期.自管)
            {
                if (信号量组[信号量.可用应用].WaitOne())
                {
                    可用应用数--;
                }
                进程计数 = 信号量组[进程信号量].Release() + 1;
                工作计数 = 信号量组[工作信号量].Release() + 1;
                应用池.Add(this);
            }
        }
        protected virtual void 开始工作(信号量 工作信号量,ref int 工作计数)
        {
            if (status==进程状态.闲置 && LifeSpan == 生命周期.自管)
            {
                status = 进程状态.工作中;
                if (信号量组[工作信号量].WaitOne())
                {
                    工作计数--;
                }
            }
        }
        protected virtual void 结束工作(信号量 工作信号量,ref int 工作计数)
        {
            if (status == 进程状态.工作中 && LifeSpan == 生命周期.自管)
            {
                status = 进程状态.闲置;
                工作计数 = 信号量组[工作信号量].Release() + 1;
            }
        }
        protected virtual void 进程退出(信号量 进程信号量,ref int 进程计数,信号量 工作信号量,ref int 工作计数)
        {
            if (LifeSpan==生命周期.自管 && status == 进程状态.闲置)
            {
                应用池.Remove(this);
                status = 进程状态.回收;
                if (信号量组[工作信号量].WaitOne())
                {
                    工作计数--;
                }
                if (信号量组[进程信号量].WaitOne())
                {
                    进程计数--;
                }
                可用应用数 = 信号量组[信号量.可用应用].Release() + 1;
            }
        }

        public abstract void 创建进程();
        public abstract void 开始工作();
        public abstract void 结束工作();
        public abstract void 进程退出();
        #endregion

        #region 实例区
        
        public readonly 进程模型 进程模型;
        protected readonly 生命周期 LifeSpan;
        protected volatile 进程状态 status;
        protected readonly DateTime CreateTime;
        protected DateTime WorkTime;
        protected DateTime RestTime;

        #region Getter
        internal bool 闲置 => status.Equals(进程状态.闲置);
        internal DateTime 创建时间 => CreateTime;
        public DateTime 打开文本时间 => WorkTime;
        public DateTime 关闭文本时间 => RestTime;
        #endregion

        #endregion
        /// <summary>
        /// 回收不用的进程
        /// </summary>
        public static void 回收应用()
        {
            回收应用(0);
        }

        /// <summary>
        /// 回收到剩余数量的进程
        /// </summary>
        /// <param name="Num"></param>
        public static void 回收应用(int Num)
        {
            if (应用池.Count != 0 && 应用池.Count > Num)
            {
                lock (应用池)
                {
                    int num = 应用池.Count - Num;
                    foreach (ExcelUnit excel in 应用池)
                    {
                        if (excel.闲置)
                        {
                            excel.进程退出();
                            num--;
                        }
                        if (num == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void 打开文本()
        {
            WorkTime = DateTime.Now;
        }

        public void 关闭文本()
        {
            RestTime = DateTime.Now;
        }
    }
    
    public enum 信号量
    {
        可用应用,
        Excel应用,
        Word应用,
        Excel文档,
        Word文档
    }
    public enum 进程模型
    {
        Word,
        Excel,
        无适配
    }
}
