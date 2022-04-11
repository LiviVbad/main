using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFrameworkDemo.MultiTenancy.HostDashboard.Dto;

namespace AppFrameworkDemo.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}