using System;
using System.Threading.Tasks;

namespace AppFramework.Services.Account
{
    public interface IUserConfigurationManager
    {
        Task GetConfigurationAsync(
            Func<string, Task> OnAccessTokenRefresh = null,
            Func<Task> OnSessionTimeOut = null,
            Func<Task> successCallback = null);
    }
}