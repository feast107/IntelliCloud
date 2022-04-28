using System;
using System.Collections.Generic;
using System.Text;

namespace IntelliHttpClient.ViewModels
{
    public class WordRule
    {
        public int Length { get; set; }
        
        public 匹配模式 Mode { get; set; }
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
    }
}
