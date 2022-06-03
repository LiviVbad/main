﻿using Abp.Localization;
using AppFramework.Common.Services.Account;
using AppFramework.ApiClient;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using Prism.Commands;
using Prism.Regions.Navigation;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Common;
using AppFramework.Shared.Services.Account;
using AppFramework.Common.Core;

namespace AppFramework.Shared.ViewModels
{
    public class SettingsViewModel : RegionViewModel
    {
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand ChangePasswordCommand { get; private set; }

        private readonly IAccountService accountService;
        private readonly IMessenger messenger;
        private readonly IDialogService dialogService;
        private readonly IApplicationContext applicationContext;
        private readonly IProfileAppService profileAppService;

        private ObservableCollection<LanguageInfo> languages;
        private LanguageInfo selectedLanguage;

        private bool isInitialized;

        public SettingsViewModel(IDialogService dialogService,
            IApplicationContext applicationContext,
            IProfileAppService profileAppService,
            IAccountService accountService,
            IMessenger messenger)
        {
            LogoutCommand = new DelegateCommand(() => accountService.LogoutAsync());
            ChangePasswordCommand = new DelegateCommand(() => AsyncRunner.Run(ChangePasswordAsync()));
            this.dialogService = dialogService;
            this.applicationContext = applicationContext;
            this.profileAppService = profileAppService;
            this.accountService = accountService;
            this.messenger = messenger;
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

        public LanguageInfo SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                RaisePropertyChanged();
                if (isInitialized) AsyncRunner.Run(ChangeLanguage());
            }
        }

        private async Task ChangeLanguage()
        {
            applicationContext.CurrentLanguage = selectedLanguage;

            await WebRequestRuner.Execute(async () =>
            {
                await profileAppService.ChangeLanguage(new ChangeUserLanguageDto
                {
                    LanguageName = selectedLanguage.Name
                });
                await AppConfigurationManager.GetAsync();
            }, async () =>
            {
                messenger.Send(AppMessengerKeys.LanguageRefresh);
                await Task.CompletedTask;
            });
        }

        private async Task ChangePasswordAsync()
        {
            await dialogService.ShowDialogAsync(AppViewManager.ChangePassword);
        }

        public override void OnNavigatedTo(INavigationContext navigationContext)
        {
            Languages = new ObservableCollection<LanguageInfo>(applicationContext.Configuration.Localization.Languages);
            SelectedLanguage = languages.FirstOrDefault(l => l.Name == applicationContext.CurrentLanguage.Name);
            isInitialized = true;
        }
    }
}