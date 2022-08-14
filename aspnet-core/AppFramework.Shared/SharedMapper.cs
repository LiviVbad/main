using AutoMapper;
using AppFramework.Shared.Models.Configuration;
using AppFramework.Configuration.Host.Dto;
using AppFramework.Configuration.Dto; 

namespace AppFramework.Shared.Core
{
    public class SharedMapper : Profile
    {
        public SharedMapper()
        {
            CreateMap<HostSettingsEditModel, HostSettingsEditDto>().ReverseMap();
            CreateMap<GeneralSettingsEditModel, GeneralSettingsEditDto>().ReverseMap();
            CreateMap<HostUserManagementSettingsEditModel, HostUserManagementSettingsEditDto>().ReverseMap();
            CreateMap<EmailSettingsEditModel, EmailSettingsEditDto>().ReverseMap();
            CreateMap<TenantManagementSettingsEditModel, TenantManagementSettingsEditDto>().ReverseMap();
            CreateMap<SecuritySettingsEditModel, SecuritySettingsEditDto>().ReverseMap();
            CreateMap<HostBillingSettingsEditModel, HostBillingSettingsEditDto>().ReverseMap();
            CreateMap<OtherSettingsEditModel, OtherSettingsEditDto>().ReverseMap();
            CreateMap<ExternalLoginProviderSettingsEditModel, ExternalLoginProviderSettingsEditDto>().ReverseMap();
            CreateMap<UserLockOutSettingsEditModel, UserLockOutSettingsEditDto>().ReverseMap();
            CreateMap<TwoFactorLoginSettingsEditModel, TwoFactorLoginSettingsEditDto>().ReverseMap();
            CreateMap<SessionTimeOutSettingsEditModel, SessionTimeOutSettingsEditDto>().ReverseMap();
            CreateMap<UserPasswordSettingsEditModel, UserPasswordSettingsEditDto>().ReverseMap();
        }
    }
}