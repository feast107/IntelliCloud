using IntelliCloud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
namespace IntelliCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IService _service;

        public HomeController(ILogger<HomeController> logger, IService service)
        {
            
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private WorkShop GetWorkShop()
        {
            string Name = HttpContext.Session.GetString("Name");
            string Psw = HttpContext.Session.GetString("Password");
            return _service.GetWorkShop(_service.Login(Name, Psw));
        }

        public void Stop()
        {
            GetWorkShop().终止();
        }

        public IActionResult GetPage()
        {
            string base64 = Request.Form["BASE64"];
            return Content("RESULT");
        }


    }

}