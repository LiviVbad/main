using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class DynamicPropertyViewModel : NavigationCurdViewModel
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAll(),
                       async result =>
                       {
                           dataPager.SetList(new PagedResultDto<DynamicPropertyDto>()
                           {
                               Items = result.Items
                           });

                           await Task.CompletedTask;
                       });
            });
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
             {
                new PermissionItem(Permkeys.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionItem(Permkeys.LanguageDelete, Local.Localize("Delete"),()=>Delete()),
                new PermissionItem(Permkeys.LanguageEdit, Local.Localize("EditValues"),()=>EditValues()),
             };
        }

        private async void Delete()
        {
            if (dataPager.SelectedItem is DynamicPropertyModel item)
            {
                if (await dialog.Question(Local.Localize("DeleteDynamicPropertyMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() => appService.Delete(
                            new EntityDto(item.Id)),
                            RefreshAsync);
                    });
                }
            }
        }

        private void EditValues() { }
    }
}
