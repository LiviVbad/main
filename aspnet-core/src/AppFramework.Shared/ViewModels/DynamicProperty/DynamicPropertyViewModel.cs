using Abp.Application.Services.Dto;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using System.Threading.Tasks;
using AppFramework.Common.Core;
using AppFramework.Common;

namespace AppFramework.Shared.ViewModels
{
    public class DynamicPropertyViewModel : RegionCurdViewModel
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService, IMessenger messenger)
        {
            this.appService = appService;
            messenger.Sub(AppMessengerKeys.Dynamic, async () => await RefreshAsync());
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(() => appService.GetAll(), RefreshSuccessed);
            });
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is DynamicPropertyDto item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.Delete(new EntityDto(item.Id));
                await RefreshAsync();
            } 
        }

        private async Task RefreshSuccessed(ListResultDto<DynamicPropertyDto> result)
        {
            GridModelList.Clear();

            foreach (var item in result.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}