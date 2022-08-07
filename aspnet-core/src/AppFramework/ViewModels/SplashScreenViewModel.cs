using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Common.Services.Storage;
using AppFramework.Localization;
using AppFramework.Services.Account;
using AppFramework.Services.Update;
using AppFramework.ViewModels.Shared;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class SplashScreenViewModel : DialogViewModel
    {
        private readonly IAccessTokenManager accessTokenManager;
        private readonly IAccountStorageService dataStorageService;
        private readonly IUpdateService updateService;
        private readonly IApplicationContext applicationContext;

        private string displayText;

        public string DisplayText
        {
            get { return displayText; }
            set { displayText = value; RaisePropertyChanged(); }
        }

        public SplashScreenViewModel(IUpdateService updateService,
           IApplicationContext applicationContext,
           IAccessTokenManager accessTokenManager,
           IAccountStorageService dataStorageService)
        {
            this.updateService = updateService;
            this.applicationContext = applicationContext;
            this.accessTokenManager = accessTokenManager;
            this.dataStorageService = dataStorageService;
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                DisplayText = LocalTranslationHelper.Localize("CheckSystemUpdates");
                await updateService.CheckVersion();

                //加载本地的缓存信息
                DisplayText = LocalTranslationHelper.Localize("Initializing");
                accessTokenManager.AuthenticateResult = dataStorageService.RetrieveAuthenticateResult();
                applicationContext.Load(dataStorageService.RetrieveTenantInfo(), dataStorageService.RetrieveLoginInfo());
                await Task.Delay(500);
                //加载系统资源
                DisplayText = LocalTranslationHelper.Localize("LoadResource");
                await UserConfigurationManager.GetIfNeedsAsync();
                if (applicationContext.Configuration == null)
                {
                    App.ExitApplication();
                    return;
                }
                //如果本地授权存在,直接进入系统首页
                if (accessTokenManager.IsUserLoggedIn)
                    OnDialogClosed();
                else
                    OnDialogClosed(ButtonResult.No);
            });
        }
    }
}
