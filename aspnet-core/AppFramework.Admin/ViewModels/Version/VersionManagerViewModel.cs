using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Models;
using AppFramework.Shared.Services.Permission; 
using AppFramework.Version;
using AppFramework.Version.Dtos; 
using Prism.Regions; 
using System.Threading.Tasks;
using AppFramework.Shared.Services;

namespace AppFramework.ViewModels
{
    public class VersionManagerViewModel : NavigationCurdViewModel
    {
        private readonly IAbpVersionsAppService appService;
        public GetAllAbpVersionsInput input;
        public VersionListModel SelectedItem => Map<VersionListModel>(dataPager.SelectedItem);

        public VersionManagerViewModel(IAbpVersionsAppService appService)
        {
            Title = Local.Localize("VersionManager");
            this.appService = appService;
            input = new GetAllAbpVersionsInput()
            {
                MaxResultCount = 10
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;
        }

        private void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {

        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetVersions(input);
            });
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.Pages_AbpVersions_Edit,Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.Pages_AbpVersions_Delete, Local.Localize("Delete"),Delete),
            };
        }

        private async Task GetVersions(GetAllAbpVersionsInput filter)
        {
            await WebRequest.Execute(() => appService.GetAll(filter),
                         async result =>
                         {
                             dataPager.SetList(result);
                             await Task.CompletedTask;
                         });
        }

        private async void Delete()
        {
            var result = await dialog.Question(Local.Localize("VersionDeleteWarningMessage", SelectedItem.Name));
            if (result)
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.Delete(
                        new EntityDto(SelectedItem.Id)), async () => await OnNavigatedToAsync());
                });
            }
        }
    }
}
