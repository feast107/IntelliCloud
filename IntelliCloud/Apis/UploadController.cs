using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ModelLib;
using IntelliHttpClient;
using IntelliCloud.Controllers;
using Repositories.API;
using System.Collections.Generic;
using System.Linq;
using IntelliHttpClient.ViewModels;
using Newtonsoft.Json;
namespace IntelliCloud.Apis
{
    [Route(nameof(Routes.api)+"/"+nameof(Routes.File))]
    [ApiController]
    [Authorize(Roles = nameof(角色.用户))]
    public class UploadController : ServiceController
    {
        public UploadController(IService service) : base(service)
        {
        }

        [Route(nameof(Files.Excel))]
        [HttpPost]
        public IActionResult 提交Excel(IFormFile file,[FromForm] IntelliHttpClient.ViewModels.ExcelRule rule)
        {
            List<SearchBase> bases = new List<SearchBase>();
            var r = Converter.Parse(rule);
            bases.Add(new ExcelSearch(r,new SourceFile(GetShop().所有者, file)));
            var ret = GetShop().提交(bases);
            return Content(ret[0].ToString());
        }
   
        [Route(nameof(Files.Example))]
        [HttpPost]
        public IActionResult 样例()
        {
            WordRuleCollection rule = new WordRuleCollection();
            rule.WordRules = new Dictionary<string, List<IntelliHttpClient.ViewModels.WordRule>>() 
            {
                { "???", new List<IntelliHttpClient.ViewModels.WordRule>(){ new IntelliHttpClient.ViewModels.WordRule() { Length=1,Mode= IntelliHttpClient.ViewModels.WordRule.匹配模式.任意字符 } } }
            }
            ;
            return Content(JsonConvert.SerializeObject(rule));
        }

        [Route(nameof(Files.Word))]
        [HttpPost]
        public IActionResult 提交Word(IFormFile file, [FromForm] string JSON)
        {
            var r = JsonConvert.DeserializeObject<WordRuleCollection>(JSON);
            System.Collections.Generic.List<SearchBase> bases = new System.Collections.Generic.List<SearchBase>();
            var rules = Converter.Parse(r);
            bases.Add(new WordSearch(rules, new SourceFile(GetShop().所有者, file)));
            var ret = GetShop().提交(bases);
            return Content(ret[0].ToString());
        }
       
        [Route(nameof(Files.Run))]
        [HttpPost]  
        public IActionResult 开启([FromBody] int[] ints)
        {
            GetShop().执行(ints.ToList());
            return Content("");
        }

        [Route(nameof(Files.Pause))]
        [HttpPost]
        public IActionResult 中断([FromBody] int[] ints)
        {
            GetShop().中断(ints);
            return Content("");
        }

        [Route(nameof(Files.ExcelTasks))]
        [HttpPost]
        public IActionResult Excel任务()
        {
            return new JsonResult(GetShop().获取Excel任务());
        }
        [Route(nameof(Files.WordTasks))]
        [HttpPost]
        public IActionResult Word任务()
        {
            return new JsonResult(GetShop().获取Word任务());
        }

        [Route(nameof(Files.WordControl))]
        [HttpPost]
        public IActionResult Word列表()
        {
            return new JsonResult(GetShop().Word执行列表);
        }

        [Route(nameof(Files.ExcelControl))]
        [HttpPost]
        public IActionResult Excel列表()
        {
            return new JsonResult(GetShop().Excel执行列表);
        }
    }
}
