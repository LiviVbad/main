using Abp.Application.Services;
using AppFrameworkDemo.Configuration.Host.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}