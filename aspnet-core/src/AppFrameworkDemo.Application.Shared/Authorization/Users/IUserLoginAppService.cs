using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts(GetLoginAttemptsInput input);
    }
}