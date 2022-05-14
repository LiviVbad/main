using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Auditing.Dto;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Authorization.Users.Dto;
using AppFramework.DynamicEntityProperties.Dto;
using AppFramework.Editions.Dto;
using AppFramework.Localization.Dto;
using AppFramework.MultiTenancy.Dto;
using AppFramework.Organizations.Dto;
using AppFramework.Sessions.Dto;
using AppFramework.Common.Models; 
using AutoMapper;

namespace AppFramework.Common.Core
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            //系统模块中实体映射关系 
            CreateMap<UserListDto,UserListModel>().ReverseMap();
            CreateMap<UserEditDto,UserEditModel>().ReverseMap();
            CreateMap<RoleListDto,RoleListModel>().ReverseMap();
            CreateMap<RoleEditDto,RoleEditModel>().ReverseMap();
            CreateMap<TenantListDto, TenantListModel>().ReverseMap();
            CreateMap<TenantEditDto, TenantListModel>().ReverseMap();
            CreateMap<AuditLogListDto, AuditLogListModel>().ReverseMap();
            CreateMap<UserCreateOrUpdateModel, CreateOrUpdateUserInput>().ReverseMap();
            CreateMap<DynamicPropertyDto, DynamicPropertyModel>().ReverseMap();
            CreateMap<OrganizationUnitDto, OrganizationListModel>().ReverseMap();
            CreateMap<OrganizationUnitDto, OrganizationUnitModel>().ReverseMap();
            CreateMap<ApplicationLanguageListDto, LanguageListModel>().ReverseMap();
            CreateMap<UserLoginInfoDto, UserLoginInfoModel>().ReverseMap();
            CreateMap<UserLoginInfoDto, UserLoginInfoPersistanceModel>().ReverseMap();
            CreateMap<AbpAuthenticateResultModel, AuthenticateResultPersistanceModel>().ReverseMap();
            CreateMap<TenantInformation, TenantInformationPersistanceModel>().ReverseMap();
            CreateMap<TenantLoginInfoDto, TenantLoginInfoPersistanceModel>().ReverseMap();
            CreateMap<ApplicationInfoDto, ApplicationInfoPersistanceModel>().ReverseMap();
            CreateMap<EditionListDto, EditionListModel>().ReverseMap();
            CreateMap<EditionCreateDto, EditionCreateModel>().ReverseMap();
            CreateMap<EditionEditDto, EditionCreateModel>().ReverseMap();
            CreateMap<FlatFeatureDto,FlatFeatureModel>().ReverseMap();
            CreateMap<FlatPermissionDto, PermissionModel>().ReverseMap();
            CreateMap<GetUserForEditOutput, UserForEditModel>().ReverseMap();
            CreateMap<GetCurrentLoginInformationsOutput, CurrentLoginInformationPersistanceModel>().ReverseMap();
            CreateMap<TenantListModel, CreateTenantInput>().ReverseMap();

            /*
             * 以下可添加更多的实体映射关系
             */
        }
    }
}