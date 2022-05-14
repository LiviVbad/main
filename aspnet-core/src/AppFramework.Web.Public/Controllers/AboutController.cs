using Microsoft.AspNetCore.Mvc;
using AppFramework.Web.Controllers;

namespace AppFramework.Web.Public.Controllers
{
    public class AboutController : AppFrameworkDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}