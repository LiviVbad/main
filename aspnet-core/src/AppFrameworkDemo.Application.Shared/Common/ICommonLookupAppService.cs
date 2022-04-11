using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.Common.Dto;
using AppFrameworkDemo.Editions.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}