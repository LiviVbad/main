using Abp.AspNetCore.Mvc.Authorization;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Storage;
using AppFramework.Version;
using Microsoft.AspNetCore.Hosting;

namespace AppFramework.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IAbpVersionsAppService versionsAppService,
            IWebHostEnvironment environment) :
            base(versionsAppService, environment, tempFileCacheManager, profileAppService)
        {
        }
    }
}