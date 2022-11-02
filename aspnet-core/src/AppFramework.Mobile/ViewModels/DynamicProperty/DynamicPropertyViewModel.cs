using Abp.Application.Services.Dto;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using System.Threading.Tasks; 
using AppFramework.Shared.Services.Permission; 

namespace AppFramework.Shared.ViewModels
{
    public class DynamicPropertyViewModel : NavigationCurdViewModel
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyDto SelectedItem => Map<DynamicPropertyDto>(dataPager.SelectedItem);

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAll(), async result =>
                {
                    dataPager.SetList(new PagedResultDto<DynamicPropertyDto>()
                    {
                        Items = result.Items
                    });

                    await Task.CompletedTask;
                });
            });
        }

        public async void Delete()
        {
            if (!await dialogService.DeleteConfirm()) return;

            await appService.Delete(new EntityDto()
            {
                 Id= SelectedItem.Id
            });
            await RefreshAsync();
        }

        protected override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.LanguageEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.LanguageDelete, Local.Localize("Delete"),Delete),
            };
        }
    }
}