using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : NavigationCurdViewModel<LanguageListModel>
    {
        private readonly ILanguageAppService appService;

        public LanguageViewModel(ILanguageAppService languageAppService)
        {
            this.appService = languageAppService;
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
         
        public override PermissionButton[] CreatePermissionButtons()
        {
            return new PermissionButton[]
            {
                new PermissionButton(PermissionKey.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionButton(PermissionKey.LanguageChangeTexts, Local.Localize("ChangeTexts"),()=>ChangeTexts()),
                new PermissionButton(PermissionKey.Languages, Local.Localize("SetAsDefaultLanguage"),()=>SetAsDefaultLanguage()),
                new PermissionButton(PermissionKey.LanguageDelete, Local.Localize("Delete"),()=>Delete())
            };
        }

        private void ChangeTexts() { }

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