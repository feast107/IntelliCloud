using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IntelliTool.效果
{
    public static class Transform
    { 
        private static void Refresh(Control control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(() =>
                {
                    control.Refresh();
                }));
            }
            else
            {
                control.Refresh();
                control.Refresh();
            }
        }
        public static void TransformWidth(Control control,float rate,bool Refresh)
        {
            control.Width = (int)(rate * (float)control.Width);
            if (Refresh)
            {
                Transform.Refresh(control);
            }
        }
        public static void TransformWidth(Control[] controls,float rate,bool Refresh)
        {
            foreach(Control control in controls)
            {
                TransformWidth(control, rate,Refresh);
            }
        }
        public static void TransformWidth(List<Control> controls,float rate,bool Refresh)
        {
            controls.ForEach(x => TransformWidth(x, rate,Refresh));
        }
    

        public static void TransformHeight(Control control,float rate,bool Refresh)
        {
            control.Height = (int)(rate * (float)control.Height);
            if (Refresh)
            {
                Transform.Refresh(control);
            }
        }
        public static void TransformHeight(Control[] controls, float rate,bool Refresh)
        {
            foreach (Control control in controls)
            {
                TransformHeight(control, rate,Refresh);
            }
        }
        public static void TransformHeight(List<Control> controls, float rate,bool Refresh)
        {
            controls.ForEach(x => TransformHeight(x, rate,Refresh));
        }

        public static void TransformBoth(Control control,float rate,bool Refresh)
        {
            control.Width = (int)(rate * (float)control.Width);
            control.Height = (int)(rate * (float)control.Height);
            if (Refresh)
            {
                Transform.Refresh(control);
            }
        }
        public static void TransformBoth(Control[] controls, float rate,bool Refresh)
        {
            foreach (Control control in controls)
            {
                TransformBoth(control, rate,Refresh);
            }
        }
        public static void TransformBoth(List<Control> controls, float rate,bool Refresh)
        {
            controls.ForEach(x => TransformBoth(x, rate,Refresh));
        }

    }

}
