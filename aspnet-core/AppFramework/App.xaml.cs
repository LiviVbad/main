using AppFramework.Services.Account;
using AppFramework.Services;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using AppFramework.ApiClient;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Navigation;

namespace AppFramework
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static IAccountService accountService;
        public static IThemeService themeService;
        public static IResourceService resourceService;

        protected override Window CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        { }

        protected override async void OnInitialized()
        {
            themeService = Container.Resolve<IThemeService>();
            accountService = Container.Resolve<IAccountService>();
            resourceService = Container.Resolve<IResourceService>();

            themeService.ThemeChanged += ThemeChenged;
            resourceService.AddCustomResources(Current.Resources);

            var configurationManager = Container.Resolve<IUserConfigurationManager>();
            await configurationManager?.GetConfigurationAsync(OnAccessTokenRefresh, OnSessionTimeout);

            if (!Authorize()) Environment.Exit(-1);

            InitializeShell();
            base.OnInitialized();
        }

        bool Authorize()
        {
            var validationResult = Validation(Container);
            if (validationResult == ButtonResult.Retry)
                return Authorize();

            return validationResult == ButtonResult.OK;

            static ButtonResult Validation(IContainerProvider container)
            {
                ButtonResult result = ButtonResult.Cancel;
                var dialogService = container.Resolve<IDialogHostService>();
                dialogService.ShowDialog("", callBack =>
                {
                    result = callBack.Result;
                });
                return result;
            }
        }

        void InitializeShell()
        {
            //获取用户的访问资源权限
            var appContext = Container.Resolve<IApplicationContext>();
            var navigationItemService = Container.Resolve<INavigationMenuService>();
            var Items = navigationItemService.GetAuthMenus(appContext.Configuration.Auth.GrantedPermissions);

            var shell = Container.Resolve<object>("MainView");
            if (shell is Window view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                //加载首页上下文及刷新区域
                ViewModelLocator.SetAutoWireViewModel(view, true);
                var regionManager = Container.Resolve<IRegionManager>();
                RegionManager.SetRegionManager(view, regionManager);
                RegionManager.UpdateRegions();

                //添加自定义的资源及配置默认主题
                themeService.SetDefaultTheme(view);
                resourceService.UpdateCustomResources(Current.Resources, "MaterialDark");
                MainWindow = (Window)shell;
            }
            else
                throw new NullReferenceException("A MainView content must be a Window!");
        }

        void ThemeChenged(string themeName)
        {
            //更新系统主题以及刷新资源的主题引用
            themeService.SetTheme(Current.MainWindow, themeName);
            resourceService.UpdateCustomResources(Current.Resources, themeName);
        }

        protected virtual async Task OnSessionTimeout()
        {
            await accountService.LogoutAsync();
        }

        protected virtual async Task OnAccessTokenRefresh(string newAccessToken)
        {
            await Task.CompletedTask;
        }
    }
}
