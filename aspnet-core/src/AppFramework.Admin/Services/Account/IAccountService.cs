using AppFramework.ApiClient.Models;
using AppFramework.Models;
using System.Threading.Tasks;

namespace AppFramework.Services
{
    public interface IAccountService
    {
        AbpAuthenticateModel AuthenticateModel { get; set; }

        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        Task LoginUserAsync();

        Task LoginCurrentUserAsync(UserListModel user);

        Task LogoutAsync();
    }
}