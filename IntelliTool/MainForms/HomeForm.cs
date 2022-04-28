using ModelLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelliTool
{
    public partial class HomeForm : BaseForm
    {
        private static HomeForm form;
        private static readonly object locker = new object();
        public static HomeForm GetForm()
        {
            if (form == null)
            {
                lock (locker)
                {
                    if (form == null)
                    {
                        form = new  HomeForm();
                    }
                }
            }
            return form;
        }

        private HomeForm()
        {
            List<Task> tasks = new List<Task>();
            if (Program._excel == null)
            {
                lock (Program.ExceLocker)
                {
                    if (Program._excel == null)
                    {
                        Task t1 = new Task(() =>
                        {
                            Program._excel = ExcelUnit.NewInstance();
                        });
                        t1.Start();
                        tasks.Add(t1);
                    }
                }
            }
            if (Program._docx == null)
            {
                lock (Program.DocxLocker)
                {
                    if (Program._docx == null)
                    {
                        Task t2 =  new Task(() =>
                        {
                            Program._docx = DocxUnit.NewInstance();
                        });
                        t2.Start();
                        tasks.Add(t2);
                    }
                }
            }
            Task.WaitAll(tasks.ToArray());
            if (Program._excel == null)
            {
                MessageBox.Show("Excel进程无法创建，请检查系统是否有Office Excel的相关版本");
                Dispose(true);
            }
            if (Program._docx == null)
            {
                MessageBox.Show("Word进程无法创建，请检查系统是否有Office Word的相关版本");
                Dispose(true);
            }

            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

        }

        private void Excel_Click(object sender, EventArgs e)
        {
            Visible = false;
            ExcelForm.GetForm().ShowDialog();
        }

        private void Word_Click(object sender, EventArgs e)
        {
            Visible = false;
            WordForm.GetForm().ShowDialog();
        }
    }
}
