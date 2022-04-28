using ModelLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelliTool.SupForms
{
    public partial class ExcelSearchForm : BaseForm
    {
        public bool OneTitle = true;
        public static readonly object 静态锁 = new object();
        public static readonly object 实例锁 = new object();

        private const int 最大主键数 = 3;

        private static ExcelSearchForm 页面;

        private List<KeyComponent> 次级主键组 = new List<KeyComponent>();
        private PicComponent 消除钮;
        public static ExcelSearchForm GetForm()
        {
            if (页面 == null)
            {
                lock (静态锁)
                {
                    if (页面 == null)
                    {
                        页面 = new ExcelSearchForm();
                    }
                }
            }
            return 页面;
        }

        private ExcelSearchForm()
        {
            InitializeComponent();
        }
        private void Comparse()
        {
            if (!OneTitle)
            {
                lock (实例锁)
                {
                    TitleLable.Text = "标题起始行：";
                    TitleEndLable.Visible = true;
                    TitleEndRow.Visible = true;
                }
            }
            else
            {
                lock (静态锁)
                {
                    TitleLable.Text = "      标题行：";
                    TitleEndLable.Visible = false;
                    TitleEndRow.Visible = false;
                }
            }
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            lock (实例锁)
            {
                OneTitle = !OneTitle;
                Comparse();
            }
        }
        private void FileUpLoad_Click(object sender, EventArgs e)
        {
            ExcelRule rule = null;
            if (MainTable.Checked)
            {
                rule = new ExcelRule(文件类型.Excel主表);
            }
            else
            {
                rule = new ExcelRule(文件类型.Excel子表);
            }
            rule.起始列 = (int)StartCol.Value;
            rule.结束列 = (int)EndCol.Value;
            if (rule.起始列 >= rule.结束列 && (rule.结束列!=0||rule.起始列!=0) )
            {
                MessageBox.Show("结束列须大于起始列", Program.项目提示());
                return;
            }
            rule.表单页 = (int)Page.Value;
            rule.数据起始行 = (int)DataStartRow.Value;
            rule.数据结束行 = (int)DataEndRow.Value;
            if (rule.数据起始行 > rule.数据结束行 && rule.数据结束行!=0)
            {
                MessageBox.Show("数据结束行须大于起始行", Program.项目提示());
                return;
            }
            rule.标题起始行 = (int)TitleRow.Value;
            if (OneTitle)
            {
                rule.标题结束行 = rule.标题起始行;
            }
            else
            {
                rule.标题结束行 = (int)TitleEndRow.Value;
            }
            if (rule.标题结束行 > rule.数据起始行 && (rule.数据起始行!=0))
            {
                MessageBox.Show("标题行须小于数据起始行", Program.项目提示());
                return;
            }
            if (rule.标题结束行 < rule.标题起始行)
            {

                MessageBox.Show("标题起始行须小于标题结束行",Program.项目提示());
                return;
            }
            List<int> ints = new List<int>();
            ints.Add((int)KeyCol.Value);
            次级主键组.ForEach(x =>
            {
                int i = (int)x.控件.Value;
                
                if (ints.Contains(i))
                {
                    ints.Add(i);
                }
            });
            rule.主键列 = new int[ints.Count];
            for (int i = 0; i < ints.Count; i++)
            {
                if (rule.起始列 != 0 || rule.结束列 != 0)
                {
                    if (ints[i] > rule.结束列 || ints[i] < rule.起始列)
                    {
                        MessageBox.Show("主键列必须在给定的列范围内",Program.项目提示());
                        return;
                    }
                }
                if (ints[i] == 0)
                {
                    MessageBox.Show("主键列不可空", Program.项目提示());
                    return;
                }
                rule.主键列[i] = ints[i];
            }
            if (MainTable.Checked)
            {
                rule.是否主表 = true;
            }
            else
            {
                rule.是否主表 = false;
            }
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!ExcelForm.GetForm().ReceiveData(rule, FileDialog.FileName))
                {
                    MessageBox.Show("已上传过选择的文件", Program.项目提示());
                }
                else
                {
                    Close();
                }
            }
        }
        private void AddBox_Click(object sender, EventArgs e)
        {
            lock (次级主键组)
            {
                if (次级主键组.Count >= 最大主键数-1)
                {
                    return;
                }
                次级主键组.Add(new KeyComponent(this));
                if (次级主键组.Count != 0 && 消除钮==null)
                {
                    消除钮 = new PicComponent(this);
                }
            }
        }
        private void AddBox_Hover(object sender,EventArgs e)
        {
            panel2.Visible = true;
        }
        private void AddBox_Leave(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }
        private void SwitchBox_Hover(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }
        private void SwitchBox_Leave(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }
        private void Remove_Click(object sender, EventArgs e)
        {
            lock (消除钮)
            {
                if (次级主键组.Count != 0)
                {
                    次级主键组[^1].Release();
                    次级主键组.RemoveAt(次级主键组.Count - 1);
                    if (次级主键组.Count == 0)
                    {
                        消除钮.Release();
                        消除钮 = null;
                    }
                }
            }
          
        }

        private class KeyComponent
        {
            public static Point 基坐标 = new Point(114, 175);
            public static int 行高 = 36;
            public static int Count = 0;

            public NumericUpDown 控件;

            private ExcelSearchForm 页面;

            public KeyComponent(ExcelSearchForm form)
            {
                this.页面 = form;
                控件 = new NumericUpDown();
                基坐标.Y += 行高; 
                控件.ImeMode = System.Windows.Forms.ImeMode.Off;
                Configuration.InitControl(控件, "KeyCol" + Count,new Size(100,23),new Point(基坐标.X,基坐标.Y),null);
                Count++;
                控件.Visible = true;
                控件.Show();
                form.panel1.Controls.Add(控件);
                ((System.ComponentModel.ISupportInitialize)(this.控件)).EndInit();
            }

            public void Release()
            {
                页面.panel1.Controls.Remove(控件);
                基坐标.Y -= 行高;
                Count--;
                控件.Dispose();
            }

        }

        private class PicComponent
        {
            private static readonly Bitmap 资源 = Properties.Resources.removeicon_small;
            public PictureBox 消除钮;

            private readonly ExcelSearchForm 页面;
            public PicComponent(ExcelSearchForm form)
            {
                this.页面 = form;
                消除钮 = new PictureBox();
                Configuration.InitControl(消除钮, "RemoveBox", new Size(25, 25), new Point(220, 175 + KeyComponent.行高),资源);
                消除钮.TabStop = false;
                消除钮.Click += new System.EventHandler(form.Remove_Click);
                消除钮.Visible = true;
                消除钮.Show();
                form.panel1.Controls.Add(消除钮);
                ((System.ComponentModel.ISupportInitialize)(消除钮)).EndInit();
            }

            public void Release()
            {
                消除钮.Visible = false;
                页面.panel1.Controls.Remove(消除钮);
                消除钮.Dispose();
            }
        }

    }
}
