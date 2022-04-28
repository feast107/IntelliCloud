using IntelliTool.SupForms;
using ModelLib;
using System;
using System.Collections;
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
using static ModelLib.Logics;
using static ModelLib.Logics.ExcelResult;

namespace IntelliTool
{
    public partial class ExcelForm : BaseForm
    {
        
        private bool Make = false;
        private bool Reset = false;
        private CancellationTokenSource 取消令牌 = new CancellationTokenSource();
        private string TmpDir;
       
        #region 静态区
        private static ExcelForm form;
        private static ExcelUnit _unit;
        private static readonly object 锁 = new object();
        private readonly static BindingList<预览> Previews = new BindingList<预览>();
        private readonly static BindingList<进度> files = new BindingList<进度>();
        private readonly static List<Task> 执行队列 = new List<Task>( );
        private readonly static Dictionary<ExcelTask, CancellationTokenSource> 任务队列 = new Dictionary<ExcelTask, CancellationTokenSource>();
        #endregion
        
        public static ExcelForm GetForm()
        {
            if (form == null)
            {
                lock (锁)
                {
                    if (form == null)
                    {
                        form = new ExcelForm();
                        if (_unit == null)
                        {
                            form = null;
                        }
                    }
                }
            }
            return form;
        }
        private ExcelForm()
        {
            if (Program._excel != null)
            {
                _unit = Program._excel;
            }
            InitializeComponent();
            FileView.DataSource = files;
            DataView.DataSource = Previews;
        }

