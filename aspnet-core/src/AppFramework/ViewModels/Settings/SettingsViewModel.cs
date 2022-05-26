using AppFramework.Common;
using AppFramework.Common.Models.Configuration;
using AppFramework.Configuration.Host;
using AppFramework.Configuration.Host.Dto;
using AppFramework.Editions.Dto;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class SettingsViewModel : NavigationViewModel
    {
        private readonly IHostSettingsAppService appService;
        private readonly ICommonLookupAppService lookupAppService;

        public DelegateCommand SaveCommand { get; private set; }

        private HostSettingsEditModel setting;

        public HostSettingsEditModel Setting
        {
            get { return setting; }
            set { setting = value; RaisePropertyChanged(); }
        }

        private SubscribableEditionComboboxItemDto selectedEdition;

        /// <summary>
        /// 选中版本
        /// </summary>
        public SubscribableEditionComboboxItemDto SelectedEdition
        {
            get { return selectedEdition; }
            set
            {
                selectedEdition = value;
                if (selectedEdition != null)
                    setting.TenantManagement.DefaultEditionId = Convert.ToInt32(value.Value);
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<SubscribableEditionComboboxItemDto> editions;

        /// <summary>
        /// 版本列表
        /// </summary>
        public ObservableCollection<SubscribableEditionComboboxItemDto> Editions
        {
            get => editions;
            set
            {
                editions = value;
                RaisePropertyChanged();
            }
        }

        public SettingsViewModel(IHostSettingsAppService appService,
            ICommonLookupAppService lookupAppService)
        {
            SaveCommand = new DelegateCommand(Save);
            this.appService = appService;
            this.lookupAppService = lookupAppService;

            editions = new ObservableCollection<SubscribableEditionComboboxItemDto>();
        }

        private async void Save()
        {
            //验证输入合法性...

            var input = Map<HostSettingsEditDto>(Setting);
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(() => appService.UpdateAllSettings(input));
             });
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

        private async Task GetEditions()
        {
            await WebRequest.Execute(() => lookupAppService.GetEditionsForCombobox(),
               async result =>
               {
                   foreach (var item in result.Items)
                       Editions.Add(item);

                   await Task.CompletedTask;
               });
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await SetBusyAsync(async () =>
            {
                await GetSettings();
                await GetEditions();
            });
        }
    }
}
