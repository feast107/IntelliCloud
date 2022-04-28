using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.Logics;

namespace ModelLib 
{ 
    public class ExcelRule : Rule
    {
        private static int 计数 = 0;

        public ExcelRule(文件类型 类型) : base(类型)
        {
            编号 = 计数++;
        }

        public readonly int 编号;

        public bool 是否主表 = false;

        public int 表单页 = 0;

        public int 标题起始行 = 0;

        public int 标题结束行 = 0;

        public int 数据起始行=0;

        public int 数据结束行=0;

        public int 起始列=0;

        public int 结束列=0;

        public int[] 主键列 ;


        public enum 参数类型
        {
            表单页,
            标题起始行,
            标题结束行,
            数据起始行,
            数据结束行,
            起始列,
            结束列,
            主键列
        }
    }
}
