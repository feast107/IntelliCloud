using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static ModelLib.Logics.ExcelResult;

namespace ModelLib
{
    public abstract class Logics
    {
        #region 匹配区
        private static MatchResult 精确匹配(string str1,string str2)
        {
            if (str1.Length == str2.Length)
            {
                if (str1.Equals(str2))
                {
                    return new MatchResult() { 匹配度 = 1f, 起始位 = 0,结束位=str1.Length, 匹配结果=str1 };
                }
            }
            if (str1.Length > str2.Length)
            {
                交换(ref str1, ref str2);
            }
            for(int i = 0; i <= str2.Length - str1.Length;i++)
            {
                if (str2.Substring(i, str1.Length).Equals(str1))
                {
                    return new MatchResult() { 匹配度 = 1f, 起始位 = i, 结束位 = i + str1.Length , 匹配结果=str1 };
                }
            }
            return new MatchResult() { 匹配度 = 0, 起始位 = 0 };
        }
        private static MatchResult 模糊匹配(string str1,string str2)
        {
            if (str1.Length == str2.Length)
            {
                if (str1.Equals(str2))
                {
                    return new MatchResult() { 匹配度 = 1f, 起始位 = 0, 结束位=str1.Length , 匹配结果=str1 };
                }
            }
            if (str1.Length > str2.Length)
            {
                交换(ref str1, ref str2);
            }
            int i = 0;
            int corr = 0;
            int wron = 0;
            int startindex = 0;
            while (i <= str2.Length - str1.Length+1)
            {
                if (str2[i].Equals(str1[corr]))
                {
                    corr++;
                    if (corr == 1 || str2[i].Equals(str1[0]))
                    {
                        startindex = i;
                    }
                }
                else
                {
                    wron++;
                }
                if (corr.Equals(str1.Length))
                {
                    return new MatchResult() { 匹配度 = corr / str1.Length, 起始位 = startindex , 结束位=i+startindex , 匹配结果=str1 };
                }
                i++;
            }
            return new MatchResult() { 匹配度 = corr / str1.Length, 起始位 =startindex ,结束位=str2.Length, 匹配结果=str1.Substring(0,corr) };
        }
        private static bool 正则匹配(string str,string Express)
        {
            return Regex.IsMatch(str, Express);
        }

        protected static MatchResult Word匹配(WordRule rule,string str,int length)
        {
            for (int index = 0; index <= str.Length - length; index++)
            {
                if (正则匹配(str.Substring(index, length),规则库[rule.模式].表达式))
                {
                    return new MatchResult() { 起始位 = index, 结束位 =index + length, 匹配度 = 1, 匹配结果 = str.Substring(index, length) };
                }
            }
            return new MatchResult() { 匹配度 = 0 };
        }
        protected static Dictionary<string, MatchResult> 获取结果(DocxUnit unit, Dictionary<string, List<WordRule>> 目标)
        {
            Dictionary<string, MatchResult> ret = new Dictionary<string, MatchResult>();
            int ParagC = unit.ParagCount;
            foreach (KeyValuePair<string, List<WordRule>> pair in 目标)
            {
                for (int p = 1; p <= ParagC; p++)
                {
                    string txt = unit.获取段落文本(p);
                    MatchResult prev = 模糊匹配(txt, pair.Key);
                    if (prev.匹配度 == 0)
                    {
                        continue;
                    }
                    //在此添加喜好，动态相关
                    int locate = prev.结束位 + 1;
                    MatchResult result = new MatchResult() { };
                    foreach (WordRule rule in pair.Value)
                    {
                        MatchResult res = rule.匹配(txt[locate..]);
                        if (res.匹配度 != 0)
                        {
                            if (result.起始位 == 0)
                            {
                                result.起始位 = res.起始位;
                            }
                            result.匹配度 += res.匹配度;
                            result.结束位 = locate += res.结束位;
                            result.匹配结果 += res.匹配结果;
                        }
                    }
                    result.匹配度 /= pair.Value.Count;
                    ret.TryAdd(pair.Key, result);
                    break;
                }

            }
            return ret;
        }
        protected static Dictionary<string, MatchResult> 获取结果(DocxUnit unit, Dictionary<string,List<WordRule>> 目标,CancellationToken token)
        {
            Dictionary<string, MatchResult> ret = new Dictionary<string, MatchResult>();
            int ParagC = unit.ParagCount;
            foreach (KeyValuePair<string, List<WordRule>> pair in 目标)
            {
                for (int p = 1; p <= ParagC; p++)
                {
                    token.ThrowIfCancellationRequested();
                    string txt = unit.获取段落文本(p);
                    MatchResult prev = 模糊匹配(txt, pair.Key);
                    if (prev.匹配度 == 0)
                    {
                        continue;
                    }
                    //在此添加喜好，动态相关
                    int locate = prev.结束位 + 1;
                    MatchResult result = new MatchResult() { };
                    foreach (WordRule rule in pair.Value)
                    {
                        MatchResult res = rule.匹配(txt[locate..]);
                        if (res.匹配度 != 0)
                        {
                            if (result.起始位 == 0)
                            {
                                result.起始位 = res.起始位;
                            }
                            result.匹配度 += res.匹配度;
                            result.结束位 = locate += res.结束位;
                            result.匹配结果 += res.匹配结果;
                        }
                    }
                    result.匹配度 /= pair.Value.Count;
                    ret.TryAdd(pair.Key, result);
                    break;
                }
                
            }return ret;
        }
        protected static Dictionary<string, MatchResult> 获取结果(DocxUnit unit, Dictionary<string, List<WordRule>> 目标,工作进度 Progress , CancellationToken token)
        {
            Dictionary<string, MatchResult> ret = new Dictionary<string, MatchResult>();
            int ParagC = unit.ParagCount;
            float probase = 3f * 100f /(float) 目标.Count;
            float leastbase = probase / (float)ParagC;
            foreach (KeyValuePair<string, List<WordRule>> pair in 目标)
            {
                float now = 0;
                for (int p = 1; p <= ParagC; p++)
                {
                    token.ThrowIfCancellationRequested();
                    string txt = unit.获取段落文本(p);
                    MatchResult prev = 模糊匹配(txt, pair.Key);
                    if (prev.匹配度 == 0)
                    {
                        now += leastbase;
                        Progress.更新(工作状态.处理中, leastbase);
                        continue;
                    }
                    //在此添加喜好，动态相关
                    int locate = prev.结束位 + 1;
                    MatchResult result = new MatchResult() { };
                    foreach (WordRule rule in pair.Value)
                    {
                        MatchResult res = rule.匹配(txt[locate..]);
                        if (res.匹配度 != 0)
                        {
                            if (result.起始位 == 0)
                            {
                                result.起始位 = res.起始位;
                            }
                            result.匹配度 += res.匹配度;
                            result.结束位 = locate += res.结束位;
                            result.匹配结果 += res.匹配结果;
                        }
                    }
                    result.匹配度 /= pair.Value.Count;
                    ret.TryAdd(pair.Key, result);

                    Progress.更新(工作状态.处理中, probase-now);
                    break;
                }

            }
            return ret;
        }
    
