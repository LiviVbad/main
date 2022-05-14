﻿using Abp.Runtime.Security;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Editions.Dto;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class TenantDetailsViewModel : HostDialogViewModel
    {
        private readonly ITenantAppService tenantAppService;
        private readonly IPermissionService permissionService;
        private readonly ICommonLookupAppService commonLookupAppService;

        #region 字段/属性

        private const string NotAssignedValue = "0";
        private bool isSubscriptionFieldVisible;
        private bool isUnlimitedTimeSubscription;
        private bool isNewTenant;
        private string pageTitle;
        private bool useHostDatabase;
        private string adminPassword;
        private string adminPasswordRepeat;
        private bool isSetRandomPassword;
        private TenantListModel model;
        private ObservableCollection<SubscribableEditionComboboxItemDto> editions;
        private SubscribableEditionComboboxItemDto selectedEdition;

        public ObservableCollection<SubscribableEditionComboboxItemDto> Editions
        {
            get => editions;
            set
            {
                editions = value;
                RaisePropertyChanged();
            }
        }

        //是否无限时间订阅
        public bool IsUnlimitedTimeSubscription
        {
            get => isUnlimitedTimeSubscription;
            set
            {
                isUnlimitedTimeSubscription = value;

                if (isUnlimitedTimeSubscription)
                {
                    //如果是无时间订阅限制, 将订阅事件设置为null
                    model.SubscriptionEndDateUtc = null;
                }
                else
                {
                    //如果不是无限时间订阅,重置是否到期选项
                    model.IsInTrialPeriod = false;
                }
                RaisePropertyChanged();
            }
        }

        public bool IsNewTenant
        {
            get => isNewTenant;
            set
            {
                isNewTenant = value;
                PageTitle = isNewTenant ? Local.Localize(AppLocalizationKeys.CreatingNewTenant) : Local.Localize(AppLocalizationKeys.EditTenant);
                RaisePropertyChanged();
            }
        }

        public string PageTitle
        {
            get => pageTitle;
            set
            {
                pageTitle = value;
                RaisePropertyChanged();
            }
        }

        //管理员密码
        public string AdminPassword
        {
            get => adminPassword;
            set
            {
                adminPassword = value;
                RaisePropertyChanged();
            }
        }

        //管理员密码重复
        public string AdminPasswordRepeat
        {
            get => adminPasswordRepeat;
            set
            {
                adminPasswordRepeat = value;
                RaisePropertyChanged();
            }
        }

        //使用主机数据库
        public bool UseHostDatabase
        {
            get => useHostDatabase;
            set
            {
                useHostDatabase = value;
                RaisePropertyChanged();
            }
        }

        //是否设置随机密码
        public bool IsSetRandomPassword
        {
            get => isSetRandomPassword;
            set
            {
                isSetRandomPassword = value;
                if (isSetRandomPassword)
                {
                    AdminPassword = null;
                    AdminPasswordRepeat = null;
                }
                RaisePropertyChanged();
            }
        }


        public bool IsSelectedEditionFree
        {
            get
            {
                if (Model == null || SelectedEdition == null)
                    return true;

                if (!Model.EditionId.HasValue)
                    return true;

                if (!SelectedEdition.IsFree.HasValue)
                    return true;

                return SelectedEdition.IsFree.Value;
            }
        }

        public SubscribableEditionComboboxItemDto SelectedEdition
        {
            get => selectedEdition;
            set
            {
                selectedEdition = value;
                UpdateModel();
                IsSubscriptionFieldVisible = SelectedEdition != null && SelectedEdition.Value != NotAssignedValue;
                RaisePropertyChanged("IsSelectedEditionFree");
                RaisePropertyChanged();
            }
        }

        //订阅字段是否可见
        public bool IsSubscriptionFieldVisible
        {
            get => isSubscriptionFieldVisible;
            set
            {
                isSubscriptionFieldVisible = value;
                RaisePropertyChanged();
            }
        }

        public TenantListModel Model
        {
            get => model;
            set { model = value; RaisePropertyChanged(); }
        }

        #endregion

        public TenantDetailsViewModel(
            ITenantAppService tenantAppService,
            ICommonLookupAppService commonLookupAppService,
            IPermissionService permissionService)
        {
            this.tenantAppService = tenantAppService;
            this.permissionService = permissionService;
            this.commonLookupAppService = commonLookupAppService;
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    Model = parameters.GetValue<TenantListModel>("Value");
                    InitializeEditTenant();
                }
                else
                    InitializeNewTenant();

                await PopulateEditionsCombobox(() =>
                {
                    if (Model != null)
                        SetSelectedEdition(Model.EditionId);
                });
            });
        }

        #region 内部方法

        private void InitializeNewTenant()
        {
            IsNewTenant = true;
            Model = new TenantListModel { IsActive = true };

            UseHostDatabase = true;
            IsSetRandomPassword = true;
        }

        private void InitializeEditTenant()
        {
            IsNewTenant = false;
            IsSetRandomPassword = false;
            UseHostDatabase = string.IsNullOrEmpty(Model.ConnectionString);
            if (!string.IsNullOrEmpty(Model.ConnectionString))
                Model.ConnectionString = TryDecryptConnectionString();
        }

        private string TryDecryptConnectionString()
        {
            try
            {
                return SimpleStringCipher.Instance.Decrypt(Model.ConnectionString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void NormalizeTenantUpdateInput(TenantEditDto input)
        {
            input.EditionId = NormalizeEditionId(input.EditionId);
            input.SubscriptionEndDateUtc = NormalizeSubscriptionEndDateUtc(input.SubscriptionEndDateUtc);
        }

        private void NormalizeTenantCreateInput(CreateTenantInput input)
        {
            input.EditionId = NormalizeEditionId(input.EditionId);
            input.SubscriptionEndDateUtc = NormalizeSubscriptionEndDateUtc(input.SubscriptionEndDateUtc);
        }

        private int? NormalizeEditionId(int? editionId)
        {
            return editionId.HasValue && editionId.Value == 0 ? null : editionId;
        }

        private DateTime? NormalizeSubscriptionEndDateUtc(DateTime? subscriptionEndDateUtc)
        {
            if (IsUnlimitedTimeSubscription)
                return null;

            return subscriptionEndDateUtc.Value.AddDays(1).AddTicks(-1);
        }

        private async Task PopulateEditionsCombobox(Action editionsPopulated)
        {
            var editions = await commonLookupAppService.GetEditionsForCombobox();
            Editions = new ObservableCollection<SubscribableEditionComboboxItemDto>(editions.Items);
            AddNotAssignedItem();
            editionsPopulated();
        }

        private void AddNotAssignedItem()
        {
            Editions.Insert(0, new SubscribableEditionComboboxItemDto(NotAssignedValue,
                string.Format("- {0} -", Local.Localize(AppLocalizationKeys.NotAssigned)), null));
        }

        private void SetSelectedEdition(int? editionId)
        {
            SelectedEdition = editionId.HasValue ?
                Editions.Single(e => e.Value == editionId.Value.ToString()) :
                Editions.Single(e => e.Value == NotAssignedValue);
        }

        private void UpdateModel()
        {
            if (SelectedEdition != null &&
                int.TryParse(SelectedEdition.Value, out var selectedEditionId))
                Model.EditionId = selectedEditionId;
            else
                Model.EditionId = null;

            Model.IsInTrialPeriod = !IsSelectedEditionFree;
        }

        #endregion 内部方法
    }
}
