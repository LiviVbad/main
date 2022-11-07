using AppFramework.ApiClient.Models;
using AppFramework.Authorization.Accounts; 
using AppFramework.Services; 
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using AppFramework.Shared;

namespace AppFramework.ViewModels
{
    public class SendTwoFactorCodeViewModel : HostDialogViewModel
    {
        private readonly IHostDialogService dialog;
        private readonly ProxyTokenAuthControllerService proxyTokenAuthControllerService;
        private readonly IAccountService accountService;

        public DelegateCommand SendSecurityCodeCommand { get; private set; }

        public SendTwoFactorCodeViewModel(IHostDialogService dialog,
            ProxyTokenAuthControllerService proxyTokenAuthControllerService,
            IAccountService accountService)
        {
            this.dialog = dialog;
            this.proxyTokenAuthControllerService = proxyTokenAuthControllerService;
            this.accountService = accountService;
            _twoFactorAuthProviders = new List<string>();

            SendSecurityCodeCommand = new DelegateCommand(SendSecurityCodeAsync);
        }

        private List<string> _twoFactorAuthProviders;

        public List<string> TwoFactorAuthProviders
        {
            get => _twoFactorAuthProviders;
            set
            {
                _twoFactorAuthProviders = value;
                RaisePropertyChanged();
            }
        }

        private string _selectedProvider;

        public string SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                _selectedProvider = value;
                RaisePropertyChanged();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            accountService.AuthenticateResultModel = parameters
              .GetValue<AbpAuthenticateResultModel>("Value");

            TwoFactorAuthProviders = accountService
                .AuthenticateResultModel.TwoFactorAuthProviders.ToList();
            SelectedProvider = TwoFactorAuthProviders.FirstOrDefault();
        }

        private async void SendSecurityCodeAsync()
        {
            await SetBusyAsync(async () =>
            {
                await proxyTokenAuthControllerService.SendTwoFactorAuthCode(
                    accountService.AuthenticateResultModel.UserId,
                    _selectedProvider
                );
            });

            var promptResult = await dialog.ShowDialogAsync("");

            if (promptResult.Result == ButtonResult.OK)
            {
                var twoFactorVerificationCode = promptResult.Parameters.GetValue<string>("Value");

                if (!string.IsNullOrEmpty(twoFactorVerificationCode))
                {
                    accountService.AuthenticateModel.TwoFactorVerificationCode = twoFactorVerificationCode;
                    accountService.AuthenticateModel.RememberClient = true;
                    await SetBusyAsync(async () =>
                    {
                        await accountService.LoginUserAsync();
                    });
                }
            }
        }
    }
}
