using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class PictureSearch :SearchBase
    {
        public PictureSearch(Stream img,ImageSource source) :base(source)
        {
            _img = img;
            Ext = Path.GetExtension(source.文件名);
        }

        private readonly string Ext;

        public readonly Stream _img;

        public string JSON = null;

        public string ERROR;

        private string OutPut;
        private string OutFile;
        private FormSheet Sheet { get; set; }
        public Task<string> 获取结果(DocxUnit unit)
        {
            if (JSON != null && Sheet == null)
            {
                try
                {
                    切换状态(工作状态.处理中);
                    string filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".docx";
                    OutFile = Path.Combine(源文件.所有者.用户wwwroot全路径(), filename);
                    OutPut = Path.Combine(源文件.所有者.用户Src路径(), filename);
                    unit.创建Doc();
                    Sheet = 生成表格(unit, JSON);
                    unit.保存Doc(OutFile);
                    切换状态(工作状态.完成);
                }
                catch (Exception e)
                {
                    ERROR = e.ToString();
                    OutPut = null;
                    OutFile=null;
                    切换状态(工作状态.失败);
                }
                finally
                {
                    unit.关闭Doc();
                }
            }
            return Task.FromResult(OutPut);
        }
        public string 获取结果()
        {
            return OutPut;
        }

        public override bool 销毁()
        {
            if(状态 != 工作状态.处理中)
            {
                Searches.Remove(this);
                _img.Dispose();
                if(OutFile!=null && File.Exists(OutFile))
                {
                    File.Delete(OutFile);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
