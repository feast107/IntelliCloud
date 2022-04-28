using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.API;
using Repositories;
using System.Linq;
using System;
using ModelLib;

namespace IntelliCloud.Controllers
{
    public class ServiceController : ControllerBase
    {
        protected readonly IService _service;
        protected ServiceController(IService service)
        {
            _service = service;
        }
        protected int GetId()
        {
            return Convert.ToInt32(User.Claims.First((x) => x.Type.Equals(Configuration.ToClaim(令牌.Sid)))?.Value);
        }
        protected WorkShop GetShop()
        {
            return _service.GetWorkShop(GetId());
        }
    }
}
