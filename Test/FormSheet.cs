using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using ModelLib;

namespace Test
{
    public class FormSheet
    {
        enum 优先级
        {
            下,
            右
        }
        public class Information
        {
            public int MinCol = int.MaxValue;
            public int MaxCol = 0;
            public int MinRow = int.MaxValue;
            public int MaxRow = 0;
        }

        public FormCell[,] Matrix;

        public const int 容差 = 8;

        private FormSheet() { }

        public List<FormCell> 确信体 = new List<FormCell>();

        public List<FormCell> 不确信体 = new List<FormCell>();

        public List<FormCell> 头部 =new List<FormCell>();

        public List<FormCell> 尾部=new List<FormCell>();


        public Rectangle Location;

        public Information information=new Information();



        /// <summary>
        /// 读取结果
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static FormSheet GetFormResult(string Content)
        {
            FormSheet sheet = new FormSheet();
            while (Content[0] != '}')
            {
                switch (Content[0])
                {
                    case '"':
                        Content = Content[1..];
                        string Attr = GetAttributeName(ref Content);
                        switch (Attr)
                        {
                            case "vertexes_location":
                                sheet.Location=GetArea(ref Content);
                                continue;
                            case "body":
                                List<FormCell>[] formsCells = sheet.GetBodyCells(ref Content,sheet.information);
                                sheet.确信体.AddRange(formsCells[0]);
                                sheet.不确信体.AddRange(formsCells[1]);
                                continue;
                            case "footer":
                                sheet.尾部.AddRange(sheet.GetCells(ref Content, sheet.information));
                                continue;
                            case "header":
                                sheet.头部.AddRange(sheet.GetCells(ref Content,sheet.information));
                                continue;
                        }
                        continue;
                }
                Content = Content[1..];
            }
            return sheet;
        }
      
        /// <summary>
        /// 获取体单元格
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public List<FormCell>[] GetBodyCells(ref string Content,Information information)
        {
            List<FormCell>[] result = new List<FormCell>[2];
            result[0] = new List<FormCell>();
            result[1] = new List<FormCell>();
            while (Content[0] != ']')
            {
                switch (Content[0])
                {
                    case '{':
                        FormCell c = MergeACell(ref Content,information);
                        if (c.置信度 == 1f)
                        {
                            result[0].Add(c);
                        }
                        else
                        {
                            result[1].Add(c);
                        }
                        continue;
                }
                Content = Content[1..];
            }
            Content = Content[1..];
            return result;
        }

        /// <summary>
        /// 解析全部单元格
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public List<FormCell> GetCells(ref string Content, Information information)
        {
            List<FormCell> result = new List<FormCell>();
            while (Content[0] != ']')
            {
                switch (Content[0])
                {
                    case '{':
                        FormCell c = MergeACell(ref Content, information);
                        result.Add(c);
                        continue;
                }
                Content = Content[1..];
            }
            Content = Content[1..];
            return result;
        }
       
        #region 解析区
        /// <summary>
        /// 解析一个单元格
        /// </summary>
        /// <param name="cell"></param>
        private void AddCell(FormCell cell)
        {
            if (cell.置信度 == 1f)
            {
                确信体.Add(cell);
            }
            else
            {
                不确信体.Add(cell);
            }
        }
        