        /// <summary>
        /// 匹配结果
        /// </summary>
        public class MatchResult
        {
            /// <summary>
            /// 目标字符和源字符的匹配值，于0到1之间的一个数
            /// </summary>
            public float 匹配度;

            /// <summary>
            /// 开始匹配的第一个字符
            /// </summary>
            public int 起始位;

            /// <summary>
            /// 检索的最后一个字符位
            /// </summary>
            public int 结束位;

            public string 匹配结果;
        }
        #endregion

        #region Word规则库
        public enum 匹配模式
        {
            任意字符,
            数字,
            非数字,
            符号,
            中文,
            全部英文字母,
            大写英文字母,
            小写英文字母,
            手机电话号码,
            空白,
            制表符,
            腾讯QQ号,
            中国邮政编码,
            身份证号,
            IPv4地址,
            IPv6地址,
            标准金额,
            日期
        }
        public static string[] 获取匹配模式()
        {
            return Enum.GetNames(typeof(匹配模式));
        }

        public struct 匹配规则
        {
            public bool 是否定长;

            public int 最小长度;

            public int 最大长度;

            public string 表达式;
        }

        public readonly static Dictionary<匹配模式, 匹配规则> 规则库 = new  Dictionary<匹配模式, 匹配规则>()
        {
            {匹配模式.数字,new 匹配规则(){是否定长 = false , 表达式 = "^\\d*$" } },
            {匹配模式.非数字,new 匹配规则(){是否定长 = false ,表达式 = "^\\D*$" } },
            {匹配模式.符号,new 匹配规则() { 是否定长 = false, 表达式 = "[^^\\W*$" } },
            {匹配模式.中文,new 匹配规则() { 是否定长 = false, 表达式 = "^[\u4e00-\u9fa5]{0,}$" } },
            {匹配模式.全部英文字母,new  匹配规则() { 是否定长 = false, 表达式 = "^[A-Za-z]+$" }},
            {匹配模式.大写英文字母,new  匹配规则() { 是否定长 = false, 表达式 = "^[A-Z]+$" }},
            {匹配模式.小写英文字母,new  匹配规则() { 是否定长 = false, 表达式 = "^[a-z]+$" }},
            {匹配模式.空白,new 匹配规则() { 是否定长 = false, 表达式 = "^\\s*|\\s*$" } },
            {匹配模式.制表符,new 匹配规则() { 是否定长 = false, 表达式 = "\r\a" } },
            {匹配模式.腾讯QQ号,new 匹配规则() { 是否定长 = true,最小长度=5,最大长度=11, 表达式 = "[1-9][0-9]{4,}" } },
            {匹配模式.中国邮政编码,new  匹配规则() { 是否定长 = true,最小长度=6,最大长度=6, 表达式 = "[1-9][0-9]{5}(?![0-9])" } },
            {匹配模式.身份证号,new 匹配规则() { 是否定长 = true,最小长度=15,最大长度=18, 表达式 = "^[0-9]{15}|[0-9]{18}" } },
            {匹配模式.任意字符,new 匹配规则() { 是否定长 = false, 表达式 = "^\\S*$" } },
            {匹配模式.IPv4地址,new 匹配规则() { 是否定长 = true,最小长度=7,最大长度=15, 表达式 = "\\d+\\.\\d+\\.\\d+\\.\\d+" } },
            {匹配模式.IPv6地址,new 匹配规则() { 是否定长 = true ,表达式 = "(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|" +
                "([0-9a-fA-F]{1,4}:)" +
                "{1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|" +
                "([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|" +
                "([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|" +
                "([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|" +
                "([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|" +
                "[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|" +
                ":((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|" +
                "::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|" +
                "1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|" +
                "([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|" +
                "(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))" ,最小长度=46,最大长度=46} },
            {匹配模式.日期,new 匹配规则() { 是否定长 = true ,最小长度=10,最大长度=11, 表达式 = "^\\d{4}[-|年|\\.]\\d{1,2}[-|月|\\.]\\d{1,2}日*" } },
            {匹配模式.标准金额,new 匹配规则() { 是否定长 = false, 表达式 = "^([0-9]+|[0-9]{1,3}(,[0-9]{3})*)(.[0-9]{1,2})?$" } }


        };

