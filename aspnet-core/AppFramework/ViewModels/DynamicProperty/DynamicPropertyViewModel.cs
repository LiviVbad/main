using AppFramework.Common;
using AppFramework.Common.Models;
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
    }
}
