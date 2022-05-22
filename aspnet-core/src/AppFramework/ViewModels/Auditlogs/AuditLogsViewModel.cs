using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Common;
using AppFramework.Models.Auditlogs;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using Prism.Ioc;

namespace AppFramework.ViewModels
{
    public class AuditLogsViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        public IDataPagerService logsdataPager { get; private set; }

        private readonly IAuditLogAppService appService;
        private string filterTitle = string.Empty;
        private bool isAdvancedFilter;
        private GetAuditLogsFilter filter;
        private GetEntityChangeFilter entityChangeFilter;

        private int selectedIndex;

        /// <summary>
        /// 错误状态选项: 全部/错误
        /// </summary>
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

        /// <summary>
        /// 当前筛选标题 [展开/收缩]
        /// </summary>
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

        /// <summary>
        /// 审计日志筛选条件
        /// </summary>
        public GetAuditLogsFilter Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 更改日志筛选条件
        /// </summary>
        public GetEntityChangeFilter EntityChangeFilter
        {
            get { return entityChangeFilter; }
            set { entityChangeFilter = value; RaisePropertyChanged(); }
        }

        //查看日志、查看更改日志、高级筛选、搜索、搜索更改日志
        public DelegateCommand ViewLogCommand { get; private set; }
        public DelegateCommand ViewChangedLogCommand { get; private set; }
        public DelegateCommand AdvancedCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand SearchChangedCommand { get; private set; }

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
            entityChangeFilter = new GetEntityChangeFilter()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize
            };
            this.appService = appService;

            SearchCommand = new DelegateCommand(Search);
            SearchChangedCommand = new DelegateCommand(SearchChanged);
            ViewLogCommand = new DelegateCommand(ViewLog);
            ViewChangedLogCommand = new DelegateCommand(ViewChangedLog);
            AdvancedCommand = new DelegateCommand(() => { IsAdvancedFilter = !IsAdvancedFilter; });

            logsdataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
        }

        /// <summary>
        /// 查看更改日志详情
        /// </summary>
        private void ViewChangedLog()
        { }

        /// <summary>
        /// 查看操作日志详情
        /// </summary>
        private void ViewLog()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", dataPager.SelectedItem);
            dialog.ShowDialogAsync(AppViewManager.AuditLogDetails, param);
        }

        /// <summary>
        /// 搜索操作日志
        /// </summary>
        private async void Search()
        {
            filter.SkipCount = 0;

            await SetBusyAsync(async () =>
            {
                await GetAuditLogs();
            });
        }

        /// <summary>
        /// 获取审计日期数据
        /// </summary>
        /// <returns></returns>
        private async Task GetAuditLogs()
        {
            var input = Map<GetAuditLogsInput>(filter);

            await WebRequest.Execute(() => appService.GetAuditLogs(input),
                         async result =>
                         {
                             dataPager.SetList(result);
                             await Task.CompletedTask;
                         });
        }

        /// <summary>
        /// 搜索更改日志
        /// </summary>
        private async void SearchChanged()
        {
            entityChangeFilter.SkipCount = 0;

            await GetEntityChanges();
        }

        /// <summary>
        /// 获取更改日志
        /// </summary>
        /// <returns></returns>
        private async Task GetEntityChanges()
        {
            var input = Map<GetEntityChangeInput>(entityChangeFilter);

            await WebRequest.Execute(() => appService.GetEntityChanges(input),
                           async result =>
                           {
                               logsdataPager.SetList(result);
                               await Task.CompletedTask;
                           });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await GetAuditLogs();
                await GetEntityChanges();
            });
        }
    }
}