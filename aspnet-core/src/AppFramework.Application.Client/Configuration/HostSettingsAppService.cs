﻿using AppFramework;
using AppFramework.ApiClient;
using AppFramework.Configuration.Host;
using AppFramework.Configuration.Host.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Configuration
{
    public class HostSettingsAppService : ProxyAppServiceBase, IHostSettingsAppService
    {
        public HostSettingsAppService(AbpApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return await ApiClient.GetAsync<HostSettingsEditDto>(GetEndpoint(nameof(GetAllSettings)));
        }

        public async Task SendTestEmail(SendTestEmailInput input)
        {
            await ApiClient.PostAsync(GetEndpoint(nameof(SendTestEmail)), input);
        }

        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await ApiClient.PutAsync(GetEndpoint(nameof(UpdateAllSettings)), input);
        }
    }
}
