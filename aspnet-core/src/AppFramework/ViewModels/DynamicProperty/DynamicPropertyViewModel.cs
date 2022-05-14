using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.DynamicEntityProperties;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class DynamicPropertyViewModel : NavigationCurdViewModel<DynamicPropertyModel>
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
                           GridModelList.Clear();

                           foreach (var item in Map<List<DynamicPropertyModel>>(result.Items))
                               GridModelList.Add(item);

                           await Task.CompletedTask;
                       });
            });
        }

        public override PermButton[] GeneratePermissionButtons()
        {
            return new PermButton[]
             {
                new PermButton(Permkeys.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermButton(Permkeys.LanguageDelete, Local.Localize("Delete"),()=>Delete()),
                new PermButton(Permkeys.LanguageEdit, Local.Localize("EditValues"),()=>EditValues()),
             };
        }

        private async void Delete()
        {
            if (await dialog.Question(Local.Localize("DeleteDynamicPropertyMessage", SelectedItem.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.Delete(
                        new EntityDto(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
        }

        private void EditValues() { }
    }
}
