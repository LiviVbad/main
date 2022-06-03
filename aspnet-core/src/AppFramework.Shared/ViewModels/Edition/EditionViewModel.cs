using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Core;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class EditionViewModel : RegionCurdViewModel
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
                await WebRequestRuner.Execute(() => appService.GetEditions(), RefreshSuccessed);
            });
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is EditionListDto item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.DeleteEdition(new EntityDto()
                {
                    Id = item.Id
                });
                await RefreshAsync();
            }
        }

        private async Task RefreshSuccessed(ListResultDto<EditionListDto> result)
        {
            GridModelList.Clear();

            foreach (var item in result.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}