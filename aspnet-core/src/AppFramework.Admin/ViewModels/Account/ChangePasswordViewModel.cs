using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Shared;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ChangePasswordViewModel : HostDialogViewModel
    {
        public DelegateCommand SendChangePasswordCommand { get; private set; }

        private readonly IProfileAppService profileAppService;
        private bool _isChangePasswordEnabled;

        public ChangePasswordViewModel(IProfileAppService profileAppService)
        {
            this.profileAppService = profileAppService;
            SendChangePasswordCommand = new DelegateCommand(SendChangePasswordAsync);
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
                NotifyBar.Info("", Local.Localize(AppLocalizationKeys.PasswordsDontMatch));
            }
            else
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => profileAppService.ChangePassword(new ChangePasswordInput
                    {
                        CurrentPassword = CurrentPassword,
                        NewPassword = NewPassword
                    }), PasswordChangedAsync);
                });
            }
        }

        private async Task PasswordChangedAsync()
        {
            NotifyBar.Info(Local.Localize(AppLocalizationKeys.YourPasswordHasChangedSuccessfully),
                Local.Localize(AppLocalizationKeys.ChangePassword));

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
