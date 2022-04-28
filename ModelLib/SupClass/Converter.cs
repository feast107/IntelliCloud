using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class Converter
    {
        #region Parse

        #region Dictionary Parse To String
        /// <summary>
        /// Dictionary Parse To String
        /// </summary>
        /// <param name="parameters">Dictionary</param>
        /// <returns>若字典为空则返回null</returns>
        static public string ParseToString(IDictionary<string, string> parameters)
        {
            if (parameters is null && parameters.Count == 0)
            {
                return null;
            }
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(":").Append(value).Append(";");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }
        #endregion

        #region String Parse To Dictionary
        /// <summary>
        /// String Parse To Dictionary
        /// </summary>
        /// <param name="parameter">String</param>
        /// <returns>Dictionary</returns>
        static public Dictionary<string, string> ParseToDictionarySS(string parameter)
        {
            try
            {
                String[] dataArry = parameter.Split(';');
                Dictionary<string, string> dataDic = new Dictionary<string, string>();
                for (int i = 0; i <= dataArry.Length - 1; i++)
                {
                    String dataParm = dataArry[i];
                    int dIndex = dataParm.IndexOf(":");
                    if (dIndex != -1)
                    {
                        String key = dataParm.Substring(0, dIndex);
                        String value = dataParm.Substring(dIndex + 1, dataParm.Length - dIndex - 1);
                        dataDic.Add(key, value);
                    }
                }

                return dataDic;
            }
            catch
            {
                return null;
            }
        }
        #endregion


        #endregion

        #region Parse

        #region Dictionary Parse To String
        /// <summary>
        /// Dictionary Parse To String
        /// </summary>
        /// <param name="parameters">Dictionary</param>
        /// <returns>若字典为空则返回null</returns>
        static public string ParseToString(IDictionary<int, string> parameters)
        {
            if (parameters is null || parameters.Count == 0)
            {
                return null;
            }
            IDictionary<int, string> sortedParams = new SortedDictionary<int, string>(parameters);
            IEnumerator<KeyValuePair<int, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                int key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(":").Append(value).Append(";");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }
        #endregion

        #region String Parse To Dictionary
        /// <summary>
        /// String Parse To Dictionary
        /// </summary>
        /// <param name="parameter">String</param>
        /// <returns>Dictionary</returns>
        static public Dictionary<int, string> ParseToDictionaryIS(string parameter)
        {
            try
            {
                string[] dataArry = parameter.Split(';');
                Dictionary<int, string> dataDic = new Dictionary<int, string>();
                for (int i = 0; i <= dataArry.Length - 1; i++)
                {
                    string dataParm = dataArry[i];
                    int dIndex = dataParm.IndexOf(":");
                    if (dIndex != -1)
                    {
                        int key = Convert.ToInt32(dataParm.Substring(0, dIndex));
                        string value = dataParm.Substring(dIndex + 1, dataParm.Length - dIndex - 1);
                        dataDic.Add(key, value);
                    }
                }

                return dataDic;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #endregion

        #region Collect







        #endregion

        #region Api
        public static WordRule Parse(IntelliHttpClient.ViewModels.WordRule rule)
        {
            return new WordRule((WordRule.匹配模式)(int)(rule.Mode), rule.Length);
        }

        public static List<WordRule> Parse(List<IntelliHttpClient.ViewModels.WordRule> rules)
        {
            List<WordRule> result = new List<WordRule>();
            foreach(var r in rules)
            {
                result.Add(Parse(r));
            }
            return result;
        }
       
        public static Dictionary<string,List<WordRule>> Parse(IntelliHttpClient.ViewModels.WordRuleCollection collection)
        {
            Dictionary<string,List<WordRule>> result = new Dictionary<string, List<WordRule>>();
            foreach(var o in collection.WordRules)
            {
                result.Add(o.Key, Parse(o.Value));
            }
            return result;
        }
       
        public static ExcelRule Parse(IntelliHttpClient.ViewModels.ExcelRule rule)
        {
            return new ExcelRule(rule.IsMain ? 文件类型.Excel主表 : 文件类型.Excel子表)
            {
                主键列 = rule.KeyCol,
                数据结束行 = rule.EndCol,
                数据起始行 = rule.StartCol,
                标题结束行 = rule.TitleEnd,
                标题起始行 = rule.TitleStart,
                是否主表 = rule.IsMain,
                结束列 = rule.EndCol,
                表单页 = rule.Sheet,
                起始列 = rule.StartCol
            };
        }
        #endregion

    }
}
