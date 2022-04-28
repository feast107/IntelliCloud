using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelliTool
{
    static class Program
    {
        public const string PROGRAMNAME = "IntelliTool";
        private const string CopyRight = "Kim17712909807";
        public static ExcelUnit _excel;
        public readonly static object ExceLocker = new object();
        public static DocxUnit _docx;
        public readonly static object DocxLocker = new object();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(HomeForm.GetForm());
            //Application.Run(new TestForm.TestFormHigher());
        }

        public async static void Release()
        {
            int ex = await ReleaseExcel();
            int wo = await ReleaseDocx();
            if (ex.Equals(0) && wo.Equals(0))
            {
                return;
            }
            string res = string.Empty;
            if (ex.Equals(-2147467262))
            {
                res += "Word进程已退出\n";
            }
            if (ex.Equals(-2147467262))
            {
                res += "Excel进程已退出\n" ;
            }
            MessageBox.Show(res,项目提示());
        }
        public static Task<int> ReleaseExcel()
        {
            if (_excel != null)
            {
                try
                {
                    _excel.进程退出();
                    _excel = null;
                    return Task.FromResult(0);
                }
                catch(Exception e)
                {
                    return Task.FromResult(e.HResult);
                }
            }
            return Task.FromResult(-1);
        }
        public static Task<int> ReleaseDocx()
        {
            if (_docx != null)
            {
                try
                {
                    _docx.进程退出();
                    _docx = null;
                    return Task.FromResult(0);
                }
                catch(Exception e)
                {
                    return Task.FromResult(e.HResult);
                }
            }
            return Task.FromResult(-1);
        }

        public static string 项目提示()
        {
            return "来自" + PROGRAMNAME + "的提示";
        }
    }
}
