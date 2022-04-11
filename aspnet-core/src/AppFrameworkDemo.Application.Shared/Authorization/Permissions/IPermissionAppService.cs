using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.Authorization.Permissions.Dto;

namespace AppFrameworkDemo.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}