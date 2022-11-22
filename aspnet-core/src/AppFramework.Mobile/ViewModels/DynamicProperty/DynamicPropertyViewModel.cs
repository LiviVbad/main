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
                await WebRequest.Execute(() => appService.GetAll(), dataPager.SetList);
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
    }
}