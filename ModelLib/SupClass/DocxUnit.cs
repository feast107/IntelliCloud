using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using MSWord = Microsoft.Office.Interop.Word;
using Task = System.Threading.Tasks.Task;
namespace ModelLib
{
    /// <summary>
    /// Docx工作单元
    /// </summary>
    public sealed class DocxUnit : UnitBase , IDisposable
    {
        #region 地址

        #endregion

        #region 常量区
        const int 队列数 = 5;
        const MSWord.WdExportOptimizeFor paramExportOptimizeFor = MSWord.WdExportOptimizeFor.wdExportOptimizeForPrint;
        const MSWord.WdExportRange paramExportRange = MSWord.WdExportRange.wdExportAllDocument;
        const MSWord.WdExportItem paramExportItem = MSWord.WdExportItem.wdExportDocumentContent;
        const MSWord.WdExportCreateBookmarks paramCreateBookmarks = MSWord.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        static readonly int paramStartPage = 0;
        static readonly bool paramOpenAfterExport = false;
        static readonly int paramEndPage = 0;
        static readonly bool paramIncludeDocProps = true;
        static readonly bool paramKeepIRM = true;
        static readonly bool paramDocStructureTags = true;
        static readonly bool paramBitmapMissingFonts = true;
        static readonly bool paramUseISO19005_1 = false;
        static readonly object paramMissing = Type.Missing;
        #endregion
            
        #region 静态区  /可修改
        /// <summary>
        /// 多线程
        /// </summary>
        private static readonly Semaphore[] 等待组 = new Semaphore[] 
        {
            信号量组[信号量.可用应用],
            信号量组[信号量.Word文档] 
        };

        /// <summary>
        /// 从属于Docx单元的应用量和闲置数
        /// </summary>
        private static int 应用量;
        private static int 闲置数;

        private static readonly List<DocxUnit> 队列 = new List<DocxUnit>( );
        private static readonly bool 应用可见性 = false;
        private static readonly MSWord.WdAlertLevel 提示展示 = MSWord.WdAlertLevel.wdAlertsNone;
        #endregion

        #region 实例区
        private readonly MSWord.Application _AppMain = null;
        private MSWord.Document _WordDoc = null;
        

        #endregion

        #region 构造器
        /// <summary>
        /// 私有构造
        /// </summary>
        /// <param name="生命周期"></param>
        private DocxUnit(生命周期 生命周期) :base(生命周期,进程模型.Word)
        {
            _AppMain = InitApp();
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
        /// 类创建空解析器/自管生命周期
        /// </summary>
        /// <returns></returns>
        public static Task<DocxUnit> GetInstance()
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
                            foreach (DocxUnit docx in 队列)
                            {
                                if (docx.闲置)
                                {
                                    docx.开始工作();
                                    return Task.FromResult(docx);
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
                            return Task.FromResult(new DocxUnit(生命周期.自管));
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
                        return Task.FromResult(new DocxUnit(生命周期.自管));
                    }
                }
            }

            int i = WaitHandle.WaitAny(等待组);
            lock (队列)
            {
                等待组[i].Release();
                return GetInstance();
            }
        }

        /// <summary>
        /// 创建空解析器/托管生命周期
        /// </summary>
        /// <returns></returns>
        public static DocxUnit NewInstance()
        {
            return new DocxUnit(生命周期.托管);
        }

        /// <summary>
        /// 初始化应用
        /// </summary>
        /// <returns></returns>
        private static MSWord.Application InitApp()
        {
            MSWord.Application appMain = new MSWord.Application();
            if (appMain == null)
            {
                return InitApp();
            }
            appMain.Visible = 应用可见性;
            appMain.DisplayAlerts = 提示展示;
            return appMain;
        }

        #region 工作相关
        /// <summary>
        /// 创建Doc并进入打开状态
        /// </summary>
        public DocxUnit 创建Doc()
        {
            if (_WordDoc == null)
            {
                _WordDoc = _AppMain.Documents.Add();
            }
            return this;
        }

        /// <summary>
        /// 打开Doc
        /// </summary>
        /// <param name="path"></param>
        public DocxUnit 打开Doc(string path)
        {
            if (_WordDoc == null)
            {
                if (!Path.IsPathRooted(path))
                {
                    path = Path.GetFullPath(path);
                }
                if (!File.Exists(path))
                {
                    return 创建Doc();
                }
                _WordDoc = _AppMain.Documents.Open(path);

            }
            return this;
        }
        public string Version { get => _AppMain.Version; }

        /// <summary>
        /// 段落数
        /// </summary>
        public int ParagCount => _WordDoc.Paragraphs.Count;

        /// <summary>
        /// 表格数
        /// </summary>
        public int TableCount => _WordDoc.Tables.Count;

        public string 获取段落文本(int index)
        {
            if (index > 0 && index <= ParagCount)
            {
                return _WordDoc.Paragraphs[index].Range.Text;
            }
            return "";
        }

