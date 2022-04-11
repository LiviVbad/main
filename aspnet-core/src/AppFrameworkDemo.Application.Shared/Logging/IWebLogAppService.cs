using Abp.Application.Services;
using AppFrameworkDemo.Dto;
using AppFrameworkDemo.Logging.Dto;

namespace AppFrameworkDemo.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}