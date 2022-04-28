using IntelliTool.SupForms;
using ModelLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelliTool
{
    public partial class WordForm : BaseForm
    {
        private bool Make = false;
        private bool Reset = false;
        private CancellationTokenSource 取消令牌 = new CancellationTokenSource();
        private string TmpDir;


        #region 静态区
        private static DocxUnit _unit;
        private static WordForm form;
        private readonly static object 锁 = new object();
        private readonly static BindingList<预览> Previews = new BindingList<预览>();
        private readonly static BindingList<进度> files = new BindingList<进度>();
        private readonly static List<Task> 执行队列 = new List<Task>();
        private readonly static Dictionary<WordTask, CancellationTokenSource> 任务队列 =new Dictionary<WordTask, CancellationTokenSource>();
        #endregion

        public static WordForm GetForm()
        {
            if (form == null)
            {
                lock (锁)
                {
                    if (form == null)
                    {
                        form = new WordForm();
                        if (_unit == null)
                        {
                            form = null;
                        }
                    }
                }
            }
            return form;
        }

        private WordForm()
        {
            InitializeComponent();
            if (Program._docx != null)
            {
                _unit = Program._docx;
            }
            FileView.DataSource = files;
            DataView.DataSource = Previews;
        }

        protected override void OnClosed(EventArgs e)
        {
            Visible = false;
            HomeForm.GetForm().Visible = true;
        }
        private void Reset_Click(object sender, EventArgs e)
        {

        }
        private void ImportButtton_Click(object sender, EventArgs e)
        {
            WordSearchForm.GetForm().ShowDialog();
        }
        private void ExportButton_Click(object sender, EventArgs e)
        {

        }
        public bool ReceiveData(Dictionary<string,List<WordRule>> rules,string FilePath)
        {
            if (files.Count != 0)
            {
                foreach (var i in files)
                {
                    if (i.文件.Equals(Path.GetFileName(FilePath)))
                    {
                        return false;
                    }
                }
            }
            进度 pro = new 进度(Path.GetFileName(FilePath), FileView);
            files.Add(pro);
            TmpDir = Path.GetDirectoryName(FilePath);
            CancellationTokenSource cancel = new CancellationTokenSource();
            WordTask task = new WordTask(rules, FilePath, 文件类型.Word, pro);
            任务队列.Add(task, cancel);
            Task t = null;
            if (执行队列.Count == 0)
            {
                lock (执行队列)
                {
                    if (执行队列.Count == 0)
                    {
                        task.计划();
                        t = new Task(() =>
                        {
                            Actions(task, cancel);
                        }, cancel.Token);
                        t.Start();
                    }
                    else
                    {
                        task.计划();
                        t = 执行队列[^1].ContinueWith((T) =>
                        {
                            Actions(task, cancel);
                        }, cancel.Token);

                    }
                    执行队列.Add(t);
                }
            }
            else
            {
                lock (执行队列)
                {
                    task.计划();
                    t = 执行队列[^1].ContinueWith((T) =>
                    {
                        Actions(task, cancel);
                    }, cancel.Token);
                    执行队列.Add(t);
                }
            }
            return true;

        }
        private void Actions(WordTask task,CancellationTokenSource cancel)
        {

            取消令牌.Token.ThrowIfCancellationRequested();
            cancel.Token.ThrowIfCancellationRequested();
            task.Execute(_unit, 取消令牌.Token);
            取消令牌.Token.ThrowIfCancellationRequested();
            if (task.类型 == 文件类型.Word)
            {
                if (task.GetResult() != null)
                {
                    new Task(() =>
                    {
                        预览 prev = 执行预览(task.GetResult());
                        lock (Previews)
                        {
                            if (DataView.InvokeRequired)
                            {
                                DataView.Invoke(new MethodInvoker(() =>
                                {
                                    Previews.Add(prev);
                                    DataView.Refresh();
                                }));
                            }
                        }
                    }).Start();
                }
            }
        }
        private 预览 执行预览(Dictionary<string,Logics.MatchResult> Result)
        {
            List<string> targ = Result.Keys.ToList();
            int i = 0;
            预览 ret = new 预览();
            if (i < targ.Count)
            {
                ret.数据项1 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项2 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项3 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项4 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项5 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项6 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项7 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项8 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项9 = Result[targ[i]].匹配结果;
                i++;
            }
            if (i < targ.Count)
            {
                ret.数据项10 = Result[targ[i]].匹配结果;
            }
            return ret;
        }
    }

    public class WordTask : Logics
    {
        public WordTask(Dictionary<string,List<WordRule>> Target,string FilePath,文件类型 类型,进度 进度)
        {
            目标 = Target;
            路径 = FilePath;
            this.类型 = 类型;
            this.进度 = 进度;
        }
        public readonly 文件类型 类型;
        private readonly string 路径;
        private readonly Dictionary<string, List<WordRule>> 目标;
        private Dictionary<string, MatchResult> Result = null;
        private readonly 进度 进度;

        public void 计划()
        {
            进度.更新(工作状态.待处理);
        }
        public Task<Dictionary<string, MatchResult>> Execute(DocxUnit unit)
        {
            if ((Result == null && 进度.status == 工作状态.创建) || 进度.status == 工作状态.失败)
            {
                if (File.Exists(路径))
                {
                    try
                    {
                        进度.更新(工作状态.处理中);
                        Result = new Dictionary<string, MatchResult>();
                        unit.打开Doc(路径);
                        Logics.获取结果( unit, 目标);
                        进度.更新(工作状态.完成);
                    }
                    catch
                    {
                        Result = null;
                        进度.更新(工作状态.失败);
                    }
                    finally
                    {
                        unit.关闭Doc();
                    }
                }
            }
            return Task.FromResult(Result);
        }
        public Task<Dictionary<string, MatchResult>> Execute(DocxUnit unit,CancellationToken token)
        {
            if ((Result == null && 进度.status == 工作状态.创建) || 进度.status == 工作状态.失败)
            {
                if (File.Exists(路径))
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        进度.更新(工作状态.处理中);
                        Result = new Dictionary<string, MatchResult>();
                        unit.打开Doc(路径);
                        Result = Logics.获取结果(unit, 目标, token);
                        进度.更新(工作状态.完成);
                    }
                    catch
                    {
                        Result = null;
                        进度.更新(工作状态.失败);
                    }
                    finally
                    {
                        unit.关闭Doc();
                    }
                }
            }
            return Task.FromResult(Result);
        }
        public Dictionary<string, MatchResult> GetResult()
        {
            return Result;
        }
    }
}
