﻿using AppFramework.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using AppFramework.ApiClient;
using AppFramework.Shared;
using AppFramework.Shared.Services.Account;
using AppFramework.Shared.Services.Navigation;
using AppFramework.Shared.Services.Storage;
using AppFramework.Services;
using AppFramework.Services.Account;
using AppFramework.Views;
using AppFramework.Shared.Services;
using AppFramework.ViewModels.Shared;
using AppFramework.Services.Notification;
using AppFramework.ViewModels.Version;
using AppFramework.Services.Update;
using AppFramework.Shared.Services.App;

namespace AppFramework.Admin
{
    public class AdminModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        { }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.RegisterSingleton<IAppStartService, AppStartService>();
            services.RegisterSingleton<IAccountService, AccountService>();
            services.RegisterSingleton<IAccountStorageService, AccountStorageService>();
            services.RegisterSingleton<IDataStorageService, DataStorageService>();
            services.RegisterSingleton<IPermissionService, PermissionService>();
            services.RegisterSingleton<IAccessTokenManager, AccessTokenManager>();

            services.RegisterSingleton<IApplicationService, ApplicationService>();
            services.RegisterSingleton<INavigationMenuService, NavigationMenuService>();
            services.RegisterSingleton<NavigationService>();
            services.RegisterSingleton<NotificationService>();
            services.RegisterSingleton<IUpdateService, UpdateService>();

            services.RegisterDialog<SplashScreenView, SplashScreenViewModel>(AppViews.SplashScreen);
            services.RegisterDialog<LoginView, LoginViewModel>(AppViews.Login);

            services.Add<FirstChangedPwdView, FirstChangedPwdViewModel>(AppViews.FirstChangedPwd);
            services.Add<SelectTenantView, SelectTenantViewModel>(AppViews.SelectTenant);
            services.Add<MainTabsView, MainTabsViewModel>(AppViews.Main);
            services.Add<HostMessageBoxView, HostMessageViewModel>(AppViews.HostMessageBox);
            services.Add<MessageBoxView, MessageViewModel>(AppViews.MessageBox);
            services.Add<UserView, UserViewModel>(AppViews.User);
            services.Add<UserDetailsView, UserDetailsViewModel>(AppViews.UserDetails);
            services.Add<UserChangePermissionView, UserChangePermissionViewModel>(AppViews.UserChangePermission);
            services.Add<RoleView, RoleViewModel>(AppViews.Role);
            services.Add<RoleDetailsView, RoleDetailsViewModel>(AppViews.RoleDetails);
            services.Add<SelectedPermissionView, SelectedPermissionViewModel>(AppViews.SelectedPermission);
            services.Add<EditionView, EditionViewModel>(AppViews.Edition);
            services.Add<EditionDetailsView, EditionDetailsViewModel>(AppViews.EditionDetails);
            services.Add<DynamicPropertyView, DynamicPropertyViewModel>(AppViews.DynamicProperty);
            services.Add<DynamicPropertyDetailsView, DynamicPropertyDetailsViewModel>(AppViews.DynamicPropertyDetails);
            services.Add<DynamicAddEntityView, DynamicAddEntityViewModel>(AppViews.DynamicAddEntity);
            services.Add<DynamicEntityDetailsView, DynamicEntityDetailsViewModel>(AppViews.DynamicEntityDetails);
            services.Add<DynamicEditValuesView, DynamicEditValuesViewModel>(AppViews.DynamicEditValues);
            services.Add<TenantView, TenantViewModel>(AppViews.Tenant);
            services.Add<TenantDetailsView, TenantDetailsViewModel>(AppViews.TenantDetails);
            services.Add<TenantChangeFeaturesView, TenantChangeFeaturesViewModel>(AppViews.TenantChangeFeatures);
            services.Add<AddRolesView, AddRolesViewModel>(AppViews.AddRoles);
            services.Add<AddUsersView, AddUsersViewModel>(AppViews.AddUsers);
            services.Add<AuditLogsView, AuditLogsViewModel>(AppViews.AuditLog);
            services.Add<AuditLogsDetailsView, AuditLogsDetailsViewModel>(AppViews.AuditLogDetails);
            services.Add<LanguageView, LanguageViewModel>(AppViews.Language);
            services.Add<LanguageTextView, LanguageTextViewModel>(AppViews.LanguageText);
            services.Add<LanguageTextDetailsView, LanguageTextDetailsViewModel>(AppViews.LanguageTextDetails);
            services.Add<LanguageDetailsView, LanguageDetailsViewModel>(AppViews.LanguageDetails);
            services.Add<DashboardView, DashboardViewModel>(AppViews.Dashboard);
            services.Add<OrganizationsView, OrganizationsViewModel>(AppViews.Organization);
            services.Add<OrganizationsAddView, OrganizationsAddViewModel>(AppViews.OrganizationAdd);
            services.Add<SettingsView, SettingsViewModel>(AppViews.Setting);
            services.Add<DemoUiView, DemoUiViewModel>(AppViews.Demo); //演示组件页
            services.Add<VisualView, VisualViewModel>(AppViews.Visual);
            services.Add<VersionManagerView, VersionManagerViewModel>(AppViews.Version);
            services.Add<VersionManagerDetailsView, VersionManagerDetailsViewModel>(AppViews.VersionDetails);

            services.Add<NotificationView, NotificationViewModel>(AppViews.Notification);
            services.Add<MyProfileView, MyProfileViewModel>(AppViews.MyProfile);
            services.Add<LoginAttemptsView, LoginAttemptsViewModel>(AppViews.LoginAttempts);
            services.Add<ManageLinkedAccountsView, ManageLinkedAccountsViewModel>(AppViews.ManageLinkedAccounts);
            services.Add<ManageUserDelegationsView, ManageUserDelegationsViewModel>(AppViews.ManageUserDelegations);
            services.Add<SelectDateRangeView, SelectDateRangeViewModel>(AppViews.SelectDate);
            services.Add<ManageNewUserView, ManageNewUserViewModel>(AppViews.ManageNewUser);
            services.Add<CreateLinkedAccountView, CreateLinkedAccountViewModel>(AppViews.CreateLinkedAccount);
            services.Add<MySettingsView, MySettingsViewModel>(AppViews.MySetting);
            services.Add<ChangeAvatarView, ChangeAvatarViewModel>(AppViews.ChangeAvatar);
            services.Add<ChangePasswordView, ChangePasswordViewModel>(AppViews.ChangePassword);
            services.Add<EmailActivationView, EmailActivationViewModel>(AppViews.EmailActivation);
            services.Add<ForgotPasswordView, ForgotPasswordViewModel>(AppViews.ForgotPassword);
            services.Add<SendTwoFactorCodeView, SendTwoFactorCodeViewModel>(AppViews.SendTwoFactorCode);

        }


    }

    public static class ServiceExtensions
    {
        public static void Add<TView, TViewModel>(this IContainerRegistry containerRegistry, string name = null)
        {
            containerRegistry.RegisterForNavigation<TView, TViewModel>(name);
        }
    }
}