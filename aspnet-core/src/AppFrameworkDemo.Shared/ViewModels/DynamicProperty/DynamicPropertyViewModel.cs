using Abp.Application.Services.Dto;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto; 
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Models;
using AppFramework.Common.Core;
using AppFramework.Common;

namespace AppFramework.Shared.ViewModels
{
    public class DynamicPropertyViewModel : RegionCurdViewModel<DynamicPropertyModel>
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService, IMessenger messenger)
        {
            this.appService=appService;
            messenger.Sub(AppMessengerKeys.Dynamic, async () => await RefreshAsync());
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(
                       () => appService.GetAll(),
                       result => RefreshSuccessed(result));
            });
        }

        public override async void Delete(DynamicPropertyModel selectedItem)
        {
            if (selectedItem==null) return;

            if (!await dialogService.DeleteConfirm()) return;

            await appService.Delete(new EntityDto(selectedItem.Id));
            await RefreshAsync();
        }

        private async Task RefreshSuccessed(ListResultDto<DynamicPropertyDto> result)
        {
            GridModelList.Clear();

            foreach (var item in Map<List<DynamicPropertyModel>>(result.Items))
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}