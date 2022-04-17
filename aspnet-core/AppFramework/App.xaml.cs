using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Common.Core;
using AppFramework.Views;
using AppFramework.Common.Services.Account;
using AppFramework.Common.Services.Navigation;
using AppFramework.Extensions;
using AppFramework.Services; 
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AppFramework
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static IAccountService accountService;
        //public static IThemeService themeService;
        //public static IResourceService resourceService;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.ConfigurationServices();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoMapper(config =>
            {
                config.AddProfile<AppMapper>();
            });
            return new DryIocContainerExtension(new Container(CreateContainerRules())
                .WithDependencyInjectionAdapter(serviceCollection));
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.ConfigurationAdapters(Container);
        }

        protected override async void OnInitialized()
        {
            accountService = Container.Resolve<IAccountService>();
            var resourceService = Container.Resolve<IResourceService>();
            resourceService.AddCustomResources(Current.Resources);

            //themeService = Container.Resolve<IThemeService>();
            //themeService.ThemeChanged += ThemeChenged;

            //var configurationManager = Container.Resolve<IUserConfigurationManager>();
            //await configurationManager.GetConfigurationAsync(OnAccessTokenRefresh, OnSessionTimeout);

            if (!Authorize()) Environment.Exit(-1);

            InitializeShell();

            base.OnInitialized();
        }

        private bool Authorize()
        {
            var validationResult = Validation(Container);
            if (validationResult == ButtonResult.Retry)
                return Authorize();

            return validationResult == ButtonResult.OK;

            static ButtonResult Validation(IContainerProvider container)
            {
                ButtonResult result = ButtonResult.Cancel;
                var dialogService = container.Resolve<IDialogHostService>();
                dialogService.ShowDialog(AppViewManager.Login, callBack =>
                {
                    result = callBack.Result;
                });
                return result;
            }
        }

        private void InitializeShell()
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
                //themeService.SetDefaultTheme(view);
                //resourceService.UpdateCustomResources(Current.Resources, "MaterialDark");
                MainWindow = (Window)shell;
            }
            else
                throw new NullReferenceException("A MainView content must be a Window!");
        }

        //private void ThemeChenged(string themeName)
        //{
        //    //更新系统主题以及刷新资源的主题引用
        //    themeService.SetTheme(Current.MainWindow, themeName);
        //    resourceService.UpdateCustomResources(Current.Resources, themeName);
        //}

        public static async Task OnSessionTimeout()
        {
            await accountService.LogoutAsync();
        }

        public static async Task OnAccessTokenRefresh(string newAccessToken)
        {
            await Task.CompletedTask;
        }
    }
}