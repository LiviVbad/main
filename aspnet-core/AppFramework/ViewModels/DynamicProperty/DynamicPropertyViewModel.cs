using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
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
                await WebRequest.Execute(
                       () => appService.GetAll(),
                       result => RefreshSuccessed(result));
            });
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
