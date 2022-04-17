 using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppFramework.Common.Models;
using AppFramework.Common.ViewModels;
using AppFramework.Localization;
using AppFramework.MultiTenancy.HostDashboard;
using AppFramework.MultiTenancy.HostDashboard.Dto;
using Prism.Regions;

namespace AppFramework.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IHostDashboardAppService appService;

        private ObservableCollection<TopStatusItem> topDashBoards;

        public ObservableCollection<DoughnutChartPopulations> ExpenditureData { get; set; }

        public ObservableCollection<AreaSeriesChart3DModel> Performance { get; set; }

        public ObservableCollection<TopStatusItem> TopDashBoards
        {
            get { return topDashBoards; }
            set { topDashBoards = value; RaisePropertyChanged(); }
        }

        private GetRecentTenantsOutput recentTenant;

        public GetRecentTenantsOutput RecentTenant
        {
            get { return recentTenant; }
            set { recentTenant = value; RaisePropertyChanged(); }
        }

        private GetExpiringTenantsOutput expiringTenant;

        public GetExpiringTenantsOutput ExpiringTenant
        {
            get { return expiringTenant; }
            set { expiringTenant = value; RaisePropertyChanged(); }
        }

        public DashboardViewModel(IHostDashboardAppService appService)
        {
            this.appService=appService; 
        } 

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
    }
}
