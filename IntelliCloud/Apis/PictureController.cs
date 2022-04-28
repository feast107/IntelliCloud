using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ModelLib;
using Repositories.API;
using Repositories;
using System.Drawing;
using System.IO;
using System;
using IntelliHttpClient;
using IntelliCloud.Controllers;

namespace IntelliCloud.Apis
{
    [Route(nameof(Routes.api) + "/" + nameof(Routes.Picture))]
    [ApiController]
    [Authorize(Roles = nameof(角色.用户))]
    public class PictureController : ServiceController
    {
        public PictureController(IService service):base(service)
        {
        }
        
        private string UserName { get => User.Identity.Name; }

        [RequestSizeLimit(3145728)]
        [Route(nameof(Picture.Submit))]
        [HttpPost]
        public IActionResult 提交图片()
        {
            return Content(Path.Combine(Environment.CurrentDirectory,GetShop().所有者.用户wwwroot路径(),"file.docx"));
        }

        [RequestSizeLimit(3145728)]
        [Route(nameof(Picture.Upload))]
        [HttpPost]
        public IActionResult 提交图片2(IFormFile picture)
        {
            Stream stream = new MemoryStream();
            picture.CopyTo(stream);

            int num = GetShop().提交(new PictureSearch(stream, new ImageSource(GetShop().所有者, 文件类型.图片)));
            if (num != -1)
            {
                var t = GetShop().获取识别任务().Find(x => x.编号 == num);
                if (t.状态 == 工作状态.完成)
                {
                    return Content(t.获取结果());
                }
                else if(t.状态 == 工作状态.创建)
                {
                    var task = GetShop().识别(t.num);
                    task.Wait();
                    return new JsonResult(num);
                }
                else if (t.状态 == 工作状态.失败)
                {
                    return Content("您的工作失败，由于:" + t.ERROR);
                }
                else
                {
                    return Content("您的工作正在执行中，请稍后");
                }

            }
            else
            {
                return Content("您没有该编号的工作");
            }
        }

        [RequestSizeLimit(3145728)]
        [Route(nameof(Picture.Ask))]
        [HttpPost]
        public IActionResult 询问(int num)
        {
            var t = GetShop().获取识别任务().Find(x => x.编号 == num);
            if (t.状态 == 工作状态.完成)
            {
                return Content(t.获取结果());
            }
            else if (t.状态 == 工作状态.创建)
            {
                var task = GetShop().识别(t.num);
                task.Wait();
                return new JsonResult(task);
            }
            else if (t.状态 == 工作状态.失败)
            {
                return Content("您的工作失败，由于:" + t.ERROR);
            }
            else
            {
                return Content("您的工作正在执行中，请稍后");
            }
        }

        [RequestSizeLimit(3145728)]
        [Route(nameof(Picture.Wait))]
        [HttpPost]
        public IActionResult 等待(int num)
        {
            var t = GetShop().获取识别任务().Find(x => x.编号 == num);
            while(t.状态 == 工作状态.处理中)
            {

            }
            if (t.状态 == 工作状态.完成)
            {
                return Content(t.获取结果());
            }
            else if (t.状态 == 工作状态.失败)
            {
                return Content("您的工作失败，由于:" + t.ERROR);
            }
            return Content("");
        }

        private static Bitmap Base64ToBitmap(string base64)
        {
            byte[] bytes;
            try
            {
                bytes = Base64UrlTextEncoder.Decode(base64);
            }
            catch
            {
                base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
                bytes = Convert.FromBase64String(base64);
            }
            MemoryStream memStream = new MemoryStream(bytes);
            Image mImage = Image.FromStream(memStream);
            return new Bitmap(mImage);
        }

        private static byte[] Base64ToByte(string base64)
        {
            byte[] bytes;
            try
            {
                bytes = Base64UrlTextEncoder.Decode(base64);
            }
            catch
            {
                base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
                bytes = Convert.FromBase64String(base64);
            }
            return bytes;
        }
    }
}
