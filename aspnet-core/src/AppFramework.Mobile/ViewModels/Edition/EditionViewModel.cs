using Abp.Application.Services.Dto; 
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System.Threading.Tasks;
using AppFramework.Shared.Services.Permission;

namespace AppFramework.Shared.ViewModels
{
    public class EditionViewModel : NavigationCurdViewModel
    {
        private readonly IEditionAppService appService;

        public EditionListDto SelectedItem => Map<EditionListDto>(dataPager.SelectedItem);

        public EditionViewModel(IEditionAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetEditions(), async result =>
                {
                    dataPager.SetList(new PagedResultDto<EditionListDto>()
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

            await appService.DeleteEdition(new EntityDto()
            {
                 Id= SelectedItem.Id
            });
            await RefreshAsync();
        }

        protected override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.EditionEdit, Local.Localize("EditEdition"),Edit),
                new PermissionItem(AppPermissions.EditionDelete, Local.Localize("Delete"),Delete),
            };
        }
    }
}