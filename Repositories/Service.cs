using ModelLib;
using Repositories.API;
using System;
using System.Collections.Generic;
using System.Threading;
using Baidu.Aip;

namespace Repositories
{
    public class Service :IService
    {
        private readonly IFactory _factory;
        private readonly IAnalyzer _analyzer;

        private static readonly Dictionary<User,WorkShop> workShops = new  Dictionary<User, WorkShop>();


        public Service(IFactory factory)
        {
            _factory = factory;
        }
        private static string APP_ID = "你的 App ID";
        private static string API_KEY = "你的 Api Key";
        private static string SECRET_KEY = "你的 Secret Key";

        readonly Baidu.Aip.Ocr.Ocr client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);


        public WorkShop GetWorkShop(User user)
        {
            if (workShops.TryGetValue(user, out WorkShop shop))
            {
                return shop;
            }
            else
            {
                lock (workShops)
                {
                    WorkShop workShop = new WorkShop(_factory,_analyzer, user);
                    workShops.Add(user,workShop);
                    return workShop;
                }
            }
        }

        public User Login(string Name, string Password)
        {
           return User.登录(Name,Password);
        }

        public User Regist(string Name, string Password)
        {
            return User.注册(Name,Password);
        }

        public WorkShop GetWorkShop(int id)
        {
            throw new NotImplementedException();
        }
    }
}
