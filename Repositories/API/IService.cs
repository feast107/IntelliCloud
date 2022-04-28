using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.API
{
    public interface IService
    {
        WorkShop GetWorkShop(User user);

        WorkShop GetWorkShop(int id);

        User Regist(string Name, string Password);

        User Login(string Name, string Password);


    }
}
