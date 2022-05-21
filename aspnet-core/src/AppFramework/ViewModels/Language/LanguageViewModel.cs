using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.Localization;
using Prism.Regions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : NavigationCurdViewModel
    {
        private readonly ILanguageAppService appService;
        private readonly IRegionManager regionManager;

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
                          GridModelList.Clear();

                          foreach (var item in Map<List<LanguageListModel>>(result.Items))
                              GridModelList.Add(item);

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
            if (SelectedItem is LanguageListModel item)
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("Name", item.Name);

                regionManager
                    .Regions[AppRegionManager.Main]
                    .RequestNavigate(AppViewManager.LanguageChengedText, param);
            }
        }

        private async void SetAsDefaultLanguage()
        {
            if (SelectedItem is LanguageListModel item)
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() =>
                    appService.SetDefaultLanguage(new Localization.Dto.SetDefaultLanguageInput()
                    {
                        Name = item.Name
                    }));
                });
            }
        }

        private async void Delete()
        {
            if (SelectedItem is LanguageListModel item)
            {
                if (await dialog.Question(Local.Localize("LanguageDeleteWarningMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() => appService.DeleteLanguage(
                            new EntityDto(item.Id)),
                            RefreshAsync);
                    });
                }
            }
        }
    }
}