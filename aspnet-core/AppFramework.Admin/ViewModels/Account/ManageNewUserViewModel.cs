using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users.Delegation;
using AppFramework.Shared;
using AppFramework.Common.Dto;
using AppFramework.Services;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using AppFramework.Common;
using AppFramework.Shared.Services; 

namespace AppFramework.ViewModels
{
    public class ManageNewUserViewModel : HostDialogViewModel
    {
        private readonly ICommonLookupAppService lookupAppService;
        private readonly IHostDialogService dialog;
        private readonly IUserDelegationAppService appService;
        public FindUsersInput input;

        public ManageNewUserViewModel(ICommonLookupAppService lookupAppService,
            IDataPagerService dataPager, IHostDialogService dialog,
            IUserDelegationAppService appService)
        {
            this.lookupAppService = lookupAppService;
            this.dataPager = dataPager;
            this.dialog = dialog;
            this.appService = appService;
            QueryCommand = new DelegateCommand(Query);
            ChooseCommand = new DelegateCommand<NameValueDto>(ChooseUser);
            input = new FindUsersInput()
            {
                MaxResultCount = 10,
                ExcludeCurrentUser = true
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler; ;
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await FindUsers(input);
            });
        }

        public DelegateCommand<NameValueDto> ChooseCommand { get; private set; }
        public DelegateCommand QueryCommand { get; private set; }

        public IDataPagerService dataPager { get; private set; }

        public string Filter
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                RaisePropertyChanged();
            }
        }

        private void Query()
        {
            dataPager.PageIndex = 0;
        }

        private async void ChooseUser(NameValueDto obj)
        {
            var dialogResult = await dialog.ShowDialogAsync(AppViews.SelectDate, null, "ManageNewUser");
            if (dialogResult.Result == ButtonResult.OK)
            {
                var startDate = dialogResult.Parameters.GetValue<DateTime?>("StartDate");
                var endDate = dialogResult.Parameters.GetValue<DateTime?>("EndDate");

                await WebRequest.Execute(() => appService.DelegateNewUser(new Authorization.Users.Delegation.Dto.CreateUserDelegationDto()
                {
                    TargetUserId = Convert.ToInt64(obj.Value),
                    StartTime = startDate.GetFirstDate(),
                    EndTime = endDate.GetLastDate()
                }), async () =>
                {
                    base.Save();
                    await Task.CompletedTask;
                });
            }
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                await FindUsers(input);
            });
        }

        private async Task FindUsers(FindUsersInput input)
        {
            await WebRequest.Execute(() => lookupAppService.FindUsers(input),
                async result =>
                {
                    dataPager.SetList(result);
                    await Task.CompletedTask;
                });
        }
    }
}