        protected override void OnClosed(EventArgs e)
        {
            Visible = false;
            HomeForm.GetForm().Visible = true;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (!Make)
            {
                ExcelSearchForm.GetForm().ShowDialog();
            }
            else
            {
                MessageBox.Show("导出任务正在执行中", Program.项目提示());
            }
        }
        private void ExportButton_Click(object sender, EventArgs e)
        {
            
            if (!Make)
            {
                if (执行队列.Count == 0)
                {
                    MessageBox.Show("请添加至少一个任务", Program.项目提示());
                    return;
                }
                if (!执行队列[^1].IsCompleted)
                {
                    MessageBox.Show("有未完成的任务", Program.项目提示());
                    return;
                }
                ExportButton.Text = "导出中...";
                List<ExcelResult> 主表 = new List<ExcelResult>();
                List<ExcelResult> 子表 = new List<ExcelResult>();
                foreach (var E in 任务队列)
                {
                    if (E.Key.进度.status == 工作状态.完成)
                    {
                        switch (E.Key.类型)
                        {
                            case 文件类型.Excel主表:
                                主表.Add(E.Key.GetResult());
                                break;
                            case 文件类型.Excel子表:
                                子表.Add(E.Key.GetResult());
                                break;
                        }
                    }
                }
                if (TmpDir != null)
                {
                    Make = true;
                    string str = DateTime.Now.ToString("yyyyMMddhhmmss");
                    int i = 0;
                    while (File.Exists(Path.Combine(TmpDir, "导出" + str + ".xlsx")))
                    {
                        str += i++;
                    }
                    new Task(() =>
                    {
                        _unit.创建Excel(Path.Combine(TmpDir, "导出" + str + ".xlsx"));
                        _unit.选择Sheet(1);
                        try
                        {
                            Logics.写入表(_unit, 主表, 子表);
                        }
                        catch
                        {
                            MessageBox.Show("生成发生了异常", Program.项目提示());
                        }
                        finally
                        {
                            _unit.关闭Excel();
                            Make = false;
                            ExportButton.Invoke(new MethodInvoker(() =>
                            {
                                ExportButton.Text = "导出";
                            }));
                        }
                        MessageBox.Show("完成", Program.项目提示());
                    }).Start();
                }
            }
            else
            {
                MessageBox.Show("生成任务正在进行中", Program.项目提示());
            }

        }
        private void Reset_Click(object _,EventArgs e)
        {
            if (任务队列.Count == 0)
            {
                MessageBox.Show("请添加至少一个任务", Program.项目提示());
                return;
            }
            if (!Reset)
            {
                重置();
            }
            else
            {
                MessageBox.Show("正在重置", Program.项目提示());
            }
        }
        /// <summary>
        /// 接受来自ExcelSearchForm的参数
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="FilePath"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public bool ReceiveData(ExcelRule rule, string FilePath)
        {
            lock (files)
            {
                foreach (进度 s in files)
                {
                    if (s.文件.Equals(Path.GetFileName(FilePath)))
                    {
                        return false;
                    }
                }
            }
            进度 source = new 进度(Path.GetFileName(FilePath), FileView);
            files.Add(source);
            TmpDir = Path.GetDirectoryName(FilePath);
            CancellationTokenSource cancel = new CancellationTokenSource();
            ExcelTask task = new ExcelTask(rule, FilePath, source);
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
      
        /// <summary>
        /// 制作任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancel"></param>
        private void Actions(ExcelTask task, CancellationTokenSource cancel)
        {
            取消令牌.Token.ThrowIfCancellationRequested();
            cancel.Token.ThrowIfCancellationRequested();
            task.Execute(_unit,取消令牌.Token);
            取消令牌.Token.ThrowIfCancellationRequested();
            if (task.类型 == 文件类型.Excel子表)
            {
                if (task.GetResult() != null)
                {
                    new Task(() =>
                    {
                        List<预览> prev = 执行预览(task.GetResult());
                        lock (Previews)
                        {
                            if (DataView.InvokeRequired)
                            {
                                DataView.Invoke(new MethodInvoker(() =>
                                {
                                    prev.ForEach(x => Previews.Add(x));
                                    DataView.Refresh();
                                }));
                            }
                        }
                    }).Start();
                }
            }
        }
        private static List<预览> 执行预览(ExcelResult result)
        {
            Dictionary<int, 标题> title = 获取直接标题(result.标题);
            List<预览> pre = new List<预览>();
            List<string> strs = new List<string>();
            foreach (var p in title)
            {
                if (!strs.Contains(p.Value.全值))
                {
                    strs.Add(p.Value.全值);
                }
            }
            foreach (var p in result.行值对)
            {
                pre.Add(转化(p.Value, strs));
            }
            return pre;
        }
        private static 预览 转化(Hashtable table, List<string> titles)
        {
            int c = 0;
            预览 prev = new 预览();
            if (c < titles.Count)
            {
                prev.数据项1 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项2 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项3 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项4 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项5 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项6 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项7 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项8 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项9 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            c++;
            if (c < titles.Count)
            {
                prev.数据项10 = (table[titles[c]] ?? string.Empty).ToString();
            }
            else
            {
                return prev;
            }
            return prev;
        }
        private void 重置()
        {
            new Task(() =>
            {
                lock (锁)
                {
                    Reset = true;
                    取消令牌.Cancel();
                    Task.WaitAll(执行队列.ToArray());
                    TmpDir = null;
                    任务队列.Clear();
                    执行队列.Clear();
                    FileView.Invoke(method: new MethodInvoker(()=>
                    {
                        lock (files)
                        {
                            files.Clear(); FileView.Refresh();
                        }
                    }));
                    DataView.Invoke(method: new MethodInvoker(() => 
                    {
                        lock (Previews)
                        {
                            Previews.Clear(); DataView.Refresh();
                        }
                    }));
                    取消令牌 = new CancellationTokenSource( );
                    Reset = false;
                    MessageBox.Show("已重置", Program.项目提示());
                }
            }).Start();
        }
    }


    public class ExcelTask : Logics
    {
        public ExcelTask(ExcelRule rule, string Path,  进度 progress)
        {
            类型 = rule.匹配类型;
            Rule = rule;
            FilePath = Path;
            进度 = progress;
        }


        public ExcelTask(ExcelRule rule, string Path)
        {
            类型 = rule.匹配类型;
            Rule = rule;
            FilePath = Path;
        }

        public readonly 文件类型 类型;
        public readonly string FilePath;
        public ExcelResult Result;
        public ExcelRule Rule;
        public readonly 进度 进度;

        public void 计划()
        {
            进度.更新(工作状态.待处理);
        }

        public ExcelResult Test(ExcelUnit unit)
        {
            unit.打开Excel(FilePath);
            unit.选择Sheet(Rule.表单页);
            Result = 读取表(unit, Rule);
            return Result;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public ExcelResult Execute(ExcelUnit unit)
        {
            if (Result == null && 进度.status != 工作状态.处理中)
            {
                进度.更新(工作状态.处理中);
                try
                {
                    unit.打开Excel(FilePath);
                    unit.选择Sheet(Rule.表单页);
                    Result = 读取表(unit, Rule, 进度);
                    进度.更新(工作状态.完成);
                }
                catch (Exception e)
                {
                    进度.更新(工作状态.失败, e);
                    Result = null;
                }
                finally
                {
                    unit.关闭Excel();
                }
            }
            return Result;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public ExcelResult Execute(ExcelUnit unit,CancellationToken 令牌)
        {
            if (Result == null && 进度.status != 工作状态.处理中)
            {
                进度.更新(工作状态.处理中);
                try
                {
                    unit.打开Excel(FilePath);
                    unit.选择Sheet(Rule.表单页);
                    Result = 读取表(unit, Rule, 进度, 令牌);
                    进度.更新(工作状态.完成);
                }
                catch (Exception e)
                {
                    进度.更新(工作状态.失败, e);
                    Result = null;
                }
                finally
                {
                    unit.关闭Excel();
                }
            }
            return Result;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        public ExcelResult GetResult()
        {
            if (Result != null && 进度.status == 工作状态.完成)
            {
                return Result;
            }
            return null;
        }
    }

    public class 预览
    {
        public string 数据项1 { get; set; }
        public string 数据项2 { get; set; }
        public string 数据项3 { get; set; }
        public string 数据项4 { get; set; }
        public string 数据项5 { get; set; }
        public string 数据项6 { get; set; }
        public string 数据项7 { get; set; }
        public string 数据项8 { get; set; }
        public string 数据项9 { get; set; }
        public string 数据项10 { get; set; }
    }
}