        public readonly static List<string> 常见表项 = new List<string>()
        {
            "姓名","序号","编号","性别","年龄","部门","职位","岗位","电话","号码","工号","学号","日期","情况"
        };

        public readonly static List<string> 省 = new List<string>()
        {
            "江苏","湖北","上海","北京","山东","山西","湖南","河南","河北","黑龙江","福建","浙江","重庆","四川","广西","广东","陕西","江西","天津","辽宁","吉林","内蒙古","贵州","云南"
        };
        #endregion

        /// <summary>
        /// 坐标值类
        /// </summary>
        public class 坐标值
        {
            public 坐标值(int row, int col, string title)
            {
                行 = row;
                列 = col;
                值 = title;
            }
            public readonly int 行;
            public readonly int 列;
            public readonly string 值;
        }


        #region Excel读取区

        /// <summary>
        /// Excel结果类
        /// </summary>
        public class ExcelResult
        {
            public ExcelResult(List<坐标值> 标题, Dictionary<主键, Hashtable> 行值对)
            {
                this.标题 = 标题;
                this.行值对 = 行值对;
            }

            /// <summary>
            /// 坐标 : 标题内容
            /// </summary>
            public readonly List<坐标值> 标题;
            /// <summary>
            /// 行 : 值
            /// </summary>
            public readonly Dictionary<主键, Hashtable> 行值对;

            public class 主键
            {
                /// <summary>
                /// 行列值
                /// </summary>
                public 主键(List<string> keys)
                {
                    层级主键 = keys;
                }
                public readonly List<string> 层级主键;

                public class 主键对比 : IEqualityComparer<主键>
                {
                    /// <summary>
                    /// 逐级匹配是否为同一个主键
                    /// </summary>
                    /// <param name="Key1"></param>
                    /// <param name="Key2"></param>
                    /// <returns></returns>
                    public bool Equals(主键 Key1, 主键 Key2)
                    {
                        int Maximun = Key1.层级主键.Count < Key2.层级主键.Count ? Key1.层级主键.Count : Key2.层级主键.Count;
                        int i = 0;
                        while (i < Maximun)
                        {
                            if (!Key1.层级主键[i].Equals(Key2.层级主键[i]))
                            {
                                return false;
                            }
                            i++;
                        }
                        return true;
                    }

                    public int GetHashCode([DisallowNull] 主键 obj)
                    {
                        return obj.GetHashCode();
                    }
                }

            }

            /// <summary>
            /// 查找某一列的标题
            /// </summary>
            /// <param name="标题"></param>
            /// <param name="列"></param>
            /// <returns></returns>
            public static string 获取某列的标题(List<坐标值> 标题, int 列)
            {
                int row = 0;
                string ret = string.Empty;
                foreach (坐标值 pair in 标题)
                {
                    if (pair.列.Equals(列))
                    {
                        if (row < pair.行)
                        {
                            row = pair.行;
                            ret = pair.值;
                        }
                    }
                }
                return ret;
            }

