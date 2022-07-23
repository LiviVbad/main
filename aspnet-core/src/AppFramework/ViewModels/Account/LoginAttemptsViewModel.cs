using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Common;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LoginAttemptsViewModel : NavigationCurdViewModel
    {
        private readonly IUserLoginAppService appService;
        private GetLoginAttemptsInput input;
        public DelegateCommand SearchCommand { get; private set; }

        public LoginAttemptsViewModel(IUserLoginAppService appService)
        {
            this.appService = appService;
            Title = Local.Localize("LoginAttempts");
            
            input = new GetLoginAttemptsInput()
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };

            SearchCommand = new DelegateCommand(Search);
            dataPager.OnPageIndexChangedEventhandler += UsersOnPageIndexChangedEventhandler;
        }

        private void Search()
        {
            dataPager.PageIndex = 0;
        }

        private async void UsersOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await GetUserLogins(input);
            });
        }

        private async Task GetUserLogins(GetLoginAttemptsInput filter)
        {
            await WebRequest.Execute(() => appService.GetUserLoginAttempts(filter),
                        async result =>
                        {
                            dataPager.SetList(result);

                            await Task.CompletedTask;
                        });
        }

        public override Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            return base.OnNavigatedToAsync(navigationContext);
        }
    }
}
