using System;
using System.Collections.Generic;
using System.Text;

namespace IntelliHttpClient.ViewModels
{
    public class ExcelRule
    {
        public bool IsMain { get; set; }
        public int Sheet { get; set; }
        public int TitleStart { get; set; }
        public int TitleEnd { get; set; }
        public int DataStart { get; set; }

        public int DataEnd { get; set; }

        public int StartCol { get; set; }

        public int EndCol { get; set; } 

        public int[]? KeyCol { get; set; }
    }
}
