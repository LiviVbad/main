using AppFramework.ApiClient;
using AppFramework.DynamicEntityProperties;
using System.Collections.Generic;

namespace AppFramework.Application
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