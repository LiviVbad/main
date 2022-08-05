using Microsoft.AspNetCore.Mvc;
using AppFramework.Web.Controllers;

namespace AppFramework.Web.Public.Controllers
{
    public class HomeController : AppFrameworkControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}