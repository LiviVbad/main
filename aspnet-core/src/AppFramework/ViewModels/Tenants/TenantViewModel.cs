using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.Editions;
using AppFramework.Models.Tenants;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using Prism.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Prism.Regions;

namespace AppFramework.ViewModels
{
    public class TenantViewModel : NavigationCurdViewModel<TenantListModel>
    {
        #region 字段/属性

        private readonly ITenantAppService appService;
        private readonly IEditionAppService editionAppService;

        private bool isSubscription;

        /// <summary>
        /// 启用订阅日期查询
        /// </summary>
        public bool IsSubscription
        {
            get { return isSubscription; }
            set
            {
                isSubscription = value;
                if (value)
                {
                    Filter.SubscriptionEndDateStart = null;
                    Filter.SubscriptionEndDateEnd = null;
                }
                RaisePropertyChanged();
            }
        }

        private bool isCreation;

        /// <summary>
        /// 启用创建时间查询
        /// </summary>
        public bool IsCreation
        {
            get { return isCreation; }
            set
            {
                isCreation = value;
                if (value)
                {
                    Filter.CreationDateStart = null;
                    Filter.CreationDateEnd = null;
                }
                RaisePropertyChanged();
            }
        }

        private GetTenantsFilter filter;
        public GetTenantsFilter Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        private EditionListModel edition;

        public EditionListModel Edition
        {
            get { return edition; }
            set { edition = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<EditionListModel> editions;

        public ObservableCollection<EditionListModel> Editions
        {
            get { return editions; }
            set { editions = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SearchCommand { get; private set; }

        #endregion

        public TenantViewModel(ITenantAppService appService, IEditionAppService editionAppService)
        {
            filter = new GetTenantsFilter()
            {
                EditionIdSpecified = false,
                MaxResultCount = 10,
                SkipCount = 0,
            };

            this.appService = appService;
            this.editionAppService = editionAppService;
            SearchCommand = new DelegateCommand(Search);

            editions = new ObservableCollection<EditionListModel>();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        private async void Search()
        {
            await RefreshAsync();
        }

        /// <summary>
        /// 获取筛选版本列表
        /// </summary>
        /// <returns></returns>
        public async Task GetAllEditions()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => editionAppService.GetEditions(),
                        async result =>
                        {
                            Editions.Clear();

                            foreach (var item in Map<List<EditionListModel>>(result.Items))
                                Editions.Add(item);

                            await Task.CompletedTask;
                        });
            });
        }

        #region 修改租户/使用当前租户登录/解锁/删除

        /// <summary>
        /// 修改租户功能
        /// </summary>
        /// <param name="Id"></param>
        private async void TenantChangeFeatures(int Id)
        {
            await dialog.ShowDialogAsync(AppViewManager.TenantChangeFeatures);
        }

        private void TenantImpersonation()
        {
            //..使用当前租户登录
        }

        private async void Unlock()
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(() => appService.UnlockTenantAdmin(
                     new EntityDto(SelectedItem.Id)), RefreshAsync);
             });
        }

        private async void Delete()
        {
            var result = await dialog.Question(Local.Localize("TenantDeleteWarningMessage", SelectedItem.TenancyName));
            if (result)
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteTenant(
                        new EntityDto(SelectedItem.Id)), RefreshAsync);
                });
            }
        }

        #endregion

        public override async Task RefreshAsync()
        {
            var input = Map<GetTenantsInput>(filter);

            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(
                      () => appService.GetTenants(input),
                      async result =>
                      {
                          GridModelList.Clear();

                          foreach (var item in Map<List<TenantListModel>>(result.Items))
                              GridModelList.Add(item);

                          await Task.CompletedTask;
                      });
            });
        }

        public override PermButton[] GeneratePermissionButtons()
        {
            return new PermButton[]
             {
                new PermButton(Permkeys.TenantImpersonation, Local.Localize("LoginAsThisTenant"),()=>TenantImpersonation()),
                new PermButton(Permkeys.TenantEdit, Local.Localize("Change"),()=>Edit()),
                new PermButton(Permkeys.TenantChangeFeatures, Local.Localize("Features"),()=>TenantChangeFeatures(SelectedItem.Id)),
                new PermButton(Permkeys.TenantDelete, Local.Localize("Delete"),()=>Delete()),
                new PermButton(Permkeys.TenantUnlock, Local.Localize("Unlock"),()=>Unlock())
             };
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await GetAllEditions();
            await base.OnNavigatedToAsync(navigationContext);
        }
    }
}
