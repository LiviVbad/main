using AppFrameworkDemo.ApiClient;
using AppFrameworkDemo.DynamicEntityProperties;
using System.Collections.Generic;

namespace AppFrameworkDemo.Application
{
    public class DynamicEntityPropertyDefinitionAppService : ProxyAppServiceBase, IDynamicEntityPropertyDefinitionAppService
    {
        public DynamicEntityPropertyDefinitionAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public List<string> GetAllAllowedInputTypeNames()
        {
            return ApiClient.GetAsync<List<string>>(GetEndpoint(nameof(GetAllAllowedInputTypeNames))).Result;
        }

        public List<string> GetAllEntities()
        {
            return ApiClient.GetAsync<List<string>>(GetEndpoint(nameof(GetAllEntities))).Result;
        }
    }
}