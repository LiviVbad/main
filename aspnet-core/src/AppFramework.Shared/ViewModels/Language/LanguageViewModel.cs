using Abp.Application.Services.Dto;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using Prism.Regions.Navigation;
using System.Threading.Tasks;
using AppFramework.Common.Core;
using AppFramework.Common;

namespace AppFramework.Shared.ViewModels
{
    public class LanguageViewModel : RegionCurdViewModel
    {
        private readonly ILanguageAppService appService;

        public LanguageViewModel(ILanguageAppService languageAppService, IMessenger messenger)
        {
            this.appService = languageAppService;
            messenger.Sub(AppMessengerKeys.Language, async () => await RefreshAsync());
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(() => appService.GetLanguages(), RefreshSuccessed);
            });
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is ApplicationLanguageListDto item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.DeleteLanguage(new EntityDto()
                {
                    Id = item.Id
                });
                await RefreshAsync();
            }
        }

        protected virtual async Task RefreshSuccessed(GetLanguagesOutput result)
        {
            GridModelList.Clear();

            foreach (var item in result.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}