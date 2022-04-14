using AppFrameworkDemo.ApiClient.Models;
using System.Threading.Tasks;

namespace AppFramework.Shared.Common.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AuthenticateModel { get; set; }

        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        Task LoginUserAsync();

        Task LogoutAsync();
    }
}