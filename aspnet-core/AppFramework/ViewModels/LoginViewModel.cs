using Abp.Localization; 
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppFramework.Services.Account;
using Prism.Ioc; 
using AppFramework.WindowHost;
using AppFramework.Services; 
using AppFrameworkDemo.Authorization.Accounts;
using AppFrameworkDemo.ApiClient;
using AppFrameworkDemo.Authorization.Users.Profile;
using AppFrameworkDemo.Authorization.Accounts.Dto;
using AppFramework.Application.Common.Services.Account;
using AppFramework.Application.Common.Services.Storage;

namespace AppFramework.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {
        public LoginViewModel(IContainerProvider provider)
        {
            this.dialog=provider.Resolve<IDialogHostService>();
            this.accountService = provider.Resolve<IAccountService>();
            this.accountAppService = provider.Resolve<IAccountAppService>();
            this.applicationContext = provider.Resolve<IApplicationContext>();
            this.profileAppService =provider.Resolve<IProfileAppService>();
            this.userConfigurationManager = provider.Resolve<IUserConfigurationManager>();
            this.dataStorageService= provider.Resolve<IDataStorageService>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            ChangeLanguageCommand = new DelegateCommand<LanguageInfo>(ChangeLanguage);

            UserName = "admin";
            Password = "123qwe";
        }

        public DelegateCommand<string> ExecuteCommand { get; }

        public DelegateCommand<LanguageInfo> ChangeLanguageCommand { get; }

        private readonly IDialogHostService dialog;
        private LanguageInfo selectedLanguage;
        public readonly IAccountAppService accountAppService;
        public readonly IApplicationContext applicationContext;
        public readonly IProfileAppService profileAppService;
        public readonly IUserConfigurationManager userConfigurationManager;
        private readonly IDataStorageService dataStorageService;
        public IAccountService accountService { get; private set; }
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

        private async void Execute(string arg)
        {
            switch (arg)
            {
                case "LoginUser": await LoginUserAsync(); break;
                case "ChangeTenant": await ChangeTenantAsync(); break;
                case "ForgotPassword": await ForgotPasswordAsync(); break;
                case "EmailActivation": await EmailActivationAsync(); break;
                case "PageAppearing": await PageAppearingAsync(); break;
            }
        }

        public void SetLoginButtonEnabled()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        public async Task ForgotPasswordAsync() { }

        public async Task EmailActivationAsync() { }

        public async void ChangeLanguage(LanguageInfo languageInfo) { }

        private async Task LoginUserAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRuner.Execute(accountService.LoginUserAsync,
                     () =>
                     {
                         OnDialogClosed();
                         return Task.CompletedTask;
                     },
                     ex =>
                     {
                         dialog.Error(ex.Message, "Login");
                         return Task.CompletedTask;
                     });
            });
        }


        public async Task ChangeTenantAsync() { }

        public async Task SetTenantAsync(string tenancyName) { }

        public async Task PageAppearingAsync() { }

        private void PopulateLoginInfoFromStorage()
        {
            //var loginInfo = _dataStorageService.RetrieveLoginInfo();
            //if (loginInfo == null)
            //{
            //    return;
            //}

            //if (loginInfo.User != null)
            //{
            //    UserName = loginInfo.User.UserName;
            //}

            //if (loginInfo.Tenant != null)
            //{
            //    TenancyName = loginInfo.Tenant.TenancyName;
            //}

            //if (loginInfo.Tenant == null)
            //{
            //    _applicationContext.SetAsHost();
            //}
            //else
            //{
            //    _applicationContext.SetAsTenant(TenancyName, loginInfo.Tenant.Id);
            //}

            RaisePropertyChanged(CurrentTenancyNameOrDefault);
        }

        public async Task IsTenantAvailableExecuted(IsTenantAvailableOutput result, string tenancyName)
        {
            var tenantAvailableResult = result;

            switch (tenantAvailableResult.State)
            {
                case TenantAvailabilityState.Available:
                    applicationContext.SetAsTenant(tenancyName, tenantAvailableResult.TenantId.Value);
                    ApiUrlConfig.ChangeBaseUrl(tenantAvailableResult.ServerRootAddress);
                    RaisePropertyChanged(CurrentTenancyNameOrDefault);
                    break;
                case TenantAvailabilityState.InActive:
                    //UserDialogs.Instance.HideLoading();
                    //await UserDialogs.Instance.AlertAsync(L.Localize("TenantIsNotActive", tenancyName));
                    break;
                case TenantAvailabilityState.NotFound:
                    //UserDialogs.Instance.HideLoading();
                    //await UserDialogs.Instance.AlertAsync(L.Localize("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await Task.CompletedTask;
        }
    }
}