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
    public partial class WordSearchForm : BaseForm
    {

        private static object 静态锁 = new object();
        private static WordSearchForm _form;

        private object 实例锁 = new object();
        private bool MainCheck=true;

        public const string 规则 = "规则";
        public const string 长度 = "长度";
        public static WordSearchForm GetForm()
        {
            if (_form == null)
            {
                lock (静态锁)
                {
                    if (_form == null)
                    {
                        _form = new WordSearchForm();
                    }
                }
            }
            return _form;
        }

        private WordSearchForm()
        {
            InitializeComponent();
            foreach (var i in Logics.获取匹配模式())
            {
                RulesCombo.Items.Add(i);
            }
        }

        private void AddBox_Click(object sender, EventArgs e)
        {
            
            if (MainCheck)
            {
                string s = InputBox.Text;
                if (s.Trim().Equals(string.Empty))
                {
                    MessageBox.Show("请勿添加空项", Program.项目提示());
                }
                foreach(TreeNode i in RulesView.Nodes)
                {
                    if (i.Text.Trim().Equals(s))
                    {
                        MessageBox.Show("请勿添加重复的项",Program.项目提示());
                        return;
                    }
                }
                this.RulesView.Nodes.Add(s);
            }
            else
            {
                if (RulesView.Nodes.Count == 0)
                {
                    MessageBox.Show("请至少添加一个项");
                    return;
                }
                string s = null;
                if (RulesCombo.SelectedItem != null)
                {
                    s = RulesCombo.SelectedItem.ToString();
                }
                else
                {
                    s = Logics.匹配模式.任意字符.ToString();
                }
                TreeNode node = 构造节点(Numberbox.Value.ToString(), s);

                if (RulesView.SelectedNode == null)
                {
                    if (RulesView.Nodes[^1].Nodes.Count != 0)
                    {
                        if(空检索(RulesView.Nodes[^1], s))
                        {
                            return;
                        }
                    }
                    RulesView.Nodes[^1].Nodes.Add(node);
                }
                else if(RulesView.SelectedNode.Parent==null)
                {
                    if (RulesView.SelectedNode.Nodes.Count != 0)
                    {
                        if(空检索(RulesView.SelectedNode, s))
                        {
                            return;
                        }
                    }
                    RulesView.SelectedNode.Nodes.Add(node);
                }
                else
                {
                    MessageBox.Show("仅允许跟随在父节点中",Program.项目提示());
                    return;
                }
            }
        }
        private bool 空检索(TreeNode node,string targ)
        {
            TreeNode tmp = null;
            bool flag = false;
            foreach (TreeNode i in node.Nodes[^1].Nodes)
            {
                if (i.Tag.ToString().Equals(长度))
                {
                    tmp = i;
                }
                if (i.Tag.ToString().Equals(规则) && i.Text.ToString().Trim().Equals(targ))
                {
                    flag = true;
                }
            }
            if (flag && tmp != null)
            {
                tmp.Text = (Convert.ToInt32(tmp.Text) + Numberbox.Value).ToString();
                node.Nodes[^1].Text = 长度+":" + tmp.Text + " "+规则+":" + targ;
            }
            return flag;
        }

        private TreeNode 构造节点(string Num,string rule)
        {
            TreeNode ret = new TreeNode();
            TreeNode length = new TreeNode();
            length.Tag = 长度;
            length.Text = Num;
            TreeNode Rule = new TreeNode();
            Rule.Tag = 规则;
            Rule.Text = rule;
            ret.Nodes.Add(length);
            ret.Nodes.Add(Rule);
            ret.Tag = "匹配";
            ret.Text = 长度+":"+Num+" "+规则+":"+rule;
            return ret;
        }
        private void SwitchPic_Click(object sender, EventArgs e)
        {
            if (MainCheck)
            {
                lock (实例锁)
                {
                    TargText.Text = "长度：";
                    InputBox.Visible = false;
                    Numberbox.Visible = true;
                    Numberbox.Value = 0;
                    RulesCombo.Visible = true;
                    RuleBox.Visible = true;
                    MainCheck = !MainCheck;
                }
            }
            else
            {
                lock (实例锁)
                {
                    TargText.Text = "前置字符：";
                    InputBox.Visible = true;
                    Numberbox.Visible = false;
                    Numberbox.Value = 0;
                    RulesCombo.Visible = false;
                    RuleBox.Visible = false;
                    MainCheck = !MainCheck;
                }
            }
        }

        private void ClosePic_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RemovePic_Click(object sender, EventArgs e)
        {
            if (RulesView.Nodes.Count == 0)
            {
                return;
            }
            if (RulesView.SelectedNode == null)
            {
                if (MessageBox.Show("是否删除最后一项？", Program.项目提示(), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    RulesView.Nodes[^1].Remove();
                    return;
                }
            }
            else if(! (RulesView.SelectedNode.Parent!=null && RulesView.SelectedNode.Nodes.Count==0))
            {
                RulesView.SelectedNode.Remove();
                return;
            }
            else
            {
                MessageBox.Show("请勿拆解匹配节点", Program.项目提示());
                return;
            }
        }

        private Dictionary<string, List<WordRule>> 转化()
        {
            Dictionary<string, List<WordRule>> ret = new Dictionary<string, List<WordRule>>();
            //根节点
            foreach (TreeNode n in RulesView.Nodes)
            {
                
                if (n.Nodes.Count != 0)
                {
                    List<WordRule> tmp = new List<WordRule>(); 
                    //子节点
                    foreach (TreeNode c in n.Nodes)
                    {
                        int length=0;
                        string Mode = string.Empty;
                        foreach(TreeNode a in c.Nodes)
                        {
                            if (a.Tag.Equals(长度))
                            {
                                length = Convert.ToInt32(a.Text);
                            }
                            if (a.Tag.Equals(规则))
                            {
                                Mode = a.Text;
                            }
                        }
                        tmp.Add(new WordRule(ModelLib.Configuration.str转模式(Mode), length));
                    }
                    ret.Add(n.Text.Trim(), tmp);
                }
            }
            return ret;
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "(全部Word) | *.docx; *.doc | (*.docx) | *.docx | (*.doc) | *.doc";
            openFile.ShowDialog();
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFile = new FolderBrowserDialog();
            
        }
    }
}
