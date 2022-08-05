using AppFramework.Update.Dtos;

using Abp.Extensions;

namespace AppFramework.Web.Areas.App.Models.AbpVersions
{
    public class CreateOrEditAbpVersionModalViewModel
    {
        public CreateOrEditAbpVersionDto AbpVersion { get; set; }

        public bool IsEditMode => AbpVersion.Id.HasValue;
    }
}