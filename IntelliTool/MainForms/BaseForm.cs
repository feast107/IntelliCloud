using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IntelliTool
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopPanel.MouseDown += TopPanel_MouseDown;
        }





        #region 引用系统函数
        const int WS_EX_LAYERED = 0x00080000;
        const int WM_NCHITTEST = 0x0084;
        const int HIT_LEFT = 10;
        const int HIT_RIGHT = 11;
        const int HIT_TOP = 12;
        const int HIT_TOPLEFT = 13;
        const int HIT_TOPRIGHT = 14;
        const int HIT_BOTTOM = 15;
        const int HIT_BOTTOMLEFT = 16;
        const int HIT_BOTTOMRIGHT = 17;
        const int AC_SRC_OVER = 0x00;
        const int AC_SRC_ALPHA = 0x01;
        const int ULW_ALPHA = 0x00000002;
        const int GW_HWNDNEXT = 2;
        const int WM_SYSCOMMAND = 0x0112;
        const int HTCAPTION = 2;
        const int WM_NCLBUTTONDBLCLK = 163;

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize,
            IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern Bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

        public enum Bool
        {
            False = 0,
            True
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public enum MouseDirection
        {

            Herizontal,//水平方向拖动，只改变窗体的宽度  

            Vertical,//垂直方向拖动，只改变窗体的高度  

            Declining,//倾斜方向，同时改变窗体的宽度和高度

            None//不做标志，即不拖动窗体改变大小

        }

        bool isMouseDown = false; //表示鼠标当前是否处于按下状态，初始值为否 
        MouseDirection direction = MouseDirection.None;//表示拖动的方向，起始为None，表示不拖动
        Point mouseOff;//鼠标移动位置变量  
        #endregion
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int Index, long Value);

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int SC_MOVE = 0xF010;
        /// <summary>
        /// 为了是主界面能够移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        protected void InitFrame(Control.ControlCollection controls)
        {
            controls.Add(this.ContentPanel);
            controls.Add(this.TopPanel);
        }

        protected void MinimunPic_Hover(object sender, EventArgs e)
        {
            this.MinimunPic.BackColor = Color.LightGray;
        }
        protected void MinimunPic_Leave(object sender, EventArgs e)
        {
            this.MinimunPic.BackColor = SystemColors.Control;
            this.MinimunPic.Refresh();
        }
        protected void MinimunPic_Prepare(object sender, EventArgs e)
        {
            this.MinimunPic.BackColor = Color.Gray;
            this.MinimunPic.Refresh();
        }

        protected void MinimunPic_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        protected void ClosePic_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void ClosePic_Hover(object sender, EventArgs e)
        {
            this.ClosePic.BackColor = Color.Red;
            this.ClosePic.Refresh();
        }

        protected void ClosePic_Prepare(object sender, EventArgs e)
        {
            this.ClosePic.BackColor = Color.DarkRed;
            this.ClosePic.Refresh();
        }
        protected void ClosePic_Leave(object sender, EventArgs e)
        {
            this.ClosePic.BackColor = SystemColors.Control;
            this.ClosePic.Refresh();
        }


        private void BaseForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOff = new Point(-e.X, -e.Y); //记录鼠标位置
                                              //当鼠标的位置处于边缘时，允许进行改变大小。

            if (e.Location.X >= this.Width - 10 && e.Location.Y > this.Height - 10)
            {
                isMouseDown = true;
            }
            else if (e.Location.X >= this.Width - 5)
            {
                isMouseDown = true;
            }
            else if (e.Location.Y >= this.Height - 5)
            {
                isMouseDown = true;
            }
            else
            {
                this.Cursor = Cursors.Arrow;//改变鼠标样式为原样
                isMouseDown = false;
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);//鼠标移动事件
            }
        }


        private void BaseForm_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("松开鼠标");
            isMouseDown = false;
            direction = MouseDirection.None;
            if (isMouseDown)
                isMouseDown = false;
        }


        private void BaseForm_MouseMove(object sender, MouseEventArgs e)
        {
            //鼠标移动到边缘，改变鼠标的图标
            if (e.Location.X >= this.Width - 5)
            {
                this.Cursor = Cursors.SizeWE;
                direction = MouseDirection.Herizontal;
            }
            else if (e.Location.Y >= this.Height - 5)
            {
                this.Cursor = Cursors.SizeNS;
                direction = MouseDirection.Vertical;
            }
            //否则，以外的窗体区域，鼠标星座均为单向箭头（默认）             
            else
            {
                this.Cursor = Cursors.Arrow;

            }
            if (e.Location.X >= (this.Width + this.Left + 10) || (e.Location.Y > this.Height + this.Top + 10))
            {
                isMouseDown = false;
            }

            //设定好方向后，调用下面方法，改变窗体大小  
            ResizeWindow();
        }

        private void ResizeWindow()
        {

            if (!isMouseDown)
                return;

            if (direction == MouseDirection.Herizontal)
            {
                this.Cursor = Cursors.SizeWE;
                this.Width = MousePosition.X - this.Left + 5;//改变宽度
            }
            else if (direction == MouseDirection.Vertical)
            {
                this.Cursor = Cursors.SizeNS;
                this.Height = MousePosition.Y - this.Top + 5;//改变高度
            }
            //鼠标不在窗口右和下边缘，把鼠标打回原型
            else
            {
                this.Cursor = Cursors.Arrow;
                isMouseDown = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                                   (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HIT_TOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)HIT_BOTTOMLEFT;
                        else m.Result = (IntPtr)HIT_LEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HIT_TOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)HIT_BOTTOMRIGHT;
                        else m.Result = (IntPtr)HIT_RIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HIT_TOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HIT_BOTTOM;
                    break;
                case 0x0201://鼠标左键按下的消息 用于实现拖动窗口功能
                    m.Msg = 0x00A1;//更改消息为非客户区按下鼠标
                    m.LParam = IntPtr.Zero;//默认值
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        //private void Form1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    this.Capture = false;
        //    Form ff = new Form();
        //    ff.StartPosition = FormStartPosition.Manual;
        //    ff.Size = this.Size;
        //    ff.Location = this.Location;
        //    //ff.Show();不让窗体显示
        //    SendMessage(ff.Handle, 274, 61440 + 1, 0);//发送移动信息,也可以发送其它比如缩放消息
        //    this.Size = ff.Size;
        //    this.Location = ff.Location;
        //    ff.Dispose();

        //}
    }
}
