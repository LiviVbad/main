using AppFramework.ApiClient.Models;
using System.Threading.Tasks;

namespace AppFramework.Common.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AuthenticateModel { get; set; }

        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        Task<bool> LoginUserAsync();

        Task LogoutAsync();
    }
}