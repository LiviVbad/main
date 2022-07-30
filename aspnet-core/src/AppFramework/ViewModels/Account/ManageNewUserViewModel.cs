using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Dto;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ManageNewUserViewModel : HostDialogViewModel
    {
        private readonly ICommonLookupAppService lookupAppService;
        public FindUsersInput input;

        public ManageNewUserViewModel(ICommonLookupAppService lookupAppService,
            IDataPagerService dataPager)
        {
            this.lookupAppService = lookupAppService;
            this.dataPager = dataPager;
            QueryCommand = new DelegateCommand(Query);

            input = new FindUsersInput()
            {
                MaxResultCount = 10,
                ExcludeCurrentUser = true
            };
        }

        public DelegateCommand QueryCommand { get; private set; }

        public IDataPagerService dataPager { get; private set; }

        private void Query()
        {

        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => lookupAppService.FindUsers(input), FindUsersSuccessed);
            });
        }

        private async Task FindUsersSuccessed(PagedResultDto<NameValueDto> output)
        {
            dataPager.SetList(output);

            await Task.CompletedTask;
        }
    }
}
