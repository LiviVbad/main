﻿using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Models;
using AppFramework.Shared.Services.Permission;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using AppFramework.Services; 
using Prism.Regions;
using System.Threading.Tasks; 

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : NavigationCurdViewModel
    {
        private readonly ILanguageAppService appService;
        private readonly NavigationService navigationService; 
        public LanguageListModel SelectedItem => Map<LanguageListModel>(dataPager.SelectedItem);

        public LanguageViewModel(ILanguageAppService languageAppService, NavigationService  navigationService)
        {
            Title = Local.Localize("Languages");
            this.appService = languageAppService;
            this.navigationService = navigationService; 
        }

        /// <summary>
        /// 更改文本
        /// </summary>
        private void ChangeTexts()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Name", SelectedItem.Name);

            navigationService.Navigate(AppViews.LanguageText, param);
        }

        /// <summary>
        /// 设置默认语言
        /// </summary>
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

        /// <summary>
        /// 删除语言
        /// </summary>
        private async void Delete()
        {
            if (await dialog.Question(Local.Localize("LanguageDeleteWarningMessage", SelectedItem.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteLanguage(
                        new EntityDto(SelectedItem.Id)),
                        async()=> await OnNavigatedToAsync());
                });
            }
        }

        /// <summary>
        /// 获取语言列表
        /// </summary>
        /// <returns></returns>
        private async Task GetLanguages()
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

        /// <summary>
        /// 刷新语言列表模块
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(GetLanguages); 
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.LanguageEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.LanguageChangeTexts, Local.Localize("ChangeTexts"),ChangeTexts),
                new PermissionItem(AppPermissions.Languages, Local.Localize("SetAsDefaultLanguage"),SetAsDefaultLanguage),
                new PermissionItem(AppPermissions.LanguageDelete, Local.Localize("Delete"),Delete)
            };
        }

    }
}