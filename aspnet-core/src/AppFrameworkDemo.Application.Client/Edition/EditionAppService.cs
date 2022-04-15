using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.Application
{
    public class EditionAppService : ProxyAppServiceBase, IEditionAppService
    {
        public EditionAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<List<SubscribableEditionComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null, bool addAllItem = false, bool onlyFree = false)
        {
            return await ApiClient.GetAsync<List<SubscribableEditionComboboxItemDto>>(GetEndpoint(nameof(GetEditionComboboxItems)), new
            {
                selectedEditionId,
                addAllItem,
                onlyFree
            });
        }

        public async Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input)
        {
            return await ApiClient.GetAsync<GetEditionEditOutput>(GetEndpoint(nameof(GetEditionForEdit)), input);
        }

        public async Task<int> GetTenantCount(int editionId)
        {
            return await ApiClient.GetAsync<int>(GetEndpoint(nameof(GetTenantCount)), editionId);
        }

        public async Task MoveTenantsToAnotherEdition(MoveTenantsToAnotherEditionDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(MoveTenantsToAnotherEdition)), input);
        }

        public async Task CreateOrUpdateAsync(CreateEditionDto input)
        {
            await ApiClient.PostAsync(GetEndpoint("CreateEdition"), input);
        }

        public async Task<ListResultDto<EditionListDto>> GetEditions()
        {
            return await ApiClient.GetAsync<ListResultDto<EditionListDto>>(GetEndpoint(nameof(GetEditions)));
        }

        public async Task CreateEdition(CreateEditionDto input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(CreateEdition)), input);
        }

        public async Task UpdateEdition(UpdateEditionDto input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateEdition)), input);
        }

        public async Task DeleteEdition(EntityDto input)
        {
            await ApiClient.DeleteAsync(GetEndpoint(nameof(DeleteEdition)), input);
        }
    }
}