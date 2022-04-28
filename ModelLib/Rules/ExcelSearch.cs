using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.Logics;

namespace ModelLib
{
    public sealed class ExcelSearch : SearchBase 
    {
        public ExcelSearch(ExcelRule rule,SourceFile file) : base(file)
        {
            规则 = rule;
        }

        public readonly ExcelRule 规则;

        public ExcelResult Result = null;

        public Task<ExcelResult> 获取结果(ExcelUnit unit)
        {
            if ((Result == null && status==工作状态.创建) ||status==工作状态.失败)
            {
                if (File.Exists(源文件.获取物理路径()))
                {
                    try
                    {
                        切换状态(工作状态.处理中);
                        unit.打开Excel(源文件.获取物理路径());
                        unit.选择Sheet(规则.表单页 == 0 ? 1 : 规则.表单页);
                        Result = 读取表(unit, 规则);
                        切换状态(工作状态.完成);
                    }
                    catch
                    {
                        Result = null;
                        切换状态(工作状态.失败);
                    }
                    finally
                    {
                        unit.关闭Excel();
                    }
                }
            }
            return Task.FromResult(Result);
        }

        public ExcelResult 获取结果()
        {
            return Result;
        }
    }
}
