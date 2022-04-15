using Abp.Application.Services.Dto;
using AppFramework.Common.Core;
using AppFramework.Common.Models;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class EditionViewModel : NavigationCurdViewModel<EditionListModel>
    {
        private readonly IEditionAppService appService;

        public EditionViewModel(IEditionAppService appService, IMessenger messenger)
        {
            this.appService = appService;
            messenger.Sub(AppMessengerKeys.Edition, async () => await RefreshAsync());
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(
                        () => appService.GetEditions(),
                        result => RefreshSuccessed(result));
            });
        }

        public override async void Delete(EditionListModel selectedItem)
        {
            if (selectedItem == null) return;

            if (!await dialogService.DeleteConfirm()) return;

            await appService.DeleteEdition(new EntityDto()
            {
                Id = selectedItem.Id
            });
            await RefreshAsync();
        }

        private async Task RefreshSuccessed(ListResultDto<EditionListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<EditionListModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}