using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AppFramework.EntityFrameworkCore;

namespace AppFramework.HealthChecks
{
    public class AppFrameworkDemoDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public AppFrameworkDemoDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("AppFrameworkDemoDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("AppFrameworkDemoDbContext could not connect to database"));
        }
    }
}