        /// <summary>
        /// 解析一组单元格
        /// </summary>
        /// <param name="cells"></param>
        private void AddCells(List<FormCell> cells)
        {
            foreach(var c in cells)
            {
                AddCell(c);
            }
        }
        private FormCell MergeACell(ref string Content,Information information)
        {
            FormCell cell = new FormCell(this);
            while(Content[0]!='}')
            {
                switch (Content[0])
                {
                    case '"':
                        Content = Content[1..];
                        SetAttribute(cell,ref Content);
                         continue;
                }
                Content = Content[1..];
            }
            Content=Content[1..];
            SetInformation(cell, information);
            return cell;
        }
        private static void SetInformation(FormCell cell,Information information)
        {
            if (cell.置信度 == 1f)
            {
                if (cell.行 < information.MinRow)
                {
                    information.MinRow = cell.行;
                }
                if (cell.行 > information.MaxRow)
                {
                    information.MaxRow = cell.行;
                }
                if (cell.列 < information.MinCol)
                {
                    information.MinCol = cell.列;
                }
                if (cell.列 > information.MaxCol)
                {
                    information.MaxCol = cell.列;
                }
            }
        }
        private static void SetAttribute(FormCell cell,ref string Content)
        {
            string Attribute=GetAttributeName(ref Content);
            switch (Attribute)
            {
                case "vertexes_location":
                    Rectangle rec=GetArea(ref Content);
                    cell.区域 = rec;
                    cell.中心 = new Point(rec.X+rec.Width/2, rec.Y+rec.Height/2);
                    break;
                case "probability":
                    cell.置信度 = GetNumber(ref Content);
                    break;
                case "column":
                    cell.列 = GetNumber(ref Content);
                    break;
                case "words":
                    cell.内容= GetContent(ref Content);
                    break;
                case "row":
                    cell.行 = GetNumber(ref Content);
                    break;
            }

        }
        private static Rectangle GetArea(ref string Content)
        {
            List<int> xs=new List<int>();
            List<int> ys=new List<int>();
            while (Content[0] != ']')
            {
                switch (Content[0])
                {
                    case '{':
                        int[] res=GetLocation(ref Content);
                        xs.Add(res[0]);
                        ys.Add(res[1]);
                        continue;
                }
                Content=Content[1..];
            }
            Content = Content[1..];
            return new Rectangle(xs[0], ys[0], (xs[1] + xs[2] - xs[0] - xs[3]) / 2, (ys[3] + ys[2] - ys[0] - ys[1]) / 2);
        }
        private static int[] GetLocation(ref string Content)
        {
            int[] ret=new int[2];
            while(Content[0] != '}')
            {
                switch (Content[0])
                {
                    case 'x':
                        ret[0] = GetNumber(ref Content);
                        continue;
                    case 'y':
                        ret[1] = GetNumber(ref Content);
                        continue;
                }
                Content=Content[1..];
            }
            Content = Content[1..];
            return ret;
        }
        private static string GetContent(ref string Content)
        {
            string ret= string.Empty;
            while (Content[0] != '"')
            {
                Content=Content[1..];
            }
            Content= Content[1..];
            while (Content[0] != '"' &&!(Content[1]=='\n' || (Content[1] == ',' && Content[2] == '\n')))
            {
                ret += Content[0];
                Content=Content[1..];
            }
            Content = Content[1..];
            return ret;
        }
        private static int GetNumber(ref string Content)
        {
            string ret = string.Empty;
            while (Content[0] != ','&& Content[0] != '\n')
            {
                if (char.IsNumber(Content[0]))
                {
                    ret+=Content[0];
                }
                Content=Content[1..];
            }
            return Convert.ToInt32(ret);
        }
        private static string GetAttributeName(ref string Content)
        {
            string ret=string.Empty;
            while (Content[0] != '"')
            {
                ret+=Content[0];
                Content = Content[1..];
            }
            Content=Content[1..];
            return ret;
        }
        #endregion

        static private bool 挂载(FormCell src,FormCell targ,优先级 prio)
        {
            switch (prio)
            {
                case 优先级.下:
                    return src.下优先挂载(targ);
                case 优先级.右:
                    return src.右优先挂载(targ);
                default:
                    return false;
            }
        }

        public FormCell[,] GenerateMatrix()
        {
            Matrix=new FormCell[information.MaxRow - information.MinRow + 1,information.MaxCol - information.MinCol + 1];
            List<FormCell> temp = new List<FormCell>(确信体);
            整合(Matrix, temp, 优先级.右);
            var head = 整合(Matrix, 确信体, 优先级.下);
            head.填入矩阵(Matrix, new Point(0,0));
            return Matrix;
        }

        private static void 矩阵整理(FormCell[,] Matrix)
        {
        }

        private FormCell 整合(FormCell[,] Matrix,List<FormCell> Source,优先级 prio)
        {
            List<FormCell> Heads=new List<FormCell>();
            while (Source.Count != 0)
            {
                for(int i = 0; i < Source.Count; i++)
                {
                    FormCell cell = Source[i];
                    bool 尝试挂载 = false;
                    foreach(var c in Heads)
                    {
                        if (FormSheet.挂载(c,cell,prio))
                        {
                            尝试挂载 = true;
                            break;
                        }
                    }
                    if (!尝试挂载)
                    {
                        Heads.Add(cell);
                    }
                    Source.RemoveAt(i);
                    i--;
                }

                for(int i = 0; i < Heads.Count; i++)
                {
                    bool 挂载 = false;
                    for(int r = 0; r < Heads.Count; r++)
                    {
                        if (r != i)
                        {
                            if (FormSheet.挂载(Heads[r], Heads[i], prio))
                            {
                                挂载 = true;
                                Heads.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    if (挂载)
                    {
                        i--;
                    }
                }
            }
            FormCell head = Heads[0];
            return head;
        }

        public static bool Compare(int int1,int int2)
        {
            if(int1-int2<= 容差 && int1-int2>= -容差)
            {
                return true;
            }
            return false;
        }

        public static bool Contain(int Large,int Small)
        {
            if (Large >= Small)
            {
                return true;
            }
            else
            {
                if(Small-Large <= 容差)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回一行中可用的值
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="posit">行\列</param>
        /// <param name="rc">True行 False列</param>
        /// <returns></returns>
        public static FormCell 查找非空(FormCell[,] cells,int posit,bool rc)
        {
            if (rc)
            {
                int cent = (int)cells.GetLongLength(1) / 2;
                for (int c = 0; c < cent; c++)
                {
                    if (cells[posit,cent + c] != null)
                    {
                        return cells[posit, cent + c];
                    }
                    if (cells[posit, cent - c] != null)
                    {
                        return cells[posit, cent + c];
                    }
                }
            }
            else
            {
                int cent = (int)cells.GetLongLength(0) / 2;
                for (int r = 0; r < cent; r++)
                {
                    if (cells[cent + r, posit] != null)
                    {
                        return cells[cent + r, posit];
                    }
                    if (cells[cent - r, posit] != null)
                    {
                        return cells[cent + r, posit];
                    }
                }
            }
            return null;
        }
    
    
    }
}
