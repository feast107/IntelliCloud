using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public abstract class SearchBase :Logics
    {
        public static List<SearchBase> Searches=new List<SearchBase>();

        public SearchBase(SourceFile souce)
        {
            创建时间 = DateTime.Now;
            源文件 = souce;
            编号 = Searches.Count;
            Searches.Add(this);
            切换状态(工作状态.创建);
        }

        /// <summary>
        /// 在当前单元安排进任何任务或者线程的时候请调用此方法，以保证能将当前单元的状态时刻通知
        /// </summary>
        public void 计划()
        {
            if (status == 工作状态.创建 || status == 工作状态.失败)
            {
                切换状态(工作状态.待处理);
            }
        }

        public 文件类型 类型 => 源文件.类型;


        protected DateTime switchTime;
        protected DateTime 切换状态时间 => switchTime;
        protected void 切换状态(工作状态 状态)
        {
            status = 状态;
            switchTime = DateTime.Now;
        }

        public readonly DateTime 创建时间;

        public readonly int 编号;

        public int num => 编号;

        public readonly SourceFile 源文件;

        protected volatile 工作状态 status;

        public 工作状态 状态 => status;

        public virtual bool 销毁()
        {
            if (status.Equals(工作状态.处理中))
            {
                Searches.Remove(this);
                return 源文件.删除文件(); ;
            }
            else
            {
                return false;
            }
        }
    }
}
