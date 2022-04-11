using Abp.Application.Services.Dto;
using AppFrameworkDemo.ApiClient;
using AppFrameworkDemo.Caching;
using AppFrameworkDemo.Caching.Dto;
using System;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Application.Client
{
    public class CachingAppService : ProxyAppServiceBase, ICachingAppService
    {
        public CachingAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public Task ClearAllCaches()
        {
            throw new NotImplementedException();
        }

        public Task ClearCache(EntityDto<string> input)
        {
            throw new NotImplementedException();
        }

        public ListResultDto<CacheDto> GetAllCaches()
        {
            throw new NotImplementedException();
        }
    }
}