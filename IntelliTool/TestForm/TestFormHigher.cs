using IntelliTool.效果;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelliTool.TestForm
{
    public partial class TestFormHigher : BaseForm
    {
        public TestFormHigher()
        {
            InitializeComponent();
        }

        private void ContentPanel_Click(object sender, EventArgs e)
        {

        }

        private void Button_Hover(object o,EventArgs e)
        {
            Transform.TransformWidth(this.button1,2f,false);
            this.Refresh();
        }

        private void Button_Leave(object o,EventArgs e)
        {
            Transform.TransformWidth(this.button1, 1/2f,false);
            this.Refresh();
        }
    }
}
