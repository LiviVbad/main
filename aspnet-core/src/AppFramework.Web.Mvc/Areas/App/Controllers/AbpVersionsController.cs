using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppFramework.Web.Areas.App.Models.AbpVersions;
using AppFramework.Web.Controllers;
using AppFramework.Authorization;
using AppFramework.Update;
using AppFramework.Update.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace AppFramework.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AbpVersions)]
    public class AbpVersionsController : AppFrameworkControllerBase
    {
        private readonly IAbpVersionsAppService _abpVersionsAppService;

        public AbpVersionsController(IAbpVersionsAppService abpVersionsAppService)
        {
            _abpVersionsAppService = abpVersionsAppService;

        }

        public ActionResult Index()
        {
            var model = new AbpVersionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AbpVersions_Create, AppPermissions.Pages_AbpVersions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAbpVersionForEditOutput getAbpVersionForEditOutput;

            if (id.HasValue)
            {
                getAbpVersionForEditOutput = await _abpVersionsAppService.GetAbpVersionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAbpVersionForEditOutput = new GetAbpVersionForEditOutput
                {
                    AbpVersion = new CreateOrEditAbpVersionDto()
                };
            }

            var viewModel = new CreateOrEditAbpVersionModalViewModel()
            {
                AbpVersion = getAbpVersionForEditOutput.AbpVersion,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

    }
}