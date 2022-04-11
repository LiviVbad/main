using Abp.Application.Services;
using AppFrameworkDemo.Configuration.Tenants.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}