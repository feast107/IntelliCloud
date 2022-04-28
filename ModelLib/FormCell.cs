using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ModelLib
{
    public class FormCell
    {
        #region 静态区
        static private float 数量 = 0f;
        public const string 下增生内容 = "";
        public const string 右增生内容 = "";
        #endregion

        public enum Formation{
            原生,
            下增生,
            右增生
        }
        public enum 方向
        {
            水平,
            垂直
        }

        public FormCell 右侧;

        public FormCell 下侧;


        public readonly Formation 形态;

        private readonly float 编号 = 0f;

        public readonly bool 临时 = false;

        public string 内容;

        public Rectangle 区域;

        public float 置信度;

        public int 行;

        public int 列;

        public Point 中心;

        private readonly FormSheet 所属;

        /// <summary>
        /// 原生的单元格
        /// </summary>
        /// <param name="sheet"></param>
        public FormCell(FormSheet sheet) { 编号 = 数量++; 所属 = sheet;形态 = Formation.原生; }

        /// <summary>
        /// 增生的单元格
        /// </summary>
        /// <param name="新区域"></param>
        /// <param name="置信度"></param>
        /// <param name="行"></param>
        /// <param name="列"></param>
        /// <param name="内容"></param>
        /// <param name="sheet"></param>
        private FormCell(Rectangle 新区域, float 置信度, int 行, int 列,方向 direction, FormSheet sheet)
        {
           
            编号 = 数量++;
            区域 = 新区域;
            this.置信度 = 置信度;
            this.行 = 行;
            this.列 = 列;
            switch (direction)
            {
                case 方向.水平:
                    this.内容 = 下增生内容;
                    形态 = Formation.下增生;
                    break;
                case 方向.垂直:
                    内容 = 右增生内容;
                    形态 = Formation.右增生;
                    break;
            }
            所属 = sheet;
            所属.确信体.Add(this);
        }

        public int 获取底坐标()
        {
            return 区域.Y + 区域.Height;
        }

        public int 获取右坐标()
        {
            return 区域.X + 区域.Width;
        }

        public void 合并内容(FormCell cell)
        {
            if (!内容.Equals(cell.内容))
                内容 += cell.内容;
        }

        public bool 右优先挂载(FormCell cell)
        {
            if (右侧 == cell)
            {
                return true;
            }
            //如果在右侧
            if (FormSheet.Contain(cell.区域.X, 获取右坐标()))
            {
                //如果邻接
                if (FormSheet.Compare(cell.区域.X, 获取右坐标()))
                {
                    //并排
                    if (FormSheet.Compare(cell.区域.Y, 区域.Y))
                    {
                        //挂载
                        if (右侧 == null)
                        {
                            右侧 = cell;
                            cell.通知分割(区域.Height, 区域.Width, 方向.水平);
                            检查分割();
                            return true;
                        }
                        //与右侧合并
                        else
                        {

                            合并内容(cell);

                            return true;
                        }
                    }
                    //不并排
                    else if (右侧 != null)
                    {
                        if (右侧.下优先挂载(cell))
                        {
                            检查分割();
                            return true;
                        }
                    }
                }
                //不邻接
                else if (右侧 != null && 右侧.右优先挂载(cell))
                {
                    检查分割();
                    return true;
                }

            }

            //如果在下侧
            if (FormSheet.Contain(cell.区域.Y, 获取底坐标()))
            {
                //如果邻接
                if (FormSheet.Compare(cell.区域.Y, 获取底坐标()))
                {
                    //并列
                    if (FormSheet.Compare(cell.区域.X, 区域.X))
                    {
                        if (下侧 == null)
                        {
                            下侧 = cell;
                            cell.通知分割(区域.Height, 区域.Width, 方向.垂直);
                            检查分割();
                            return true;
                        }
                        else
                        {

                            //与下侧合并
                            合并内容(cell);

                            return true;
                        }
                    }
                    //不并列
                    else if (下侧 != null)
                    {
                        if (下侧.右优先挂载(cell))
                        {
                            检查分割();
                            return true;
                        }
                    }
                }
                //不邻接
                else if (下侧 != null && 下侧.下优先挂载(cell))
                {
                    检查分割();
                    return true;
                }
            }

            return false;
        }

        public bool 下优先挂载(FormCell cell)
        {
            if (下侧 == cell)
            {
                return true;
            }
            //如果在下侧
            if (FormSheet.Contain(cell.区域.Y, 获取底坐标()))
            {
                //如果邻接
                if (FormSheet.Compare(cell.区域.Y, 获取底坐标()))
                {
                    //并列
                    if (FormSheet.Compare(cell.区域.X, 区域.X))
                    {
                        if (下侧 == null)
                        {
                            下侧 = cell;
                            cell.通知分割(区域.Height, 区域.Width, 方向.垂直);
                            检查分割();
                            return true;
                        }
                        else
                        {
                            //与下侧合并
                            合并内容(cell);
                            return true;
                        }
                    }
                    //不并列
                    else if (下侧 != null)
                    {
                        if (下侧.右优先挂载(cell))
                        {
                            检查分割();
                            return true;
                        }
                    }
                }
                //不邻接
                else if (下侧 != null && 下侧.下优先挂载(cell))
                {
                    检查分割();
                    return true;
                }
            }

            //如果在右侧
            if (FormSheet.Contain(cell.区域.X, 获取右坐标()))
            {
                //如果邻接
                if (FormSheet.Compare(cell.区域.X, 获取右坐标()))
                {
                    //并排
                    if (FormSheet.Compare(cell.区域.Y, 区域.Y))
                    {
                        //挂载
                        if (右侧 == null)
                        {
                            右侧 = cell;
                            cell.通知分割(区域.Height, 区域.Width, 方向.水平);
                            检查分割();
                            return true;
                        }
                        //与右侧合并
                        else
                        {
                            合并内容(cell);
                            return true;
                        }
                    }
                    //不并排
                    else if (右侧 != null)
                    {
                        if (右侧.下优先挂载(cell))
                        {
                            检查分割();
                            return true;
                        }
                    }
                }
                //不邻接
                else if (右侧 != null && 右侧.右优先挂载(cell))
                {
                    检查分割();
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 分割自身
        /// </summary>
        private void 检查分割()
        {
            if (右侧 != null)
            {
                if (!FormSheet.Compare(高度, 右侧.高度))
                {
                    int Height = 高度 - 右侧.高度;
                    if (Height > 0)
                    {
                        分割(右侧.区域.Height, 方向.水平);
                    }
                }
            }

            if (下侧 != null)
            {
                if (!FormSheet.Compare(宽度, 下侧.宽度))
                {
                    int Width = 宽度 - 下侧.宽度;
                    if (Width > 0)
                    {
                        分割(下侧.区域.Width, 方向.垂直);
                    }
                }
            }
        }
        /// <summary>
        /// 分割目标
        /// </summary>
        /// <param name="Height"></param>
        /// <param name="Width"></param>
        private void 通知分割(int Height, int Width, 方向 方向)
        {
            switch (方向)
            {
                case 方向.水平:
                    if (区域.Height > Height)
                    {
                        分割(Height, 方向);
                    }
                    break;
                case 方向.垂直:
                    if (区域.Width > Width)
                    {
                        分割(Width, 方向);
                    }
                    break;
            }
        }
        private FormCell 分割(int length, 方向 方向)
        {
            Rectangle newrec = 分割区域(ref 区域, length, 方向);
            FormCell newcell = null;
            switch (方向)
            {
                case 方向.水平:
                    newcell = new FormCell(newrec, 置信度, 行 + 1, 列, 方向, 所属);
                    if (下侧 != null)
                    {
                        newcell.下侧 = 下侧;
                    }
                    下侧 = newcell;
                    break;
                case 方向.垂直:
                    newcell = new FormCell(newrec, 置信度, 行, 列 + 1, 方向, 所属);
                    if (右侧 != null)
                    {
                        newcell.右侧 = 右侧;
                    }
                    右侧 = newcell;
                    break;
            }
            return newcell;
        }

        public int 高度 { get => 区域.Height; }

        public int 宽度 { get => 区域.Width; }

        /// <summary>
        /// 分割之后返回割出来的区域，保留值为被分割的预期长或宽
        /// </summary>
        /// <param name="初值"></param>
        /// <param name="保留值"></param>
        /// <param name="方向"></param>
        /// <returns></returns>
        private static Rectangle 分割区域(ref Rectangle 初值, int 保留值, 方向 方向)
        {
            Rectangle rec = 初值;
            Rectangle targ = new Rectangle();
            switch (方向)
            {
                case 方向.水平:
                    初值 = new Rectangle(rec.Location, new Size(rec.Width, 保留值));
                    targ = new Rectangle(rec.X, rec.Y + 保留值, rec.Width, rec.Height - 保留值);
                    break;
                case 方向.垂直:
                    初值 = new Rectangle(rec.Location, new Size(保留值, rec.Height));
                    targ = new Rectangle(rec.X + 保留值, rec.Y, rec.Width - 保留值, rec.Height);
                    break;
            }
            return targ;
        }

        public bool 填入矩阵(FormCell[,] Matrix, Point 坐标)
        {
            if (坐标.X < Matrix.GetLongLength(0) && 坐标.Y < Matrix.GetLongLength(1))
            {
                Matrix[坐标.X, 坐标.Y] = this;
            }
            else
            {
                return false;
            }
            if (右侧 != null)
            {
                右侧.填入矩阵(Matrix, new Point(坐标.X, 坐标.Y + 1));
            }
            if (下侧 != null)
            {
                下侧.填入矩阵(Matrix, new Point(坐标.X + 1, 坐标.Y));
            }
            return true;
        }


        public static bool 判断空或增生单元(string src)
        {
            if (src == "" || src == 下增生内容 || src == 右增生内容)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
