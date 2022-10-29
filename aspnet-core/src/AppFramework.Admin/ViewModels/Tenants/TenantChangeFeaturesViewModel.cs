﻿using AppFramework.Shared; 
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto; 
using Prism.Services.Dialogs; 
using System.Threading.Tasks;
using AppFramework.Services;

namespace AppFramework.ViewModels
{
    public class TenantChangeFeaturesViewModel : HostDialogViewModel
    {
        private int Id;
        private readonly ITenantAppService tenantAppService;

        public IFeaturesService featuresService { get; set; }

        public TenantChangeFeaturesViewModel(IFeaturesService featuresService,
            ITenantAppService tenantAppService)
        {
            this.featuresService = featuresService;
            this.tenantAppService = tenantAppService;
        }

        public override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => tenantAppService.UpdateTenantFeatures(new UpdateTenantFeaturesInput()
                {
                    Id = Id,
                    FeatureValues = featuresService.GetSelectedItems()
                }), async () =>
                 {
                     base.Save();
                     await Task.CompletedTask;
                 });
            });
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Id"))
                Id = parameters.GetValue<int>("Id");

            if (parameters.ContainsKey("Value"))
            {
                var output = parameters.GetValue<GetTenantFeaturesEditOutput>("Value");

                featuresService.CreateFeatures(output.Features, output.FeatureValues);
            }
        }
    }
}