            public static Dictionary<int, string> 获取某行的所有标题(List<坐标值> 标题, int 行)
            {
                Dictionary<int, string> pairs = new Dictionary<int, string>();
                foreach (坐标值 pair in 标题)
                {
                    if (行.Equals(pair.行))
                    {
                        pairs.TryAdd(pair.列, pair.值);
                    }
                }
                return pairs;
            }
            /// <summary>
            /// 列号 / 值
            /// </summary>
            /// <param name="标题"></param>
            /// <returns></returns>
            public static Dictionary<int, string> 获取直接标题(List<坐标值> 标题)
            {
                坐标值[] strs = new 坐标值[获取坐标最值(标题, 最值.最大列)];
                Dictionary<int, string> ret = new Dictionary<int, string>();
                标题.ForEach(x =>
                {
                    if (strs[x.列 - 1] == null)
                    {
                        strs[x.列 - 1] = x;
                        ret.TryAdd(x.列, x.值);
                    }
                    else if (x.行 > strs[x.列 - 1].行)
                    {
                        strs[x.列 - 1] = x;
                        ret[x.列] = x.值;
                    }
                });
                return ret;
            }

        }



        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        protected static ExcelResult 读取表(ExcelUnit excel, ExcelRule rule,文件类型 类型)
        {
            string[,] 数据 = 转化为矩阵(excel);
            List<坐标值> 标题 = 获取标题(数据);
            校对坐标(ref 数据, rule, 标题);
            Dictionary<主键, Hashtable> 值 = 获取值(数据, 标题, rule,类型);
            return new ExcelResult(标题,值);
        }
        /// <summary>
        /// Progress步进三次
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="rule"></param>
        /// <param name="类型"></param>
        /// <param name="Progress"></param>
        /// <returns></returns>
        protected static ExcelResult 读取表(ExcelUnit excel, ExcelRule rule, 文件类型 类型,工作进度 Progress)
        {
            string[,] 数据 = 转化为矩阵(excel,Progress);
            List<坐标值> 标题 = 获取标题(数据, Progress);
            校对坐标(ref 数据,  rule, 标题);
            Dictionary<主键, Hashtable> 值 = 获取值(数据, 标题, rule, 类型, Progress);
            return new ExcelResult(标题, 值);
        }
        protected static ExcelResult 读取表(ExcelUnit excel, ExcelRule rule, 文件类型 类型, 工作进度 Progress,CancellationToken 中断令牌)
        {
            中断令牌.ThrowIfCancellationRequested();
            string[,] 数据 = 转化为矩阵(excel, Progress,中断令牌);
            中断令牌.ThrowIfCancellationRequested();
            List<坐标值> 标题 = 获取标题(数据, Progress);
            中断令牌.ThrowIfCancellationRequested();
            校对坐标(ref 数据, rule, 标题);
            中断令牌.ThrowIfCancellationRequested();
            Dictionary<主键, Hashtable> 值 = 获取值(数据, 标题, rule, 类型, Progress);
            中断令牌.ThrowIfCancellationRequested();
            return new ExcelResult(标题, 值);
        }
        #region 私有
        /// <summary>
        /// 坐标对应模式
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static string[,] 转化为矩阵(ExcelUnit unit)
        {
            int rowMax = unit.RowCount;
            for (; rowMax > 0; rowMax--)
            {
                int col = 1;
                while (unit.读取(rowMax, col).Trim() == string.Empty && col<=unit.ColCount)
                {
                    col++;
                }
                if (col <= unit.ColCount)
                {
                    break;
                }
            }
            string[,] ret = new string[rowMax,unit.ColCount];
            for(int r=1; r <= rowMax; r++)
            {
                for(int c = 1; c <= unit.ColCount; c++)
                {
                    ret[r-1, c-1] = unit.读取(r , c );
                }
            }
            return ret;
        }
        private static string[,] 转化为矩阵(ExcelUnit unit,工作进度 Progress)
        {
            float deg = 100f / (unit.RowCount+1);
            int rowMax = unit.RowCount;
            for (; rowMax > 0; rowMax--)
            {
                int col = 1;
                while (unit.读取(rowMax, col).Trim() == string.Empty && col <= unit.ColCount)
                {
                    col++;
                }
                if (col <= unit.ColCount)
                {
                    break;
                }
                Progress.更新(工作状态.处理中, deg);
            }
            string[,] ret = new string[rowMax, unit.ColCount];
            for (int r = 1; r <= rowMax; r++)
            {
                for (int c = 1; c <= unit.ColCount; c++)
                {
                    ret[r - 1, c - 1] = unit.读取(r, c);
                    
                }
                Progress.更新( 工作状态.处理中,deg);
            }
            return ret;
        }
        private static string[,] 转化为矩阵(ExcelUnit unit, 工作进度 Progress,CancellationToken 中断令牌)
        {
            float deg = 100f / (unit.RowCount + 1);
            int rowMax = unit.RowCount;
            for (; rowMax > 0; rowMax--)
            {
                中断令牌.ThrowIfCancellationRequested();
                int col = 1;
                while (unit.读取(rowMax, col).Trim() == string.Empty && col <= unit.ColCount)
                {
                    col++;
                }
                if (col <= unit.ColCount)
                {
                    break;
                }
                Progress.更新(工作状态.处理中, deg);
            }
            string[,] ret = new string[rowMax, unit.ColCount];
            for (int r = 1; r <= rowMax; r++)
            {
                for (int c = 1; c <= unit.ColCount; c++)
                {
                    中断令牌.ThrowIfCancellationRequested();
                    ret[r - 1, c - 1] = unit.读取(r, c);
                    
                }
                Progress.更新(工作状态.处理中, deg);
            }
            return ret;
        }
        /// <summary>
        /// 坐标对应模式
        /// </summary>
        /// <param name="Values"></param>
        /// <param name="unit"></param>
        private static void 矩阵写入表(string[,] Values,ExcelUnit unit)
        {
            for (int r = 0; r < Values.GetLongLength(0); r++)
            {
                for (int c = 0; c < Values.GetLongLength(1); c++)
                {
                    unit.写入(r + 1, c + 1, Values[r, c] ?? string.Empty);
                }
            }
        }
        /// <summary>
        /// 匹配并修正表格相关值的坐标
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="rule"></param>
        /// <param name="标题"></param>
        public static void 校对坐标(ref string[,] 数据,ExcelRule rule, List<坐标值> 标题)
        {
            if (rule.标题起始行 == 0)
            {
                rule.标题起始行 = 获取坐标最值(标题, 最值.最小行);
            }
            if (rule.标题结束行 == 0)
            {
                rule.标题结束行 = 获取坐标最值(标题, 最值.最大行);
            }
            if (rule.起始列 == 0)
            {
                rule.起始列 = 获取坐标最值(标题, 最值.最小列);
            }
            if (rule.结束列 == 0)
            {
                rule.结束列 = 获取坐标最值(标题, 最值.最大列);
            }
            if (rule.数据起始行 == 0 || rule.数据起始行>(int) 数据.GetLongLength(0))
            {
                rule.数据起始行 = rule.标题结束行 + 1;
            }
            if (rule.数据结束行 == 0 || rule.数据结束行>(int)数据.GetLongLength(0))
            {
                rule.数据结束行 = (int)数据.GetLongLength(0);
            }

        }

