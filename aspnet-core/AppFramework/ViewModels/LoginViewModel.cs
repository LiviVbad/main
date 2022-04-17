using Abp.Localization;
using AppFramework.ApiClient;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Storage;
using AppFramework.Localization;
using AppFramework.Services;
using AppFramework.Services.Account;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {
        #region 字段/属性

        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand<LanguageInfo> ChangeLanguageCommand { get; }

        private readonly IDialogHostService dialogService;
        private readonly IAccountService accountService;
        private readonly IAccountAppService accountAppService;
        private readonly IApplicationContext applicationContext;
        private readonly IProfileAppService profileAppService;
        private readonly IUserConfigurationManager userConfigurationManager;
        private readonly IDataStorageService dataStorageService;


        private LanguageInfo selectedLanguage;
        public string CurrentTenancyNameOrDefault { get; set; }
        private ObservableCollection<LanguageInfo> languages;
        private string tenancyName;
        private bool isLoginEnabled;

        public bool IsMultiTenancyEnabled { get; set; }

        public LanguageInfo SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<LanguageInfo> Languages
        {
            get => languages;
            set
            {
                languages = value;
                RaisePropertyChanged();
            }
        }

        public string TenancyName
        {
            get => tenancyName;
            set
            {
                tenancyName = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get => accountService.AuthenticateModel.UserNameOrEmailAddress;
            set
            {
                accountService.AuthenticateModel.UserNameOrEmailAddress = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => accountService.AuthenticateModel.Password;
            set
            {
                accountService.AuthenticateModel.Password = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public bool IsLoginEnabled
        {
            get { return isLoginEnabled; }
            set { isLoginEnabled = value; RaisePropertyChanged(); }
        }

        #endregion

        public LoginViewModel(
            IDialogHostService dialogService,
            IAccountService accountService,
            IAccountAppService accountAppService,
            IApplicationContext applicationContext,
            IProfileAppService profileAppService,
            IUserConfigurationManager userConfigurationManager,
            IDataStorageService dataStorageService)
        {
            this.dialogService = dialogService;
            this.accountService = accountService;
            this.accountAppService = accountAppService;
            this.applicationContext = applicationContext;
            this.profileAppService = profileAppService;
            this.userConfigurationManager = userConfigurationManager;
            this.dataStorageService = dataStorageService;

            ExecuteCommand = new DelegateCommand<string>(Execute);
            ChangeLanguageCommand = new DelegateCommand<LanguageInfo>(ChangeLanguage);
            UserName = "admin";
            Password = "123qwe";
        }

        private async void Execute(string arg)
        {
            switch (arg)
            {
                case "LoginUser": await LoginUserAsync(); break;
                case "ChangeTenant": ChangeTenantAsync(); break;
                case "ForgotPassword": ForgotPasswordAsync(); break;
                case "EmailActivation": EmailActivationAsync(); break;
            }
        }

        #region 忘记密码/激活邮件/修改语言/登录/租户

        public void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        public void ForgotPasswordAsync()
        {
            dialogService.ShowViewDialog(AppViewManager.ForgotPassword);
        }

        public void EmailActivationAsync()
        {
            dialogService.ShowViewDialog(AppViewManager.EmailActivation);
        }

        public void ChangeLanguage(LanguageInfo languageInfo)
        { }

        public void ChangeTenantAsync()
        { }

        private async Task LoginUserAsync()
        {
            await SetBusyAsync(async () =>
            {
                await accountService.LoginUserAsync();
            });
        }

        public async Task SetTenantAsync(string tenancyName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                    async () => await accountAppService.IsTenantAvailable(
                        new IsTenantAvailableInput { TenancyName = tenancyName }),
                    result => IsTenantAvailableExecuted(result, tenancyName)
                );
            });
        }

        public async Task IsTenantAvailableExecuted(IsTenantAvailableOutput result, string tenancyName)
        {
            var tenantAvailableResult = result;
            IsBusy = false;

            switch (tenantAvailableResult.State)
            {
                case TenantAvailabilityState.Available:
                    applicationContext.SetAsTenant(tenancyName, tenantAvailableResult.TenantId.Value);
                    ApiUrlConfig.ChangeBaseUrl(tenantAvailableResult.ServerRootAddress);
                    RaisePropertyChanged(CurrentTenancyNameOrDefault);
                    break;

                case TenantAvailabilityState.InActive:
                    await applayer.Show("InActive", Local.Localize("TenantIsNotActive", tenancyName));
                    break;

                case TenantAvailabilityState.NotFound:
                    await applayer.Show("NotFound", Local.Localize("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            await Task.CompletedTask;
        }

        #endregion 

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await userConfigurationManager.GetConfigurationAsync(App.OnAccessTokenRefresh, App.OnSessionTimeout);
        }
    }
}