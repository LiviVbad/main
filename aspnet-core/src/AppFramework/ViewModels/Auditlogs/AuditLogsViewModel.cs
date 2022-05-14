using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
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

        public GetAuditLogsInput input;
        private readonly IAuditLogAppService appService;

        private string filterTitle;

        public string FilerTitle
        {
            get { return filterTitle; }
            set { filterTitle = value; RaisePropertyChanged(); }
        }

        private bool isAdvancedFilter;

        public bool IsAdvancedFilter
        {
            get { return isAdvancedFilter; }
            set
            {
                isAdvancedFilter = value;

                FilerTitle = value ? Local.Localize("HideAdvancedFilters") : Local.Localize("ShowAdvancedFilters");
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ViewCommand { get; private set; }
        public DelegateCommand AdvancedCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }

        #endregion

        public AuditLogsViewModel(IAuditLogAppService appService)
        {
            input = new GetAuditLogsInput()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize,
            };
            this.appService = appService;

            IsAdvancedFilter = false;
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
            input.SkipCount = 0;
            GridModelList.Clear();

            await RefreshAsync();
        }

        public override async Task RefreshAsync()
        {
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