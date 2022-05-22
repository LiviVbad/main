using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using Prism.Regions;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : NavigationCurdViewModel
    {
        private readonly ILanguageAppService appService;
        private readonly IRegionManager regionManager;

        public LanguageListModel SelectedItem => Map<LanguageListModel>(dataPager.SelectedItem);

        public LanguageViewModel(ILanguageAppService languageAppService, IRegionManager regionManager)
        {
            this.appService = languageAppService;
            this.regionManager = regionManager;
        }

        public override async Task RefreshAsync()
        {
            await WebRequest.Execute(() => appService.GetLanguages(),
                      async result =>
                      {
                          dataPager.SetList(new PagedResultDto<ApplicationLanguageListDto>()
                          {
                              Items = result.Items
                          });
                          await Task.CompletedTask;
                      });
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(Permkeys.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionItem(Permkeys.LanguageChangeTexts, Local.Localize("ChangeTexts"),()=>ChangeTexts()),
                new PermissionItem(Permkeys.Languages, Local.Localize("SetAsDefaultLanguage"),()=>SetAsDefaultLanguage()),
                new PermissionItem(Permkeys.LanguageDelete, Local.Localize("Delete"),()=>Delete())
            };
        }

        private void ChangeTexts()
        { 
            NavigationParameters param = new NavigationParameters();
            param.Add("Name", SelectedItem.Name);

            regionManager
                .Regions[AppRegionManager.Main]
                .RequestNavigate(AppViewManager.LanguageChengedText, param);
        }

        private async void SetAsDefaultLanguage()
        { 
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                appService.SetDefaultLanguage(new Localization.Dto.SetDefaultLanguageInput()
                {
                    Name = SelectedItem.Name
                }));
            });
        }

        private async void Delete()
        { 
            if (await dialog.Question(Local.Localize("LanguageDeleteWarningMessage", SelectedItem.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteLanguage(
                        new EntityDto(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
        }
    }
}