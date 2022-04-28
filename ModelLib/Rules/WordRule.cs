using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ModelLib.Logics;

namespace ModelLib
{
    public class WordRule : Rule
    {

        public WordRule(匹配模式 Mode, int 长度) : base(文件类型.Word)
        {
            编号 = 计数++;
            模式 = Mode;
            this.长度 = 长度;
        }

        private static int 计数 = 0;

        public readonly int 编号;

        public int 长度;

        public 匹配模式 模式;

        public 匹配规则 返回模式长度() { return 规则库[模式]; }

        public MatchResult 匹配(string str)
        {
            int length = 规则库[模式].最大长度;
            if (length != 0 && length <= str.Length)
            {
                return Word匹配(this, str,length);
            }
            if (长度 != 0)
            {
                return Word匹配(this, str, 长度);
            }
            return new MatchResult() { 匹配度 = 0 };
        }
    }
}
