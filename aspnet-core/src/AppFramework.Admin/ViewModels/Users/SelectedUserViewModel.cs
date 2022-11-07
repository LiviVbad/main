using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class SelectedUserViewModel : HostDialogViewModel
    {
        private readonly ICommonLookupAppService appService;
        public IDataPagerService dataPager { get; private set; }
        private FindUsersInput input;

        public DelegateCommand<NameValueDto> SelectedUserCommand { get; private set; }

        public SelectedUserViewModel(IDataPagerService dataPager,
            ICommonLookupAppService appService)
        {
            this.dataPager= dataPager;
            this.appService=appService;
            input=new FindUsersInput()
            {
                MaxResultCount = 10,
            };
            SelectedUserCommand=new DelegateCommand<NameValueDto>(SelectedUser);
            this.dataPager.OnPageIndexChangedEventhandler +=DataPager_OnPageIndexChangedEventhandler;
        }

        private void SelectedUser(NameValueDto obj)
        {
            Save(Convert.ToInt32(obj.Value));
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await GetFindUsers(input);
        }

        private async Task GetFindUsers(FindUsersInput input)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.FindUsers(input), dataPager.SetList);
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await GetFindUsers(input);
        }
    }
}
