using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.DynamicEntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class DynamicPropertyViewModel : NavigationCurdViewModel<DynamicPropertyModel>
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAll(),
                       async result =>
                       {
                           GridModelList.Clear();

                           foreach (var item in Map<List<DynamicPropertyModel>>(result.Items))
                               GridModelList.Add(item);

                           await Task.CompletedTask;
                       });
            });
        }

        public override PermissionButton[] CreatePermissionButtons()
        {
            return new PermissionButton[]
             {
                new PermissionButton(PermissionKey.LanguageEdit, Local.Localize("Change")),
                new PermissionButton(PermissionKey.LanguageDelete, Local.Localize("Delete")),
                new PermissionButton(PermissionKey.LanguageEdit, Local.Localize("EditValues")),
             };
        }
    }
}
