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
    public partial class TransparentPanel : Control
    {
        public TransparentPanel()
        {
            this.BringToFront();
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //不进行背景的绘制
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //绘制panel的背景图像
            if (BackgroundImage != null) e.Graphics.DrawImage(this.BackgroundImage, new Point(0, 0));
        }

        private int num1 = 1;

        [Bindable(true), Category("自定义属性栏"), DefaultValue(1), Description("此处为自定义属性Attr1的说明信息！")]
        public int Attr1
        {
            get { return num1; }
            set { this.Invalidate(); }
        }
    
    }
}
