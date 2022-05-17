using Abp.Application.Services.Dto;
using AppFramework.Editions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Common.Services
{
    public interface IFeaturesService
    {
        void CreateFeatures(List<FlatFeatureDto> features, List<NameValueDto> featureValues);

        List<string> GetSelectedItems();
    }
}
