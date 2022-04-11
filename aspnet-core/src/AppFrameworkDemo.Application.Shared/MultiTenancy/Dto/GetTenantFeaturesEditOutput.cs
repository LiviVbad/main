using Abp.Application.Services.Dto;
using AppFrameworkDemo.Editions.Dto;
using System.Collections.Generic;

namespace AppFrameworkDemo.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}