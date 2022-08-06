using AppFramework.Common;
using AppFramework.Models.Auditlogs;
using AppFramework.Update;
using AppFramework.Update.Dtos;
using AppFramework.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class VersionManagerViewModel : NavigationCurdViewModel
    {
        private readonly IAbpVersionsAppService appService;
        public GetAllAbpVersionsInput input;

        public VersionManagerViewModel(IAbpVersionsAppService appService)
        {
            Title = Local.Localize("VersionManager");
            this.appService = appService;
            input = new GetAllAbpVersionsInput()
            { 
                MaxResultCount = 10
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;
        }

        private void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {

        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await GetVersions(input);
            });
        }

        private async Task GetVersions(GetAllAbpVersionsInput filter)
        {
            await WebRequest.Execute(() => appService.GetAll(filter),
                         async result =>
                         {
                             dataPager.SetList(result);
                             await Task.CompletedTask;
                         });
        }
    }
}
