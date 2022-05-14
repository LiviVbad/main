using Abp.Application.Services;
using AppFramework.Sessions.Dto;
using System.Threading.Tasks;

namespace AppFramework.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}