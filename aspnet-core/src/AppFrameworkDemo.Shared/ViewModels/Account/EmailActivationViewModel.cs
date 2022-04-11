using Acr.UserDialogs;
using AppFrameworkDemo.Authorization.Accounts;
using AppFrameworkDemo.Authorization.Accounts.Dto;
using AppFrameworkDemo.Shared.Localization;
using Prism.Commands;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class EmailActivationViewModel : DialogViewModel
    {
        public DelegateCommand SendEmailActivationCommand { get; private set; }

        private readonly IAccountAppService _accountAppService;
        private bool _isEmailActivationEnabled;

        public EmailActivationViewModel(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;

            SendEmailActivationCommand=new DelegateCommand(SendEmailActivationAsync);
        }

        private string _emailAddress;

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                SetEmailActivationButtonEnabled();
                RaisePropertyChanged();
            }
        }

        public bool IsEmailActivationEnabled
        {
            get => _isEmailActivationEnabled;
            set
            {
                _isEmailActivationEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void SetEmailActivationButtonEnabled()
        {
            IsEmailActivationEnabled = !string.IsNullOrWhiteSpace(EmailAddress);
        }

        private async void SendEmailActivationAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(
                    async () =>
                    await _accountAppService.SendEmailActivationLink(new SendEmailActivationLinkInput { EmailAddress = EmailAddress }),
                    PasswordResetMailSentAsync
                );
            });
        }

        private async Task PasswordResetMailSentAsync()
        {
            await UserDialogs.Instance.AlertAsync(Local.Localize(AppLocalizationKeys.ActivationMailSentMessage), 
                Local.Localize(AppLocalizationKeys.MailSent), Local.Localize(AppLocalizationKeys.Ok));

            base.OnSave();
        }
    }
}