using Microsoft.AspNetCore.Mvc;
using AppFrameworkDemo.Web.Controllers;

namespace AppFrameworkDemo.Web.Public.Controllers
{
    public class HomeController : AppFrameworkDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}