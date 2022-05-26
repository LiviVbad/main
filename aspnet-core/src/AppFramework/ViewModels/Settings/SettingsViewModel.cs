using AppFramework.Common;
using AppFramework.Common.Models.Configuration;
using AppFramework.Configuration.Host;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class SettingsViewModel : NavigationViewModel
    {
        private readonly IHostSettingsAppService appService;

        public DelegateCommand SaveCommand { get; private set; }

        private HostSettingsEditModel setting;

        public HostSettingsEditModel Setting
        {
            get { return setting; }
            set { setting = value; RaisePropertyChanged(); }
        }

        public SettingsViewModel(IHostSettingsAppService appService)
        {
            SaveCommand = new DelegateCommand(Save);
            this.appService = appService;
        }

        private void Save()
        {

        }

        private async Task GetSettings()
        {
            await WebRequest.Execute(() => appService.GetAllSettings(),
                async result =>
                {
                    Setting = Map<HostSettingsEditModel>(result);

                    await Task.CompletedTask;
                });
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await SetBusyAsync(GetSettings);
        }
    }
}
