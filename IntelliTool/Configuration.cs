using ModelLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntelliTool
{
    public class Configuration
    {
        public static void InitControl(ref Control control, string Name)
        {
            control.Name = Name;
        }
        public static void InitControl(ref Control control, Size size)
        {
            control.Size = size;
        }
        public static void InitControl(ref Control control, Point Location)
        {
            control.Location = Location;
        }
        public static void InitControl(ref Control control, Bitmap Img)
        {
            control.BackgroundImage = Img;
        }

        #region
        #nullable enable
        public static void InitControl(Control control, string? Name, Size? Size, Point? Location, Bitmap? Img)
        {
            if (Name != null)
            {
                InitControl(ref control, Name);
            }
            if (Size != null)
            {
                InitControl(ref control,(Size)Size);
            }
            if (Location != null)
            {
                InitControl(ref control, (Point)Location);
            }
            if (Img != null)
            {
                InitControl(ref control, Img);
            }
        }
        #endregion
    }

    public class 进度 : 工作进度
    {
        public 进度(string Path,DataGridView 控件):base(Path)
        {
            this.控件 = 控件;
        }
        private readonly DataGridView 控件;

        public override void 更新(工作状态 状态, float? Step)
        {
            base.更新(状态,Step);
            if (控件.InvokeRequired)
            {
                控件.Invoke(new MethodInvoker(() =>
                {
                    控件.Refresh();
                }));
            }
            else
            {
                 控件.Refresh();
            }
        }

        public override void 更新(工作状态 状态)
        {
            base.更新(状态);
            if (控件 != null)
            {
                if (控件.InvokeRequired)
                {
                    控件.Invoke(new MethodInvoker(() =>
                    {
                        控件.Refresh();
                    }));
                }
                else
                {
                    控件.Refresh();
                }
            }
        }
        public override void 更新()
        {
            base.更新();
            if (控件 != null)
            {
                if (控件.InvokeRequired)
                {
                    控件.Invoke(new MethodInvoker(() =>
                    {
                        控件.Refresh();
                    }));
                }
                else
                {
                    控件.Refresh();
                }
            }
        }

        public override void 更新(工作状态 状态,Exception e)
        {
            base.更新(状态,e);
            if (控件 != null)
            {
                if (控件.InvokeRequired)
                {
                    控件.Invoke(new MethodInvoker(() =>
                    {
                        控件.Refresh();
                    }));
                }
                else
                {
                    控件.Refresh();
                }
            }
        }
    }
}
