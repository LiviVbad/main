using Microsoft.AspNetCore.Mvc;
using AppFramework.Web.Controllers;

namespace AppFramework.Web.Public.Controllers
{
    public class AboutController : AppFrameworkControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}