        #region nullable
        #nullable enable
        /// <summary>
        /// 获取一个表格中的内容
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string[,]? 获取表格文本(int index)
        {
            if (index > 0 && index <= TableCount)
            {
                MSWord.Table table = _WordDoc.Tables[index];
                int rm = table.Rows.Count;
                int cm = table.Columns.Count;
                string[,] strs = new string[rm, cm];
                for(int r =0; r< rm; r++)
                {
                    for(int c = 0; c < cm; c++)
                    {
                        try
                        {
                            strs[r, c] = 去除制表符(table.Cell(r + 1, c + 1).Range.Text.Trim());
                        }
                        catch
                        {
                            if (r > 0)
                            {
                                strs[r, c] = strs[r - 1, c];
                            }
                        }
                    }
                }
                return strs;

            }
            return null;
        }
        #endregion

        public MSWord.Table 添加表格(int Row,int Col)
        {
            return _WordDoc.Tables.Add(_AppMain.Selection.Range,Row,Col);
        }


        private static string 去除制表符(string str)
        {
            string ret = string.Empty;
            foreach(char c in str)
            {
                if (c != '\r' && c != '\a')
                {
                    ret += c;
                }
            }
            return ret;
        }

        /// <summary>
        /// 关闭dOC
        /// </summary>
        public void 关闭Doc()
        {
            if (_WordDoc != null)
            {
                _WordDoc.Close();
                _WordDoc = null;
                base.关闭文本();
            }
        }
  
        public void 保存Doc()
        {
            _WordDoc.Save();
        }

        public void 保存Doc(string path)
        {
            _WordDoc.SaveAs(path);
        }

        /// <summary>
        /// 保存绝对地址和相对地址都可以
        /// </summary>
        /// <param name="purppath"></param>
        public void SaveAsPDF(string purppath)
        {
            if (!Path.IsPathRooted(purppath))
            {
                purppath = Path.GetFullPath(purppath);
            }
            if(File.Exists(purppath))
            {
                return;
            }
            //文件保存
            _WordDoc.ExportAsFixedFormat(purppath,
                          MSWord.WdExportFormat.wdExportFormatPDF, paramOpenAfterExport,
                          paramExportOptimizeFor, paramExportRange, paramStartPage,
                          paramEndPage, paramExportItem, paramIncludeDocProps,
                          paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                          paramBitmapMissingFonts, paramUseISO19005_1,
                          paramMissing); //文件关闭

            _WordDoc.Save();
        }

        /// <summary>
        /// 插入表格内容
        /// </summary>
        /// <param name="table"></param>
        /// <param name="CellRow"></param>
        /// <param name="CellCol"></param>
        /// <param name="value"></param>
        public void Insert(int table, int CellRow, int CellCol, string str)
        {
            _WordDoc.Tables[table].Cell(CellRow, CellCol).Range.Text = str;
        }

        /// <summary>
        /// Doc转换pdf
        /// </summary>
        /// <param name="源路径"></param>
        /// <param name="目标路径"></param>
        /// <returns></returns>
        static public bool Doc转Pdf(string 源路径, string 目标路径)
        {
            信号量组[信号量.可用应用].WaitOne();
            bool result;
            MSWord.Application wordApplication = new MSWord.Application();
            MSWord.Document? wordDocument = null;
            try
            {
                object paramSourceDocPath = Path.IsPathRooted(源路径) ? 源路径 : Path.GetFullPath(源路径);
                string paramExportFilePath = Path.IsPathRooted(目标路径) ? 目标路径 : Path.GetFullPath(目标路径);

                if (File.Exists(paramExportFilePath) || !File.Exists(paramSourceDocPath.ToString()))
                {
                    return false;
                }

                MSWord.WdExportFormat paramExportFormat =  Word.WdExportFormat.wdExportFormatPDF;

                wordDocument = wordApplication.Documents.Open(
                        ref paramSourceDocPath, paramMissing, paramMissing,
                         paramMissing, paramMissing, paramMissing,
                        paramMissing, paramMissing, paramMissing,
                        paramMissing, paramMissing, paramMissing,
                        paramMissing, paramMissing, paramMissing,
                        paramMissing);

                if (wordDocument != null)
                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
                            paramExportFormat, paramOpenAfterExport,
                            paramExportOptimizeFor, paramExportRange, paramStartPage,
                            paramEndPage, paramExportItem, paramIncludeDocProps,
                            paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                            paramBitmapMissingFonts, paramUseISO19005_1,
                            paramMissing);
                result = true;

            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(paramMissing, paramMissing, paramMissing);
                }
                wordApplication.Quit();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                信号量组[信号量.可用应用].Release();
            }

            return result;
        }

        #endregion

        #region 进程控制
        public override void 创建进程()
        {
            创建进程(信号量.Word应用, ref 应用量, 信号量.Word文档, ref 闲置数);
        }

        /// <summary>
        /// 开始调用/交换状态
        /// </summary>
        public override void 开始工作()
        {
            开始工作(信号量.Word文档,ref 闲置数);
        }

        /// <summary>
        /// 结束调用/交换状态
        /// </summary>
        public override void 结束工作()
        {
            关闭Doc();
            结束工作(信号量.Word文档,ref 闲置数);
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        public override void 进程退出()
        {
            //立刻通知
            进程退出(信号量.Word应用, ref 应用量, 信号量.Word文档, ref 闲置数); 
            关闭Doc();
            if (_AppMain != null)
            {
                if (LifeSpan.Equals(生命周期.自管))
                {
                    lock (队列)
                    {
                        队列.Remove(this);
                    }
                }
                object Nothing = Missing.Value;
                _AppMain.Quit(ref Nothing, ref Nothing, ref Nothing);
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


