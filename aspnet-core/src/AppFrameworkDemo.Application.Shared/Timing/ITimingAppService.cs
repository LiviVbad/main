using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.Timing.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Timing
{
    public interface ITimingAppService : IApplicationService
    {
        Task<ListResultDto<NameValueDto>> GetTimezones(GetTimezonesInput input);

        Task<List<ComboboxItemDto>> GetTimezoneComboboxItems(GetTimezoneComboboxItemsInput input);
    }
}