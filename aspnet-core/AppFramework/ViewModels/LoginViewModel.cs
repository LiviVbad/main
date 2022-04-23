using Abp.Localization;
using AppFramework.ApiClient;
using AppFramework.Authorization.Accounts;
using AppFramework.Authorization.Accounts.Dto;
using AppFramework.Common;
using AppFramework.Common.Services.Account;
using AppFramework.Services;
using AppFramework.Services.Account;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {
        #region 字段/属性

        public DelegateCommand<string> ExecuteCommand { get; }
        public DelegateCommand<LanguageInfo> ChangeLanguageCommand { get; }

        private readonly IAppHostDialogService dialogService;
        private readonly IAccountService accountService;
        private readonly IAccountAppService accountAppService;
        private readonly IApplicationContext applicationContext;


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
            IAppHostDialogService dialogService,
            IAccountService accountService,
            IAccountAppService accountAppService,
            IApplicationContext applicationContext)
        {
            this.dialogService = dialogService;
            this.accountService = accountService;
            this.accountAppService = accountAppService;
            this.applicationContext = applicationContext;

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
            DialogExtensions.ShowDialog(dialogService, AppViewManager.ForgotPassword);
        }

        public void EmailActivationAsync()
        {
            DialogExtensions.ShowDialog(dialogService, AppViewManager.EmailActivation);
        }

        public async void ChangeLanguage(LanguageInfo languageInfo)
        {
            applicationContext.CurrentLanguage = languageInfo;

            await SetBusyAsync(async () =>
             {
                 await UserConfigurationManager.GetAsync();
             });

            OnDialogClosed(new DialogResult(ButtonResult.Retry));
        }

        public void ChangeTenantAsync()
        { }

        private async Task LoginUserAsync()
        {
            await SetBusyAsync(async () =>
            {
                var loginResult = await accountService.LoginUserAsync();

                if (loginResult) OnDialogClosed();
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
                    await dialogService.Question("InActive",
                        Local.Localize("TenantIsNotActive", tenancyName), "Login");
                    break;

                case TenantAvailabilityState.NotFound:
                    await dialogService.Question("NotFound", 
                        Local.Localize("ThereIsNoTenantDefinedWithName{0}", tenancyName), "Login");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            await Task.CompletedTask;
        }

        #endregion 

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(async () => await UserConfigurationManager.GetIfNeedsAsync(),
                      async () =>
                      {
                          var configuration = applicationContext.Configuration;
                          if (configuration != null)
                          {
                              Languages = new ObservableCollection<LanguageInfo>(configuration.Localization.Languages);
                              SelectedLanguage = Languages.FirstOrDefault(l => l.Name == applicationContext.CurrentLanguage.Name);

                              if (applicationContext.CurrentTenant != null)
                              {
                                  IsMultiTenancyEnabled = configuration.MultiTenancy.IsEnabled;
                                  CurrentTenancyNameOrDefault = applicationContext.CurrentTenant.TenancyName;
                              }
                          }
                          else
                          {
                              CurrentTenancyNameOrDefault = Local.Localize(AppLocalizationKeys.NotSelected);
                          }

                          await Task.CompletedTask;
                      });
             });
        }
    }
}