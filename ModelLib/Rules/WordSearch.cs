using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.Logics;

namespace ModelLib
{
    public sealed class WordSearch :SearchBase
    {
        public WordSearch(Dictionary<string,List<WordRule>>目标,SourceFile 源文件) :base(源文件)
        {
            this.目标 = 目标;
        }

        public readonly Dictionary<string, List<WordRule>> 目标;

        private Dictionary<string, MatchResult> Result = null;

        public Task<Dictionary<string,MatchResult>> 获取结果(DocxUnit unit)
        {
            if ((Result == null && status == 工作状态.创建) || status == 工作状态.失败)
            {
                if (File.Exists(源文件.获取物理路径()))
                {
                    try 
                    {
                        切换状态(工作状态.处理中);
                        Result = new Dictionary<string, MatchResult>();
                        unit.打开Doc(源文件.获取物理路径());
                        Result = Logics.获取结果(unit, 目标);
                        切换状态(工作状态.完成);
                    }
                    catch
                    {
                        Result = null;
                        切换状态(工作状态.失败);
                    }
                    finally
                    {
                        unit.关闭Doc();
                    }
                }
            }
            return Task.FromResult(Result);
        }

        public Dictionary<string, MatchResult> 获取结果()
        {
            return Result;
        }

    }
}
