using AppFramework.Auditing.Dto;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Shared.Models;
using AppFramework.Models.Auditlogs;
using AppFramework.Models.Tenants;
using AppFramework.MultiTenancy.Dto;
using AutoMapper; 

namespace AppFramework.Admin
{
    public class AdminMapper : Profile
    {
        public AdminMapper()
        {
            CreateMap<GetAuditLogsFilter, GetAuditLogsInput>().ReverseMap();
            CreateMap<GetEntityChangeFilter, GetEntityChangeInput>().ReverseMap();
            CreateMap<GetTenantsFilter, GetTenantsInput>().ReverseMap();
            CreateMap<FlatPermissionWithLevelDto, PermissionModel>().ReverseMap();
        }
    }
}
