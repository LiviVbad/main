using AppFrameworkDemo.ApiClient.Models;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AuthenticateModel { get; set; }

        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        Task LoginUserAsync();

        Task LogoutAsync();
    }
}