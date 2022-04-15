using Abp.Application.Services.Dto;
using AppFramework.Editions.Dto;
using System.Collections.Generic;

namespace AppFramework.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}