using AppFramework.Common.Services;
using AppFramework.MultiTenancy.Dto;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class TenantChangeFeaturesViewModel : HostDialogViewModel
    {
        public IFeaturesService featuresService { get; set; }

        public TenantChangeFeaturesViewModel(IFeaturesService featuresService)
        {
            this.featuresService = featuresService;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                var output = parameters.GetValue<GetTenantFeaturesEditOutput>("Value");

                featuresService.CreateFeatures(output.Features, output.FeatureValues); 
            }
        }
    }
}
