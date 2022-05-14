using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.Common.Dto;
using AppFramework.Editions.Dto;
using System.Threading.Tasks;

namespace AppFramework.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}