using AppFramework.ApiClient; 
using AppFramework.Update.Dto;
using Flurl.Http.Content;
using System; 
using System.Threading.Tasks;

namespace AppFramework.Update
{
    public class AppUpdateService : ProxyAppServiceBase, IAppUpdateService
    {
        public AppUpdateService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<UpdateFileOutput> CheckVersion(CheckVersionInput input)
        {
            return await ApiClient.GetAsync<UpdateFileOutput>(GetEndpoint(nameof(CheckVersion)), input);
        } 
    }
}
