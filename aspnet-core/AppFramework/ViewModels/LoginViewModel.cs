using Abp.Localization;
using AppFramework.ApiClient;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Storage;
using AppFramework.Services;
using AppFramework.Services.Account;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {
        public LoginViewModel(IDialogHostService dialog,
            IAccountService accountService,
            IAccountAppService accountAppService,
            IApplicationContext applicationContext,
            IProfileAppService profileAppService,
            IUserConfigurationManager userConfigurationManager,
            IDataStorageService dataStorageService)
        {
            this.dialog = dialog;
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

        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand<LanguageInfo> ChangeLanguageCommand { get; }

        private readonly IDialogHostService dialog;
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

        public async Task ForgotPasswordAsync()
        { }

        public async Task EmailActivationAsync()
        { }

        public async void ChangeLanguage(LanguageInfo languageInfo)
        { }

        private async Task LoginUserAsync()
        { }

        public async Task ChangeTenantAsync()
        { }

        public async Task SetTenantAsync(string tenancyName)
        { }

        public async Task PageAppearingAsync()
        { }

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