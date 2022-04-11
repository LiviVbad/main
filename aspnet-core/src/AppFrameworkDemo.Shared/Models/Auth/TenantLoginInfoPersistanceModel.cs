using Abp.Application.Services.Dto;

namespace AppFrameworkDemo.Shared.Models
{
    public class TenantLoginInfoPersistanceModel : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}