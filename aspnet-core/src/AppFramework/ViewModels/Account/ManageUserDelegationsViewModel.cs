using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users.Delegation;
using AppFramework.Authorization.Users.Delegation.Dto;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Common;
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
    public class ManageUserDelegationsViewModel : HostDialogViewModel
    {
        private readonly IUserDelegationAppService appService;
        public IDataPagerService dataPager { get; private set; }
        public DelegateCommand AddCommand { get; private set; }

        public GetUserDelegationsInput input;

        public ManageUserDelegationsViewModel(
            IUserDelegationAppService appService,
            IDataPagerService dataPager)
        {
            this.appService = appService;
            this.dataPager = dataPager;
            AddCommand = new DelegateCommand(Add);

            input = new GetUserDelegationsInput()
            {
                MaxResultCount = 10,
            };
        }

        private void Add()
        {

        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                        appService.GetDelegatedUsers(input),
                        GetDelegatedUsersSuccessed);
            });
        }

        private async Task GetDelegatedUsersSuccessed(PagedResultDto<UserDelegationDto> output)
        { 
            dataPager.SetList(output);

            await Task.CompletedTask;
        }
    }
}
