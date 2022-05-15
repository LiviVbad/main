using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Models.Auditlogs;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class AuditLogsViewModel : NavigationCurdViewModel<AuditLogListModel>
    {
        #region 字段/属性

        private readonly IAuditLogAppService appService;
        private string filterTitle = string.Empty;
        private bool isAdvancedFilter;
        private GetAuditLogsFilter filter;

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;

                if (selectedIndex == 0)
                    filter.HasException = null;
                else if (selectedIndex == 1)
                    filter.HasException = false;
                else
                    filter.HasException = true; 
                RaisePropertyChanged();
            }
        }

        public string FilerTitle
        {
            get { return filterTitle; }
            set { filterTitle = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 高级筛选
        /// </summary>
        public bool IsAdvancedFilter
        {
            get { return isAdvancedFilter; }
            set
            {
                isAdvancedFilter = value;

                FilerTitle = value ? "△ " + Local.Localize("HideAdvancedFilters") : "▽ " + Local.Localize("ShowAdvancedFilters");
                RaisePropertyChanged();
            }
        }

        public GetAuditLogsFilter Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        public DelegateCommand ViewCommand { get; private set; }
        public DelegateCommand AdvancedCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        #endregion

        public AuditLogsViewModel(IAuditLogAppService appService)
        {
            IsAdvancedFilter = false;
            filter = new GetAuditLogsFilter()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize
            };
            this.appService = appService;
            ViewCommand = new DelegateCommand(ViewLogs);
            AdvancedCommand = new DelegateCommand(() =>
              {
                  IsAdvancedFilter = !IsAdvancedFilter;
              });
            SearchCommand = new DelegateCommand(Search);
        }

        private void ViewLogs()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", SelectedItem);
            dialog.ShowDialogAsync(AppViewManager.AuditLogDetails, param);
        }

        private async void Search()
        {
            filter.SkipCount = 0;
            GridModelList.Clear();

            await RefreshAsync();
        }

        public override async Task RefreshAsync()
        {
            var input = Map<GetAuditLogsInput>(filter);

            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAuditLogs(input),
                           async result =>
                           {
                               foreach (var item in Map<List<AuditLogListModel>>(result.Items))
                                   GridModelList.Add(item);

                               await Task.CompletedTask;
                           });
            });
        }
    }
}