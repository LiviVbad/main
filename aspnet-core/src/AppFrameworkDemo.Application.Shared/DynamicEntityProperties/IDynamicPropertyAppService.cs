using Abp.Application.Services.Dto;
using Abp.UI.Inputs;
using AppFrameworkDemo.DynamicEntityProperties.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.DynamicEntityProperties
{
    public interface IDynamicPropertyAppService
    {
        Task<DynamicPropertyDto> Get(int id);

        Task<ListResultDto<DynamicPropertyDto>> GetAll();

        Task Add(DynamicPropertyDto dto);

        Task Update(DynamicPropertyDto dto);

        Task Delete(EntityDto input);

        IInputType FindAllowedInputType(string name);
    }
}