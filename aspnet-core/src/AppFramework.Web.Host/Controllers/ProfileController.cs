using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Storage;
using System.Threading.Tasks;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Dto;
using System.Linq;
using Abp.UI;
using System.IO;
using Abp.Web.Models;
using Microsoft.AspNetCore.Hosting;
using AppFramework.Update;

namespace AppFramework.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService) :
            base(tempFileCacheManager, profileAppService)
        {
        }
    }
}