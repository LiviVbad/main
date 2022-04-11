using Microsoft.AspNetCore.Mvc;
using AppFrameworkDemo.Web.Controllers;

namespace AppFrameworkDemo.Web.Public.Controllers
{
    public class AboutController : AppFrameworkDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}