        /// <summary>
        /// 识别标题
        /// </summary>
        /// <param name="数据"></param>
        /// <returns></returns>
        private static List<坐标值> 获取标题(string[,] 数据)
        {
            List<坐标值> ret = new List<坐标值>();
            List<string> 标题组 = new List<string>();
            //行遍历
            for (int r = 0; r < 数据.GetLongLength(0); r++)
            {
                List<坐标值> tmp = new List<坐标值>();
                List<string> 行标题 = new List<string>();
                bool flag = false;
                 int empty = 0;
                //列遍历
                for (int c = 0; c < 数据.GetLongLength(1); c++)
                {
                    string 值 = 数据[r, c].Trim();
                    if ( 值!=string.Empty)
                    {
                        empty = 0;
                        //判断是否为数据项
                        if(判断数据项(值))
                        {
                            flag = true;
                            break;
                        }
                        if (!行标题.Contains(值) && !标题组.Contains(值))
                        {
                            行标题.Add(值);
                            tmp.Add(new 坐标值(r + 1, c + 1, 值));
                        }
                    }else
                    {
                        //empty++;
                        //if (empty >= 2)
                        //{
                        //    flag = true;
                        //    break;
                        //}
                    }
                }
                if (flag)
                {
                    break;
                }
                ret.AddRange(tmp);
                标题组.AddRange(行标题);
            }
            return ret;
        }
        private static List<坐标值> 获取标题(string[,] 数据, 工作进度 Progress)
        {
            List<坐标值> ret = new List<坐标值>();
            List<string> 标题组 = new List<string>();
            //行遍历
            float basic = 100f;
            float deg = 100f / (数据.GetLongLength(0) + 1);

            for (int r = 0; r < 数据.GetLongLength(0); r++)
            {
                List<坐标值> tmp = new List<坐标值>();
                List<string> 行标题 = new List<string>();
                bool flag = false;
                int empty = 0;
                //列遍历
                for (int c = 0; c < 数据.GetLongLength(1); c++)
                {
                    string 值 = 数据[r, c].Trim();
                    if (值 != string.Empty)
                    {
                        empty = 0;
                        //判断是否为数据项
                        if (判断数据项(值))
                        {
                            flag = true;
                            break;
                        }
                        if (!行标题.Contains(值) && !标题组.Contains(值))
                        {
                            行标题.Add(值);
                            tmp.Add(new 坐标值(r + 1, c + 1, 值));
                        }
                    }
                    else
                    {
                        //empty++;
                        //if (empty >= 2)
                        //{
                        //    flag = true;
                        //    break;
                        //}
                    }
                }
                if (flag)
                {
                    break;
                }
                ret.AddRange(tmp);
                标题组.AddRange(行标题);
                Progress.更新(工作状态.处理中, deg);
                basic -= deg;

            }
            Progress.更新(工作状态.处理中, basic);
            return ret;
        }
        /// <summary>
        /// 判断一个格中的内容语义上是否是数据项，如果是标题则返回false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool 判断数据项(string str)
        {
            if (正则匹配(str, 规则库[匹配模式.日期].表达式))
            {
                return true;
            }
            if (正则匹配(str, 规则库[匹配模式.数字].表达式))
            {
                return true;
            }
            if(正则匹配(str,规则库[匹配模式.标准金额].表达式))
            {
                return true;
            }
            if (str.Equals("男") || str.Equals("女"))
            {
                return true;
            }
            if (省.Contains(str))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取Excel并获取数据
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="主键列"></param>
        /// <param name="标题"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private static Dictionary<主键, Hashtable> 获取值(string[,] 数据, List<坐标值> 标题, ExcelRule rule, 文件类型 类型)
        {
            Dictionary<int, string> 列标题 = new Dictionary<int, string>();
            Dictionary<主键, Hashtable> ret = new Dictionary<主键, Hashtable>();
            for (int r = rule.数据起始行 - 1; r <= rule.数据结束行 - 1; r++)
            {
                List<string> vs = new List<string>();
                rule.主键列.ToList().ForEach(x => vs.Add(数据[r, x - 1].Trim()));
                主键 key = new 主键(vs);


                Hashtable hashtable = new Hashtable();
                for (int c = rule.起始列 - 1; c <= rule.结束列 - 1; c++)
                {
                    string tit = string.Empty;
                    if (列标题.TryGetValue(c + 1, out tit))
                    {
                        hashtable.Add(tit, 数据[r, c]);
                    }
                    else
                    {
                        tit = 获取某列的标题(标题, c + 1);
                        列标题.TryAdd(c + 1, tit);
                        hashtable.Add(tit, 数据[r, c]);
                    }
                }
                ret.TryAdd(key, hashtable);

                //if (类型.Equals(文件类型.Excel子表))
                //{
                //    Hashtable hashtable = new();
                //    for (int c = rule.起始列 - 1; c <= rule.结束列 - 1; c++)
                //    {
                //        string tit = string.Empty;
                //        if (列标题.TryGetValue(c + 1, out tit))
                //        {
                //            hashtable.Add(tit, 数据[r, c]);
                //        }
                //        else
                //        {
                //            tit = 获取某列的标题(标题, c + 1);
                //            列标题.TryAdd(c + 1, tit);
                //            hashtable.Add(tit, 数据[r, c]);
                //        }
                //    }
                //    ret.TryAdd(key, hashtable);
                //}
                //if (类型.Equals(文件类型.Excel主表))
                //{
                //    ret.TryAdd(key, null);
                //}
            }
            return ret;
        }
        private static Dictionary<主键, Hashtable> 获取值(string[,] 数据, List<坐标值> 标题, ExcelRule rule, 文件类型 类型,工作进度 Progress)
        {
            float deg = 100f / (rule.数据结束行 - rule.数据起始行+1);
            Dictionary<int, string> 列标题 = new Dictionary<int, string>();
            Dictionary<主键, Hashtable> ret = new Dictionary<主键, Hashtable>();
            for (int r = rule.数据起始行 - 1; r <= rule.数据结束行 - 1; r++)
            {
                if (数据[r, rule.主键列[0]-1].Trim()==string.Empty)
                {
                    continue;
                }
                List<string> vs = new List<string>();
                
                rule.主键列.ToList().ForEach(x =>
                {
                    vs.Add(数据[r, x - 1].Trim());
                });
                主键 key = new 主键(vs);

                Hashtable hashtable = new Hashtable();
                for (int c = rule.起始列 - 1; c <= rule.结束列 - 1; c++)
                {
                    string tit = string.Empty;
                    if (列标题.TryGetValue(c + 1, out tit))
                    {
                        hashtable.Add(tit, 数据[r, c]);
                    }
                    else
                    {
                        tit = 获取某列的标题(标题, c + 1);
                        列标题.TryAdd(c + 1, tit);
                        hashtable.Add(tit, 数据[r, c]);
                    }
                }
                ret.TryAdd(key, hashtable);

                //if (类型.Equals(文件类型.Excel子表))
                //{
                //    Hashtable hashtable = new();
                //    for (int c = rule.起始列 - 1; c <= rule.结束列 - 1; c++)
                //    {
                //        string tit = string.Empty;
                //        if (列标题.TryGetValue(c + 1, out tit))
                //        {
                //            hashtable.Add(tit, 数据[r, c]);
                //        }
                //        else
                //        {
                //   D i a g T o o l W i n d o w . v 2                               $ 
   �   ����                                    <        O u t l i n i n g S t a t e D i r                               $  ������������                                    7  ��     P r o j E x p l o r e r S t a t e                               $       ����                                    *  k	      T a s k L i s t S h o r t c u t s                               $  ������������                                    �         @H : \ I n t e l l i C l o u d \ M o d e l L i b \ U s e r . c s �k���>^��@A����'b�~ʟv=^�a�T�    ����������������           XH : \ I n t e l l i C l o u d \ R e p o s i t o r i e s \ A P I \ I S e r v i c e . c s �5��@�`�E��s7s{�����j{+ (�    ����������������                                                   zH : \ I n t e l l i C l o u d \ I n t e l l i M o b i l e \ I n t e l l i M o b i l e \ A P I \ C a m e r a B a s e . c s �z��m�Rɵ�|Y'�Տ�8`��d�WI��    ����������������                 O u t l i n i n g S t a t e V 1 6 7 7 0 9                       , �   @  ����                                    �  �       O u t l i n i n g S t a t e V 1 6 7 7 1 0                       , ������������                                    �  �       O u t l i n i n g S t a t e V 1 6 7 7 1 1                       ,     ����                                    �  �       O u t l i n i n g S t a t e V 1 6 7 7 1 2                       , ������������                                    �  �       tH : \ I n t e l l i C l o u d \ M o d e l L i b \ M o d e l L i b . c s p r o j . n u g e t . d g s p e c . j s o n [L���-�`��y�����i-�QȮ�'�^{�    ����������������   ,   -   .   /   0   lH : \ I n t e l l i C l o u d \ M o d e l L i b \ M o d e l L i b . c s p r o j . n u g e t . g . p r o p s ��p1,��!s�Q�G�@<�@nl0K��V%���    ����������������   Z   [   ����]   ^   _   ����pH : \ I n t e l l i C l o u d \ M o d e l L i b \ M o d e l L i b . c s p r o j . n u g e t . g . t a r g e t s �ɵ58HI��a�v�z0�  �  $16049fb3-174f-40b4-91d1-abada6e5327e2 �D:0:0:{B3C7830C-E02C-44C2-829E-3C2AE49FF55D}|MobileLib\MobileLib.csproj|solutionrelative:mobilelib\formsender.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{B994FCD2-CD18-468C-8463-56FCA14DB664}|IntelliMobile\IntelliMobile.Android\IntelliMobile.Android.csproj|solutionrelative:intellimobile\intellimobile.android\properties\androidmanifest.xml||{FA3CD31E-987B-443A-9B81-186104E8DAC1}�D:0:0:{5DD445F6-2020-48F0-8CA7-C436224F20A0}|ModelLib\ModelLib.csproj|solutionrelative:modellib\sourcefile.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{F08D1547-AAA1-4601-AEBD-66F44C604D8D}|IntelliCloud\IntelliCloud.csproj|solutionrelative:intellicloud\controllers\homecontroller.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{96C3E2A6-7BF0-481C-92CC-7C8032BD9115}|TestAndroid\TestAndroid\TestAndroid.csproj|solutionrelative:testandroid\testandroid\mainpageviewmodel.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{96C3E2A6-7BF0-481C-92CC-7C8032BD9115}|TestAndroid\TestAndroid\TestAndroid.csproj|solutionrelative:testandroid\testandroid\viewmodellocator.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{F08D1547-AAA1-4601-AEBD-66F44C604D8D}|IntelliCloud\IntelliCloud.csproj|solutionrelative:intellicloud\models\webserver.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{F08D1547-AAA1-4601-AEBD-66F44C604D8D}|IntelliCloud\IntelliCloud.csproj|solutionrelative:intellicloud\models\errorviewmodel.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{F08D1547-AAA1-4601-AEBD-66F44C604D8D}|IntelliCloud\IntelliCloud.csproj|solutionrelative:intellicloud\program.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{F08D1547-AAA1-4601-AEBD-66F44C604D8D}|IntelliCloud\IntelliCloud.csproj|solutionrelative:intellicloud\startup.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{B3C7830C-E02C-44C2-829E-3C2AE49FF55D}|MobileLib\MobileLib.csproj|solutionrelative:mobilelib\androidclient.cs||{A6C744A8-0E4A-4FC6-886A-064283054674}�D:0:0:{B32BEEDB-67AA-41C3-900D-F0290944B3B4}|Repositories\Repositories.csproj|seach (KeyValuePair<主键, Hashtable> pair in x.行值对)
                    {
                        if (!索引组.Contains(pair.Key, 比较器))
                        {
                            值.TryAdd(pair.Key, pair.Value);
                            索引组.Add(pair.Key);
                        }
                    }
                });
                
            }
            t.Start();
            t.Wait();
            string[,] Values = new string[标题最大行 + 值.Count, 结束列];
            Task t1 = new Task(() =>
            {
                标题入矩阵(Values,标题);
            });
            Task t2 = new Task(() =>
            {
                数据入矩阵(Values, 标题, 值, 标题最大行 + 1);
            });
            t1.Start();
            t2.Start();
            Task.WaitAll(new Task[] { t1, t2 });
            矩阵写入表(Values, excel);
            excel.保存();
            return Task.FromResult(true);
        }
        private static void 标题入矩阵(string[,] Matrix,List<坐标值> 标题)
        {
            标题.ForEach(x => 
            {
                if (x.行 <= Matrix.GetLongLength(0) && x.列 <= Matrix.GetLongLength(1))
                {
                    Matrix[x.行 - 1, x.列 - 1] = x.值;
                }
            });
        }
        private static void 数据入矩阵(string[,] Matrix,List<坐标值> 标题, Dictionary<主键, Hashtable> 值,int 起始行)
        {
            Dictionary<int, string> ts = 获取直接标题(标题);
            int row = 起始行 -1;
            foreach (KeyValuePair<主键, Hashtable> pair in 值)
            {
                foreach (KeyValuePair<int, string> s in ts)
                {
                    if (pair.Value != null)
                    {
                        Matrix[row, s.Key - 1] = pair.Value[s.Value] != null ? pair.Value[s.Value].ToString() : string.Empty;
                    }
                }
                row++;
            }
        }
        /// <summary>
        /// 执行后的Excel已经生成
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="结果集"></param>
        /// <returns></returns>
        public static Task<bool> 写入表(ExcelUnit excel,List<WordSearch> 结果集)
        {
            int ColMaximun = 0;
            List<Dictionary<string, string>> Collect=new List<Dictionary<string, string>>();
            foreach(WordSearch search in 结果集)
            {
                Collect.Add(search.获取结果().ToDictionary(x=>x.Key,x=>x.Value.匹配结果));
                ColMaximun = search.获取结果().Count > ColMaximun ? search.获取结果().Count : ColMaximun;
            }
            string[,] 矩阵 = new string[3*Collect.Count+1,ColMaximun+1];
            int Startindex = 1;
            Collect.ForEach(x => 
            {
                int ColIndex = 1;
                foreach(KeyValuePair<string,string> pair in x)
                {
                    矩阵[Startindex, ColIndex]= pair.Key;
                    矩阵[Startindex + 1, ColIndex] = pair.Value;
                    ColIndex++;
                }
                Startindex += 3;
            });
            矩阵写入表(矩阵,excel);
            excel.保存();
            return Task.FromResult(true);
        }
        #endregion


