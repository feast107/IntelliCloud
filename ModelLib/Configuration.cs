using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ModelLib.Logics;
using static ModelLib.WordRule;

namespace ModelLib
{
    public enum 角色
    {
        用户
    }

    public enum 令牌
    {
        Sid,
        PhoneNumber,
        Name
    }

    public enum 工作状态
    {
        创建,
        待处理,
        处理中,
        完成,
        失败
    }

    public enum 进程状态
    {
        工作中,
        闲置,
        待重置,
        回收
    }

    public enum 文件类型
    {
        Word,
        Excel主表,
        Excel子表,
        图片,
        无适配
    }

   
    public class 工作进度
    {
        public 工作进度(string FilePath)
        {
            filepath = FilePath;
            进度 = 0f;
            this.更新();
        }
        private float 进度;
        public 工作状态 status=工作状态.创建;
        private readonly string filepath;
        private string statusstring;
        public string 文件 { get => filepath; }
        public string 状态 { get => statusstring; }

        public virtual void 更新(工作状态 状态,float? Step)
        {

            status = 状态;
            if (status == 工作状态.处理中)
            {
                statusstring = status.ToString() + "..." + (int)((进度 += (Step == null ? 0f :(float)Step))/3f) + "%";
            }
            else
            {
                statusstring = status.ToString();
            }
        }
        public virtual void 更新()
        {
            statusstring = status.ToString();
        }
        public virtual void 更新(工作状态 状态)
        {
            status = 状态;
            statusstring = status.ToString();
        }
        public virtual void 更新(工作状态 状态,Exception e)
        {
            status = 状态;
            statusstring = status.ToString()+"  于："+e.Message;
        }
    }
  

    public static class Configuration
    {
        /// <summary>
        /// 令牌转化
        /// </summary>
        /// <param name="cl"></param>
        /// <returns></returns>
        public static string ToClaim(令牌 cl)
        {
            switch (cl)
            {
                case 令牌.Sid:
                    return ClaimTypes.Sid;
                case 令牌.PhoneNumber:
                    return ClaimTypes.MobilePhone;
                case 令牌.Name:
                    return ClaimTypes.Name;
            }
            return string.Empty;
        }
        public const int 最大进程数 = 10;

        public static volatile int 可用应用数 = 最大进程数;
        private static readonly Semaphore 可用应用量 = new Semaphore(最大进程数, 最大进程数);
        private static readonly Semaphore Excel应用量 = new Semaphore(0, 最大进程数);
        private static readonly Semaphore Word应用量 = new Semaphore(0, 最大进程数);
        private static readonly Semaphore 工作Excel量 = new Semaphore(0, 最大进程数);
        private static readonly Semaphore 工作Word量 = new Semaphore(0, 最大进程数);
        public static readonly Dictionary<信号量, Semaphore> 信号量组 = new  Dictionary<信号量, Semaphore>()
        {
            {信号量.可用应用,可用应用量 },
            {信号量.Excel应用,Excel应用量 },
            {信号量.Excel文档,工作Excel量 },
            {信号量.Word应用,Word应用量 },
            {信号量.Word文档,工作Word量 },
        };

        public static void 回收应用()
        {
            new Task(()=> UnitBase.回收应用()).Start();
        }

        public const string Src目录 = @"src";

        public const string wwwroot目录 = @"wwwroot";

        #region 进程与文件比对
        public static bool 类型比对(进程模型 模型, 文件类型 类型)
        {
            switch (模型)
            {
                case 进程模型.Excel:
                    if (类型.Equals(文件类型.Excel主表) || 类型.Equals(文件类型.Excel子表))
                    {
                        return true;
                    }
                    break;
                case 进程模型.Word:
                    if (类型.Equals(文件类型.Word))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        #endregion

        #region  后缀与文件类型转换

        private static readonly Dictionary<文件类型, string[]> 后缀 = new Dictionary<文件类型, string[]>()
        {
            { 文件类型.Word,new string []{ ".docx",".doc" } },
            { 文件类型.Excel主表, new string [] {".xlsx",".xls" } },
            { 文件类型.Excel子表, new string [] {".xlsx",".xls" } },
            { 文件类型.图片, new string [] {".jpg",".png" ,".bmp" ,".jpeg"} }
        };

        public static string[] 获取后缀(文件类型 类型)
        {
            string[] vs;
            后缀.TryGetValue(类型, out vs);
            return vs;
        }

        public static 文件类型 获取类型(string Ext)
        {
            foreach(KeyValuePair<文件类型,string[]> keyValuePair in 后缀)
            {
                if (keyValuePair.Value.Contains(Ext))
                {
                    return keyValuePair.Key;
                }

            }
            return 文件类型.无适配 ;
        }

        #endregion

        #region 匹配模式与str转换
        public static 匹配模式 str转模式(string str)
        {
            foreach(var 模式 in Enums.GetMembers<匹配模式>())
            {
                if (str.Equals(模式.Name))
                {
                    return 模式.Value;
                }
            }
            return 匹配模式.任意字符;
        }

        public static string 模式转str(匹配模式 模式)
        {
            return 模式.GetName();
        }

        #endregion

        #region 进程模型与Type转换
        public static 进程模型 Type转模型(Type T)
        {
            switch (T.Name)
            {
                case "DocxUnit":
                    return 进程模型.Word;
                case "ExcelUnit":
                    return 进程模型.Excel;
            }
            return 进程模型.无适配;
        }

        public static string 模型转str(进程模型 Model)
        {
            switch (Model)
            {
                case 进程模型.Word:
                    return "DocxUnit";
                case 进程模型.Excel:
                    return "ExcelUnit";
            }
            return null;
        }
        #endregion
    }
}
