using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class DynamicPropertyViewModel : NavigationCurdViewModel
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// 删除动态属性
        /// </summary>
        private async void Delete()
        {
            if (dataPager.SelectedItem is DynamicPropertyModel item)
            {
                if (await dialog.Question(Local.Localize("DeleteDynamicPropertyMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() => appService.Delete(
                            new EntityDto(item.Id)),
                            RefreshAsync);
                    });
                }
            }
        }

        /// <summary>
        /// 编辑值
        /// </summary>
        private async void EditValues()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", dataPager.SelectedItem);

            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.DynamicEditValues, param);
        }

        /// <summary>
        /// 获取全部动态属性
        /// </summary>
        /// <returns></returns>
        private async Task GetDynamicPropertyAll()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAll(),
                       async result =>
                       {
                           dataPager.SetList(new PagedResultDto<DynamicPropertyDto>()
                           {
                               Items = result.Items
                           });
                           await Task.CompletedTask;
                       });
            });
        }

        /// <summary>
        /// 刷新动态属性模块
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await GetDynamicPropertyAll();
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
             {
                new PermissionItem(Permkeys.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionItem(Permkeys.LanguageDelete, Local.Localize("Delete"),()=>Delete()),
                new PermissionItem(Permkeys.LanguageChangeTexts, Local.Localize("EditValues"),()=>EditValues()),
             };
        }
    }
}
