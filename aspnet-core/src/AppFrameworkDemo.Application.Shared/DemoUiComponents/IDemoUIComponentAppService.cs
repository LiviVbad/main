using Abp;
using Abp.Application.Services;
using AppFrameworkDemo.DemoUiComponents.Dto;
using System;
using System.Collections.Generic;

namespace AppFrameworkDemo.DemoUiComponents
{
    public interface IDemoUiComponentsAppService : IApplicationService
    {
        DateToStringOutput SendAndGetDate(DateTime? date);

        DateToStringOutput SendAndGetDateTime(DateTime? date);

        DateToStringOutput SendAndGetDateRange(DateTime? startDate, DateTime? endDate);

        List<NameValue<string>> GetCountries(string searchTerm);

        List<NameValue<string>> SendAndGetSelectedCountries(List<NameValue<string>> selectedCountries);

        StringOutput SendAndGetValue(string input);
    }
}