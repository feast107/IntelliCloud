using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace ModelLib
{
    /// <summary>
    /// Excel工作单元
    /// </summary>
    public sealed class ExcelUnit : UnitBase, IDisposable
    {
        #region 地址

        #endregion

        #region 常量区
        public const int 队列数 = 1;
        private string m_target = null;
        private bool m_isCreateMode = false;
        private readonly object MISSING_VALUE = System.Reflection.Missing.Value;
        #endregion

        #region 静态区 /可修改
        /// <summary>
        /// 多线程
        /// </summary>
        private static readonly Semaphore[] 等待组 = new Semaphore[]
        {
            信号量组[信号量.可用应用],
            信号量组[信号量.Excel文档]
        };
        /// <summary>
        /// 从属于Excel单元的应用量和闲置数
        /// </summary>
        private static int 应用量;
        private static int 闲置数;

        private static readonly List<ExcelUnit> 队列 = new List<ExcelUnit>();

        #endregion

        #region 实例区 
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        private readonly Application m_AppMain = null;
        private Workbook m_Workbook = null;
        private Worksheet m_Worksheet = null;
        #endregion

        #region 构造器
        /// <summary>
        /// 私有构造
        /// </summary>
        /// <param name="生命周期"></param>
        private ExcelUnit(生命周期 生命周期) : base(生命周期, 进程模型.Excel)
        {
            m_AppMain = InitApp();
            switch (生命周期)
            {
                case 生命周期.自管:
                    创建进程();
                    队列.Add(this);
                    开始工作();
                    break;
            }
        }
        #endregion

        #region 外部调用
        /// <summary>
        /// 由类创建解析器/自管生命周期
        /// </summary>
        /// <returns></returns>
        public static Task<ExcelUnit> GetInstance()
        {
            if (队列.Count != 0)
            {
                //返回闲置的实例
                if (闲置数 != 0)
                {
                    lock (队列)
                    {
                        if (闲置数 != 0)
                        {
                            foreach (ExcelUnit excel in 队列)
                            {
                                if (excel.闲置)
                                {
                                    excel.开始工作();
                                    return Task.FromResult(excel);
                                }
                            }
                        }
                    }
                }
                //请求新的实例
                if (Configuration.可用应用数 != 0)
                {
                    lock (队列)
                    {
                        if (Configuration.可用应用数 != 0)
                        {
                            return Task.FromResult(new ExcelUnit(生命周期.自管));
                        }
                    }
                }
                //通知其余应用
                else
                {
                    Configuration.回收应用();
                }
            }
            //创建新的实例
            else
            {
                lock (队列)
                {
                    if (队列.Count == 0)
                    {
                        return Task.FromResult(new ExcelUnit(生命周期.自管));
                    }
                }
            }
            int i = WaitHandle.WaitAny(等待组);
            lock (队列)
            {
                等待组[i].Release();
                //至多回调一次
                return GetInstance();
            }
        }

        /// <summary>
        /// 由工厂创建解析器/托管生命周期
        /// </summary>
        /// <returns></returns>
        public static ExcelUnit NewInstance()
        {
            return new ExcelUnit(生命周期.托管);
        }

        /// <summary>
        /// 初始化应用
        /// </summary>
        /// <returns></returns>
        private static Application InitApp()
        {
            Application appMain = new Application();
            if (appMain == null)
            {
                return InitApp();
            }
            appMain.UserControl = true;
            //appMain.Visible = false;
            appMain.DisplayAlerts = false;
            appMain.AlertBeforeOverwriting = false;
            return appMain;
        }

        #region 工作相关
        /// <summary>
        /// 创建Excel，并进入打开状态
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public bool 创建Excel(string strPath)
        {
            m_target = strPath;
            m_isCreateMode = true;
            m_Workbook = m_AppMain.Workbooks.Add(MISSING_VALUE);
            return true;
        }
        /// <summary>
        /// 打开Excel
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public bool 打开Excel(string strPath)
        {
            if (m_Workbook == null)
            {
                base.打开文本();
                m_isCreateMode = true;
                m_Workbook = m_AppMain.Workbooks.Open(strPath);
                if (m_Workbook == null)
                {
                    return false;
                }
                m_target = strPath;
                return true;
            }
            return false;
        }
        public string Version { get { return m_AppMain.Version; } }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get { return m_Workbook.Sheets.Count; } }
        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get { return m_Worksheet.UsedRange.Cells.Rows.Count; } }
        /// <summary>
        /// 列数
        /// </summary>
        public int ColCount { get { return m_Worksheet.UsedRange.Cells.Columns.Count; } }
        /// <summary>
        /// 选择某页
        /// </summary>
        /// <param name="page"></param>
        public void 选择Sheet(int page)
        {
            if (page <= PageCount)
            {
                m_Worksheet = (Worksheet)m_Workbook.Worksheets.get_Item(page);
            }
            else
            {
                m_Worksheet = (Worksheet)m_Workbook.Worksheets.get_Item(PageCount);
                for (int i = PageCount; i < page; i++)
                {
                    m_Workbook.Worksheets.Add(After: m_Worksheet);
                    m_Worksheet = (Worksheet)m_Workbook.Worksheets.get_Item(i + 1);
                }
                m_Worksheet = (Worksheet)m_Workbook.Worksheets.get_Item(page);
            }


        }
        /// <summary>
        /// 获取单元格的跨行
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public int RowSpan(int nRow, int nCol)
        {
            return ((Range)m_Worksheet.Cells[nRow, nCol]).MergeArea.Rows.Count;
        }
        /// <summary>
        /// 获取单元格的跨列
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public int ColSpan(int nRow, int nCol)
        {
            return ((Range)m_Worksheet.Cells[nRow, nCol]).MergeArea.Columns.Count;
        }
        /// <summary>
        /// 读某行某列的内容
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string 读取(int nRow, int nCol)
        {
            Range range = (Range)m_Worksheet.Cells[nRow, nCol];
            return (bool)range.MergeCells ? ((Range)m_Worksheet.Cells[range.MergeArea.Row, range.MergeArea.Column]).Text.ToString() : (range.Text.ToString());
        }
        /// <summary>
        /// 写某行某列的内容
        /// </summary>
        /// <param name="nRow">单元格行</param>
        /// <param name="nCol">单元格列</param>
        /// <param name="value">值</param>
        public void 写入(int nRow, int nCol, string value)
        {
            m_Worksheet.Cells[nRow, nCol] = value;
        }
        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="index">索引</param>
        public void DeleteRow(int index)
        {
            ((Range)m_Worksheet.Rows[index]).Delete();
        }
        /// <summary>
        /// 删除一列
        /// </summary>
        /// <param name="index">索引</param>
        public void DeleteCol(int index)
        {
            ((Range)m_Worksheet.Columns[index]).Delete();
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void 保存()
        {
            if (m_isCreateMode)
            {
                m_Workbook.SaveAs(m_target);
            }
        }
        /// <summary>
        /// 单独关闭文本，配合QuitApp使用
        /// </summary>
        public void 关闭Excel()
        {
            if (m_Workbook != null)
            {
                m_Workbook.Close();
                m_Workbook = null;
                base.关闭文本();
            }
        }
        #endregion

        /// <summary>
        /// 保证进程关闭 
        /// </summary>
        /// <param name="excel"></param>
        private void Kill(Application excel)
        {
            IntPtr t = new IntPtr(excel.Hwnd);//得到这个句柄，具体作用是得到这块内存入口 

            _ = GetWindowThreadProcessId(t, out int k);   //得到本进程唯一标志k
            Process p = Process.GetProcessById(k);   //得到对进程k的引用
            try
            {
                _ = Marshal.ReleaseComObject(o: m_AppMain);
            }
            finally
            {
                p.CloseMainWindow();
                p.Kill();
            }
            /*   p.Kill();  */   //关闭进程k

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #region 进程控制
        public override void 创建进程()
        {
            创建进程(信号量.Excel应用, ref 应用量, 信号量.Excel文档, ref 闲置数);
        }

        /// <summary>
        /// 开始调用/交换状态
        /// </summary>
        public override void 开始工作()
        {
            开始工作(信号量.Excel文档, ref 闲置数);
        }

        /// <summary>
        /// 结束调用/交换状态
        /// </summary>
        public override void 结束工作()
        {
            关闭Excel();
            结束工作(信号量.Excel文档, ref 闲置数);
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        public override void 进程退出()
        {
            进程退出(信号量.Excel应用, ref 应用量, 信号量.Excel文档, ref 闲置数);
            关闭Excel();
            if (m_AppMain != null)
            {
                if (LifeSpan.Equals(生命周期.自管))
                {
                    lock (队列)
                    {
                        队列.Remove(this);
                    }
                }
                m_AppMain.Quit();
                Kill(m_AppMain);
            }
        }

        public void Dispose()
        {
            进程退出();
        }
        #endregion

        #endregion

    }


}