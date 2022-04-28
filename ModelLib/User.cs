using ModelLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class User
    {
        #region 静态区
        private static readonly string 根路径 = Configuration.wwwroot目录;
        private static readonly string Src路径 = Configuration.Src目录;
        #endregion

        private static readonly Dictionary<int, User> Users=new Dictionary<int, User>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns>null为失败成功为注册对象</returns>
        public static User 注册(string UserName,string Password)
        {
            foreach(KeyValuePair<int ,User> keyValuePair in Users)
            {
                if (keyValuePair.Value.UserName.Equals(UserName))
                {
                    return null;
                }
            }
            lock (Users)
            {
                int Count = Users.Count;
                User user = new User(Count, UserName, Password);
                Users.Add(Count, user);
                return user;
            }
            
        }
        
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns>成功返回对象失败返回空</returns>
        public static User 登录(string UserName,string Password)
        {
            foreach (KeyValuePair<int, User> keyValuePair in Users)
            {
                if (keyValuePair.Value.UserName.Equals(UserName))
                {
                    if (keyValuePair.Value.密码校对(Password))
                    {
                        return keyValuePair.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }


        private User(int id,string password, string userName)
        {
            ID = id;
            Password = password;
            UserName = userName;
            DirectoryInfo directory = new DirectoryInfo(用户wwwroot路径());
            if(directory.Exists)
            {
                directory.GetFiles().ToList().ForEach(x=>x.Delete());
                directory.Delete();
            }
            new  DirectoryInfo(用户wwwroot全路径()).Create();
        }
     
        private readonly int ID;

        private readonly string Password;

        private readonly string UserName;

        private readonly int PhoneNumber;
        
        private readonly List<SourceFile> 拥有文件 =new  List<SourceFile>();


        public string 用户名 => UserName;

        public int 编号 => ID;
        public bool 添加文件(SourceFile sourceFile)
        {
            lock (sourceFile)
            {
                if (拥有文件.Contains(sourceFile))
                {
                    return false;
                }
                拥有文件.Add(sourceFile);
            }
            return true;
        }

        public int 获取文件数()
        {
            return 拥有文件.Count;
        }
        public string 用户wwwroot路径()
        {
            return Path.Combine(Configuration.wwwroot目录,Configuration.Src目录, ID.ToString());
        }
        public string 用户wwwroot全路径()
        {
            return Path.Combine(Environment.CurrentDirectory, 用户wwwroot路径());
        }

        public string 用户Src路径()
        {
            return Path.Combine(Configuration.Src目录, ID.ToString());
        }

        public bool 密码校对(string Psw)
        {
            if (Psw.Equals(Password))
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
