using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.Caching.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);

        Task ClearAllCaches();
    }
}