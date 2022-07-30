﻿using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Common;
using AppFramework.Services;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
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

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<LinkedUserDto> DeleteCommand { get; private set; }
        public DelegateCommand<LinkedUserDto> LoginUserCommand { get; private set; }

        public ManageLinkedAccountsViewModel(IUserLinkAppService appService, IDataPagerService dataPager, IHostDialogService dialog)
        {
            this.appService = appService;
            this.dataPager = dataPager;
            this.dialog = dialog;
            input = new GetLinkedUsersInput()
            {
                MaxResultCount = 10,
            };
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand<LinkedUserDto>(Delete);
            LoginUserCommand = new DelegateCommand<LinkedUserDto>(LoginUser);
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;
        }

        private void LoginUser(LinkedUserDto obj)
        {
            //..
        }

        private async void Delete(LinkedUserDto obj)
        {
            if (await dialog.Question(
                Local.Localize("LinkedUserDeleteWarningMessage", obj.Username), "ManageLinkedAccounts"))
            {
                await WebRequest.Execute(() => appService.UnlinkUser(new UnlinkUserInput()
                {
                    TenantId = obj.TenantId,
                    UserId = obj.Id
                }), async () => await GetRecentlyUsedLinkedUsers(input));
            }
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await GetRecentlyUsedLinkedUsers(input);
        }

        private async void Add()
        {
            var dialogResult = await dialog.ShowDialogAsync(
                AppViewManager.CreateLinkedAccount, null, "ManageLinkedAccounts");
            if (dialogResult.Result == ButtonResult.OK)
            {
                await GetRecentlyUsedLinkedUsers(input);
            }
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
