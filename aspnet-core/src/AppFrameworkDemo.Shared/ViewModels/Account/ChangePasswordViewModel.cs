using Acr.UserDialogs;
using AppFrameworkDemo.Authorization.Users.Profile;
using AppFrameworkDemo.Authorization.Users.Profile.Dto;
using AppFrameworkDemo.Shared.Localization;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class ChangePasswordViewModel : DialogViewModel
    {
        public DelegateCommand SendChangePasswordCommand { get; private set; }

        private readonly IProfileAppService _profileAppService;
        private readonly INavigationService navigationService;
        private bool _isChangePasswordEnabled;

        public ChangePasswordViewModel(IProfileAppService profileAppService,
            INavigationService navigationService)
        {
            _profileAppService = profileAppService;
            this.navigationService=navigationService;
            SendChangePasswordCommand=new DelegateCommand(SendChangePasswordAsync);
        }

        private string _currentPassword;

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                _currentPassword = value;
                SetChangePasswordButtonEnabled();
                RaisePropertyChanged();
            }
        }

        private string _newPassword;

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                SetChangePasswordButtonEnabled();
                RaisePropertyChanged();
            }
        }

        private string _newPasswordRepeat;

        public string NewPasswordRepeat
        {
            get => _newPasswordRepeat;
            set
            {
                _newPasswordRepeat = value;
                SetChangePasswordButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public bool IsChangePasswordEnabled
        {
            get => _isChangePasswordEnabled;
            set
            {
                _isChangePasswordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void SetChangePasswordButtonEnabled()
        {
            IsChangePasswordEnabled = !string.IsNullOrWhiteSpace(CurrentPassword)
                                      && !string.IsNullOrWhiteSpace(NewPassword)
                                      && !string.IsNullOrWhiteSpace(NewPasswordRepeat);
        }

        private async void SendChangePasswordAsync()
        {
            if (NewPassword != NewPasswordRepeat)
            {
                await UserDialogs.Instance.AlertAsync(Local.Localize(AppLocalizationKeys.PasswordsDontMatch));
            }
            else
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequestRuner.Execute(
                        async () =>
                            await _profileAppService.ChangePassword(new ChangePasswordInput
                            {
                                CurrentPassword = CurrentPassword,
                                NewPassword = NewPassword
                            }),
                        PasswordChangedAsync
                    );
                });
            }
        }

        private async Task PasswordChangedAsync()
        {
            await UserDialogs.Instance.AlertAsync(Local.Localize(AppLocalizationKeys.YourPasswordHasChangedSuccessfully), 
                Local.Localize(AppLocalizationKeys.ChangePassword), Local.Localize(AppLocalizationKeys.Ok));

            await navigationService.NavigateAsync(AppViewManager.Main);
        }
    }
}