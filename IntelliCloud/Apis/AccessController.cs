using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using ModelLib;
using Repositories.API;
using IntelliHttpClient;
using IntelliCloud.Controllers;

namespace IntelliCloud.Apis
{
    [Route(nameof(Routes.api) +"/"+nameof(Routes.Access))]
    [ApiController]
    [AllowAnonymous]
    public class AccessController : ServiceController
    {
        public AccessController(IService service):base(service)
        {
        }

        [Route(nameof(Access.Regist))]
        [HttpGet]
        public IActionResult 注册(string UserName, string Password)
        {
            if (!是否登录())
            {
                User user = _service.Regist(UserName, Password);
                
                if (user != null)
                {
                    _service.GetWorkShop(user);
                    Authenticate(user.用户名,user.编号);
                    return Content("您:" + UserName + "已经成功注册");
                }
            }
            return Content("您已经注册过了");
        }

        [Route(nameof(Access.Login))]
        [HttpGet]
        public IActionResult 登录(string UserName, string Password)
        {
            if (!是否登录())
            {
                User user = _service.Login(UserName, Password);
                
                if (user != null)
                {
                    _service.GetWorkShop(user);
                    Authenticate(user.用户名, user.编号);
                    return Content("您:" + UserName + "已经成功登录");
                }
                else
                {
                    return Content("您的账号并不存在");
                }
            }
            return Content("您已经在登录状态");
        }

        [Route(nameof(Access.Logout))]
        [HttpGet]
        public IActionResult 退出()
        {
            if(是否登录())
                HttpContext.SignOutAsync().Wait();
            return Content("已退出登录");
        }

        [Route(nameof(Access.Current))]
        [HttpGet]
        public IActionResult 检查()
        {
            if (是否登录())
{
                return Content("您已经登录");
            }
            else
            {
                return Content("您是匿名用户");
            }

        }

        [Route(nameof(Access.User))]
        [HttpGet]
        public IActionResult 用户()
        {
            return new JsonResult(GetShop().所有者);
        }

        /// <summary>
        /// 通过验证
        /// </summary>
        /// <param name="Name"></param>
        private void Authenticate(string Name,int Id)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, nameof(角色.用户)),
                new Claim(ClaimTypes.Name,Name),
                new Claim(ClaimTypes.Sid,Id.ToString())
            };
            var Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(角色.用户)));
            Task t = Task.Run(() =>
            {
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(3),
                        IsPersistent = false,
                        AllowRefresh = true
                    });
            });
            t.Wait();
        }

        /// <summary>
        /// 是否验证
        /// </summary>
        /// <returns></returns>
        private bool 是否登录()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

    }



}
