using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCloud.Apis
{
    public class DocumentController : Controller
    {
        public IActionResult Swagger()
        {
            return Redirect("/swagger/index.html");
        } 
    }
}
