using Abp.Application.Services;
using AppFrameworkDemo.Editions.Dto;
using AppFrameworkDemo.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.MultiTenancy
{
    public interface ITenantRegistrationAppService : IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}