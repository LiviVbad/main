using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class EditionViewModel : NavigationCurdViewModel<EditionListModel>
    {
        private readonly IEditionAppService appService;

        public EditionViewModel(IEditionAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetEditions(),
                        async result =>
                        {
                            GridModelList.Clear();

                            foreach (var item in Map<List<EditionListModel>>(result.Items))
                                GridModelList.Add(item);

                            await Task.CompletedTask;
                        });
            });
        }
    }
}
