using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Storage;
using AppFramework.Localization;
using AppFramework.Services.Dialog;
using AppFramework.Sessions;
using AppFramework.Sessions.Dto;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace AppFramework.Services.Account
{
    public class AccountService : BindableBase, IAccountService
    {
        public readonly IAccountStorageService dataStorageService;
        public readonly IApplicationContext applicationContext;
        public readonly ISessionAppService sessionAppService;
        public readonly IAccessTokenManager accessTokenManager;
        private readonly IAppDialogService appDialogService;
        public readonly IProfileAppService profileAppService;

        public AccountService(
            IAppDialogService appDialogService,
            IProfileAppService profileAppService,
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccessTokenManager accessTokenManager,
            IAccountStorageService dataStorageService,
            AbpAuthenticateModel authenticateModel)
        {
            this.appDialogService = appDialogService;
            this.profileAppService = profileAppService;
            this.applicationContext = applicationContext;
            this.sessionAppService = sessionAppService;
            this.accessTokenManager = accessTokenManager;
            this.dataStorageService = dataStorageService;
            this.AuthenticateModel = authenticateModel;
        }

        public AbpAuthenticateModel AuthenticateModel { get; set; }
        public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        public async Task<bool> LoginUserAsync()
        {
            bool loginResult = false;
            await WebRequest.Execute(
                accessTokenManager.LoginAsync,
                async result =>
                {
                    await AuthenticateSucceed(result);
                    loginResult = true;
                });
            return loginResult;
        }

        public async Task LogoutAsync()
        {
            accessTokenManager.Logout();
            applicationContext.ClearLoginInfo();
            dataStorageService.ClearSessionPersistance();
            await GoToLoginPageAsync();
        }

        private async Task GoToLoginPageAsync()
        {
            await Task.CompletedTask;
        }

        private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;

            if (AuthenticateResultModel.ShouldResetPassword)
            {
                await appDialogService.Show("", Local.Localize("ChangePasswordToLogin"));
                return;
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                //await _navigationService.SetMainPage<SendTwoFactorCodeView>(AuthenticateResultModel);
                return;
            }

            //if (!AbpAuthenticateModel.IsTwoFactorVerification)
            //{
            //    await _dataStorageService.StoreAuthenticateResultAsync(AuthenticateResultModel);
            //}

            AuthenticateModel.Password = null;
            await SetCurrentUserInfoAsync();
            await UserConfigurationManager.GetAsync();
        }

        public async Task SetCurrentUserInfoAsync()
        {
            await WebRequest.Execute(async () =>
                await sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecuted);
        }

        public async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
        {
            applicationContext.SetLoginInfo(result);
            await dataStorageService.StoreLoginInformationAsync(applicationContext.LoginInfo);
        }
    }
}