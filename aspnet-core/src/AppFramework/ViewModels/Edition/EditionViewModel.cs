using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.Editions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class EditionViewModel : NavigationCurdViewModel
    {
        private readonly IEditionAppService appService;

        public EditionViewModel(IEditionAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetEditions(),
                        async result =>
                        {
                            GridModelList.Clear();

                            foreach (var item in Map<List<EditionListModel>>(result.Items))
                                GridModelList.Add(item);

                            await Task.CompletedTask;
                        });
            });
        }

        public override PermButton[] GeneratePermissionButtons()
        {
            return new PermButton[]
            {
                new PermButton(Permkeys.EditionEdit, Local.Localize("EditEdition"),()=>Edit()),
                new PermButton(Permkeys.EditionDelete, Local.Localize("Delete"),()=>Delete()),
            };
        }

        private async void Delete()
        {
            if (SelectedItem is EditionListModel item)
            {
                if (await dialog.Question(Local.Localize("EditionDeleteWarningMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() =>
                                appService.DeleteEdition(new EntityDto(item.Id)),
                            RefreshAsync);
                    });
                }
            }
        }
    }
}
