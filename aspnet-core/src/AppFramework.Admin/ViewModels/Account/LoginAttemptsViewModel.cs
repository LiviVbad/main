using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class LoginAttemptsViewModel : NavigationCurdViewModel
    {
        private readonly IUserLoginAppService appService;
        private GetLoginAttemptsInput input;
        public DelegateCommand SearchCommand { get; private set; }

        public DateTime? StartDate
        {
            get { return input.StartDate; }
            set { input.StartDate = value; RaisePropertyChanged(); }
        }

        public DateTime? EndDate
        {
            get { return input.EndDate; }
            set { input.EndDate = value; RaisePropertyChanged(); }
        }

        public string FilterText
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                RaisePropertyChanged();
                Search();
            }
        }


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

            StartDate=DateTime.Now.AddDays(-3);
            EndDate=DateTime.Now;

            SearchCommand = new DelegateCommand(Search);
            dataPager.OnPageIndexChangedEventhandler += UsersOnPageIndexChangedEventhandler;
        }

        private void Search()
        {
            dataPager.PageIndex = 0;
        }

        private async void UsersOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.StartDate = Convert.ToDateTime(Convert.ToDateTime(StartDate).ToString("D"));
            input.EndDate = Convert.ToDateTime((Convert.ToDateTime(EndDate).AddDays(1).ToString("D"))).AddSeconds(-1);
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await GetUserLogins(input);
            });
        }

        private async Task GetUserLogins(GetLoginAttemptsInput filter)
        {
            await WebRequest.Execute(() => appService.GetUserLoginAttempts(filter), dataPager.SetList);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetUserLogins(input);
            });
        }
    }
}
