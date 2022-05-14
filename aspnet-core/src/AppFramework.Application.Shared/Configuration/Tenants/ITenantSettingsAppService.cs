using Abp.Application.Services;
using AppFramework.Configuration.Tenants.Dto;
using System.Threading.Tasks;

namespace AppFramework.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}