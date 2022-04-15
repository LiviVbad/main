using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.Authorization.Permissions.Dto;

namespace AppFramework.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}