using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace ModelLib
{
    public class SourceFile
    {
        #region 静态区
        private static readonly string 根路径 = Configuration.wwwroot目录;
        private static readonly string Src路径 = Configuration.Src目录;
        #endregion

        #region 实例区
        private readonly int Count;
        private readonly string FileName;
        private readonly 文件类型 type;
        private readonly string FilePath;
        private readonly DateTime UpDateTime;
        private readonly string Extension;
        public readonly User 所有者;
        #endregion

        protected SourceFile(User 所有者,文件类型 type)
        {
            this.所有者 = 所有者;
            this.type = type;
            所有者.添加文件(this);
            UpDateTime = DateTime.Now;
        }

        public SourceFile(User 所有者,IFormFile file)
        {
            this.所有者 = 所有者;
            string Ext = Path.GetExtension(file.FileName);
            if (Configuration.获取类型(Ext).Equals(文件类型.无适配))
            {
                return;
            }
            type = Configuration.获取类型(Ext);
            Extension = Ext;
            FileName = file.FileName;
            Count = 所有者.获取文件数();
            FilePath = 保存文件(file);
            所有者.添加文件(this);
            UpDateTime = DateTime.Now;
        }


        public DateTime 上传时间=>UpDateTime;
        public string 文件名 => FileName;
        public 文件类型 类型 => type;

        public int 编号 => Count;

        private string 保存文件(IFormFile file)
        {
            string path = Path.Combine(所有者.用户wwwroot路径(), Count + Path.GetExtension(file.FileName));
            var stream = new FileStream(Path.GetFullPath(path), FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            return path;
        }
        private string 保存文件(FileInfo file)
        {
            string path = Path.Combine(所有者.用户wwwroot路径(), Count + Path.GetExtension(file.Name));
            File.Move(file.FullName, Path.GetFullPath(path));
            return path;
        }

        public string 获取物理路径()
        {
            return Path.IsPathRooted(FilePath) ? FilePath : Path.GetFullPath(FilePath);
        }

        public string 获取Src路径()
        {
            return Path.Combine(Src路径, FilePath);
        }

        public bool 删除文件()
        {
            string Path = 获取物理路径();
            if (File.Exists(Path))
            {
                File.Delete(Path);
                return true;
            }
            return false;
        }
    }
}
