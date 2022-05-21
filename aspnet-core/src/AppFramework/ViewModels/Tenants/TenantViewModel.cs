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
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class TenantViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        private readonly ITenantAppService appService;
        private readonly IEditionAppService editionAppService;

        private bool isSubscription;
        private bool isCreation;
        private GetTenantsFilter filter;
        private EditionListModel edition;
        private ObservableCollection<EditionListModel> editions;

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
         
        public GetTenantsFilter Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }
         
        public EditionListModel Edition
        {
            get { return edition; }
            set { edition = value; RaisePropertyChanged(); }
        }
         
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
            if (Editions.Count > 0) return;

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
        private async void TenantChangeFeatures()
        {
            if (SelectedItem is TenantListModel item)
            {
                GetTenantFeaturesEditOutput output = null;
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.GetTenantFeaturesForEdit(new EntityDto(item.Id)),
                          async result =>
                          {
                              output = result;
                              await Task.CompletedTask;
                          });

                });

                if (output == null) return;

                DialogParameters param = new DialogParameters();
                param.Add("Id", item.Id);
                param.Add("Value", output);

                await dialog.ShowDialogAsync(AppViewManager.TenantChangeFeatures, param);
            }
        }

        private void TenantImpersonation()
        {
            //..使用当前租户登录
        }

        private async void Unlock()
        {
            if (SelectedItem is TenantListModel item)
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.UnlockTenantAdmin(
                        new EntityDto(item.Id)), RefreshAsync);
                });
            }
        }

        private async void Delete()
        {
            if (SelectedItem is TenantListModel item)
            {
                var result = await dialog.Question(Local.Localize("TenantDeleteWarningMessage", item.TenancyName));
                if (result)
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() => appService.DeleteTenant(
                            new EntityDto(item.Id)), RefreshAsync);
                    });
                }
            }
        }

        #endregion

        public override async Task RefreshAsync()
        {
            var input = Map<GetTenantsInput>(filter);

            await SetBusyAsync(async () =>
            {
                await GetAllEditions();

                await WebRequest.Execute(() => appService.GetTenants(input),
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
                new PermButton(Permkeys.TenantChangeFeatures, Local.Localize("Features"),()=>TenantChangeFeatures()),
                new PermButton(Permkeys.TenantDelete, Local.Localize("Delete"),()=>Delete()),
                new PermButton(Permkeys.TenantUnlock, Local.Localize("Unlock"),()=>Unlock())
             };
        }
    }
}
