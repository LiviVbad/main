using Abp.Application.Services;
using Abp.Extensions;
using AppFrameworkDemo.ApiClient;

namespace AppFrameworkDemo
{
    public abstract class ProxyAppServiceBase : IApplicationService
    {
        public AbpApiClient ApiClient { get; set; }

        public const string ApiBaseUrl = "api/services/app/";

        private readonly string _serviceUrlSegment;

        protected ProxyAppServiceBase(AbpApiClient apiClient)
        {
            ApiClient = apiClient;
            _serviceUrlSegment = GetServiceUrlSegmentByConvention();
        }

        protected string GetEndpoint(string methodName)
        {
            return ApiBaseUrl + _serviceUrlSegment + "/" + methodName;
        }

        private string GetServiceUrlSegmentByConvention()
        {
            return GetType()
                .Name
                .RemovePreFix("Proxy")
                .RemovePostFix("AppServiceProxy", "AppService");
        }
    }
}