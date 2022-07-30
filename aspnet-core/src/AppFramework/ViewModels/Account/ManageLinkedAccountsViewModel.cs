using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Common;
using AppFramework.Services;
using AppFramework.ViewModels.Shared;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ManageLinkedAccountsViewModel : HostDialogViewModel
    {
        private readonly IUserLinkAppService appService;
        private readonly IHostDialogService dialog;

        public IDataPagerService dataPager { get; private set; }

        public GetLinkedUsersInput input;

        public ManageLinkedAccountsViewModel(IUserLinkAppService appService, IDataPagerService dataPager, IHostDialogService dialog)
        {
            this.appService = appService;
            this.dataPager = dataPager;
            this.dialog = dialog;
            input = new GetLinkedUsersInput()
            {
                MaxResultCount = 10,
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await GetRecentlyUsedLinkedUsers(input);
        }

        private async Task GetRecentlyUsedLinkedUsers(GetLinkedUsersInput filter)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetLinkedUsers(filter), async result =>
                {
                    dataPager.SetList(result);

                    await Task.CompletedTask;
                });
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await GetRecentlyUsedLinkedUsers(input);
        }
    }
}