        #region 基础方法区
        public static int 均值(List<int> ins)
        {
            int res=0;
            ins.ForEach(x => res += x);
            return res / ins.Count;
        } 
        
        public static void 交换(ref string str1,ref string str2)
        {
            string tmp = str1;
            str1 = str2;
            str2 = tmp;
        }

        public static string 移除空格(string str)
        {
            string ret = string.Empty;
            foreach(char c in str)
            {
                if(!c.Equals(' '))
                {
                    ret += c;
                }
            }
            return ret;
        }

        public static string 移除制表符(string str)
        {
            string ret = string.Empty;
            foreach (char c in str)
            {
                if (!c.Equals('\r')&&!c.Equals('\a'))
                {
                    ret += c;
                }
            }
            return ret;
        }

        public static bool 包含(string str1, string str2)
        {
            if (str2.Length > str1.Length)
            {
                交换(ref str1, ref str2);
            }
            if (str1.Contains(str2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int 获取坐标最值(List<坐标值> 坐标,最值 最值类型)
        {
            
            int ret = 1;
            switch (最值类型)
            {
                case 最值.最大列:
                    foreach(坐标值 loc in 坐标)
                    {
                        if (loc.列 > ret)
                        {
                            ret = loc.列;
                        }
                    }
                    break;
                case 最值.最大行:
                    foreach (坐标值 loc in 坐标)
                    {
                        if (loc.行 > ret)
                        {
                            ret = loc.行;
                        }
                    }
                    break;
                case 最值.最小行:
                    if (坐标.Count!= 0)
                    {
                        ret = 坐标[0].行;
                        foreach (坐标值 loc in 坐标)
                        {
                            if (loc.行 < ret)
                            {
                                ret = loc.行;
                            }
                        }
                    }
                    break;
                case 最值.最小列:
                    if (坐标.Count != 0)
                    {
                        ret = 坐标[0].列;
                        foreach (坐标值 loc in 坐标)
                        {
                            if (loc.列 < ret)
                            {
                                ret = loc.列;
                            }
                        }
                    }
                    break;
            }
            return ret;
        }
        public enum 最值
        {
            最大行,
            最小行,
            最大列,
            最小列
        }
        #endregion

    #region Excel生成区
        private static Point Measure(FormCell[,] Matrix)
        {
            int c = (int)Matrix.GetLongLength(1) - 1;
            int r = (int)Matrix.GetLongLength(0) - 1;
            while ((Matrix[r, c] == null || FormCell.判断空或增生单元(Matrix[r, c].内容)) && r >= 0)
            {
                if (c == 0)
                {
                    r--;
                    c = (int)Matrix.GetLongLength(1) - 1;
                }
                else
                {
                    c--;
                }
            }
            int retr = r;
            if (r > 0)
            {
                c = (int)Matrix.GetLongLength(1) - 1;
                while ((Matrix[r, c] == null || FormCell.判断空或增生单元(Matrix[r, c].内容)) && c >= 0)
                {
                    if (r == 0)
                    {
                        c--;
                        r = retr;
                    }
                    else
                    {
                        r--;
                    }
                }
            }
            return new Point(retr, c);
        }
        private static Dictionary<Point, Point> 单元格整合(FormCell[,] form, Point p)
        {
            Dictionary<KeyValuePair<Point, Point>, bool> ret = new Dictionary<KeyValuePair<Point, Point>, bool>();
            for (int r = p.X; r >= 0; r--)
            {
                for (int c = p.Y; c >= 0; c--)
                {
                    if (form[r, c]?.形态 == FormCell.Formation.右增生 || form[r, c]?.形态 == FormCell.Formation.下增生)
                    {
                        Point pt = new Point(r, c);
                        foreach (var pair in ret)
                        {
                            if (pair.Key.Key.X >= r && pair.Key.Key.Y >= c && pair.Key.Value.X <= r && pair.Key.Value.Y <= c)
                            {
                                c = pair.Key.Value.Y;
                                goto OUT;
                            }
                        }
                        bool clear = true;
                        var targ = 查重(form, pt, ref clear);
                        if (targ.X != pt.X || targ.Y != pt.Y)
                        {
                            ret.Add(new KeyValuePair<Point, Point>(pt, targ), clear);
                        }
                    }
                OUT:;
                }
            }
            var rs = new Dictionary<Point, Point>();
            foreach (var kv in ret)
            {
                if (kv.Value)
                {
                    rs.Add(kv.Key.Key, kv.Key.Value);
                }
            }
            return rs;
        }
        private static Point 查重(FormCell[,] form, Point basic, ref bool f)
        {
            var cell = form[basic.X, basic.Y];
            int r = basic.X;
            int c = basic.Y;
            switch (cell.形态)
            {
                case FormCell.Formation.下增生:
                    return 查重(form, new Point(r - 1, c), ref f);
                case FormCell.Formation.右增生:
                    return 查重(form, new Point(r, c - 1), ref f);
                case FormCell.Formation.原生:
                    if (cell.内容.Equals(""))
                    {
                        f = false;
                    }
                    else
                    {
                        f = true;
                    }
                    break;
            }
            return new Point(r, c);
        }
        protected static FormSheet 生成表格(DocxUnit unit, string JSON)
        {
            FormSheet sheet = FormSheet.解析结果(JSON);

            FormCell[,] matr = sheet.生成矩阵(out Dictionary<Point, Point> Merges, out Point area);
            Point rc = Measure(matr);
            var pack = Merges;
            var t = unit.添加表格(area.X + 1, area.Y + 1);
            t.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
            t.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            for (int r = 0; r < rc.X + 1; r++)
            {
                for (int c = 0; c < rc.Y + 1; c++)
                {
                    string content = matr[r, c] != null ? matr[r, c].内容 : "";
                    string target = "";
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (i + 1 < content.Length && content[i] == '\\')
                        {
                            switch (content[i + 1])
                            {
                                case 'n':
                                    target += '\n';
                                    break;
                                case 'r':
                                    target += '\r';
                                    break;
                                case 'a':
                                    target += '\a';
                                    break;
                            }
                            i++;
                        }
                        else
                        {
                            target += content[i];
                        }
                    }
                    t.Cell(r + 1, c + 1).Range.Text = target;
                    t.Cell(r + 1, c + 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                }
            }
            foreach (var p in pack)
            {
                t.Cell(p.Value.X + 1, p.Value.Y + 1).Merge(t.Cell(p.Key.X + 1, p.Key.Y + 1));
            }


            return sheet;
        }

        #endregion
    }
}
