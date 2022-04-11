using Abp.Localization;
using AppFrameworkDemo.ApiClient;
using AppFrameworkDemo.Authorization.Users.Dto;
using AppFrameworkDemo.Authorization.Users.Profile;
using AppFrameworkDemo.Shared.Services.Account;
using Prism.Commands;
using Prism.Navigation;
using Prism.Regions.Navigation;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class SettingsViewModel : RegionViewModel
    {
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand ChangePasswordCommand { get; private set; }

        private readonly IAccountService accountService;
        private readonly IDialogService dialogService;
        private readonly IApplicationContext applicationContext;
        private readonly IProfileAppService profileAppService;

        private ObservableCollection<LanguageInfo> languages;
        private LanguageInfo selectedLanguage;

        private bool isInitialized;

        public SettingsViewModel(
            IDialogService dialogService,
            IApplicationContext applicationContext,
            IProfileAppService profileAppService,
            IAccountService accountService)
        {
            LogoutCommand = new DelegateCommand(() => accountService.LogoutAsync());
            ChangePasswordCommand = new DelegateCommand(() => AsyncRunner.Run(ChangePasswordAsync()));
            this.dialogService = dialogService;
            this.applicationContext = applicationContext;
            this.profileAppService = profileAppService;
            this.accountService = accountService;
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
                    await profileAppService.ChangeLanguage(new ChangeUserLanguageDto
                    {
                        LanguageName = selectedLanguage.Name
                    }),
                    () =>
                    {
                        AppDialogHelper.Success("ChangeLanguage", Localization.LocalizationSource.LocalTranslation);
                        return Task.CompletedTask;
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