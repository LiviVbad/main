using Abp.Application.Services;
using AppFrameworkDemo.Sessions.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}