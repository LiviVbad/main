using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users.Delegation;
using AppFramework.Authorization.Users.Delegation.Dto;
using AppFramework.Shared;
using AppFramework.Services;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using AppFramework.Shared.Services; 

namespace AppFramework.ViewModels
{
    public class ManageUserDelegationsViewModel : HostDialogViewModel
    {
        private readonly IUserDelegationAppService appService;
        private readonly IHostDialogService dialog;

        public IDataPagerService dataPager { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand<UserDelegationDto> DeleteCommand { get; private set; }

        public GetUserDelegationsInput input;

        public ManageUserDelegationsViewModel(
            IUserDelegationAppService appService,
            IDataPagerService dataPager,
            IHostDialogService dialog)
        {
            this.appService = appService;
            this.dataPager = dataPager;
            this.dialog = dialog;
            AddCommand = new DelegateCommand(Add);
            DeleteCommand = new DelegateCommand<UserDelegationDto>(Delete);

            input = new GetUserDelegationsInput()
            {
                MaxResultCount = 10,
            };
        }

        private async void Delete(UserDelegationDto obj)
        {
            if (await dialog.Question(Local.Localize("UserDelegationDeleteWarningMessage", obj.Username), "ManageUserDelegationsView"))
            {
                await WebRequest.Execute(() => appService.RemoveDelegation(new EntityDto<long>
                    (obj.Id)), GetDelegatedUsers);
            }
        }

        private async void Add()
        {
            var dialogResilt = await dialog.ShowDialogAsync(AppViews.ManageNewUser, null, "ManageUserDelegationsView");
            if (dialogResilt.Result == ButtonResult.OK)
            {
                await GetDelegatedUsers();
            }
        }

        private async Task GetDelegatedUsers()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                        appService.GetDelegatedUsers(input),
                        GetDelegatedUsersSuccessed);
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await GetDelegatedUsers();
        }

        private async Task GetDelegatedUsersSuccessed(PagedResultDto<UserDelegationDto> output)
        {
            dataPager.SetList(output);

            await Task.CompletedTask;
        }
    }
}
