using Abp.Application.Services;
using AppFramework.Editions.Dto;
using AppFramework.MultiTenancy.Dto;
using System.Threading.Tasks;

namespace AppFramework.MultiTenancy
{
    public interface ITenantRegistrationAppService : IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}