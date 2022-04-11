using System.Threading.Tasks;
using Abp.Application.Services;
using AppFrameworkDemo.Install.Dto;

namespace AppFrameworkDemo.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}