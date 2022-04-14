using AppFrameworkDemo.ApiClient;
using AppFrameworkDemo.ApiClient.Models;
using AppFrameworkDemo.Auditing.Dto;
using AppFrameworkDemo.Authorization.Permissions.Dto;
using AppFrameworkDemo.Authorization.Roles.Dto;
using AppFrameworkDemo.Authorization.Users.Dto;
using AppFrameworkDemo.DynamicEntityProperties.Dto;
using AppFrameworkDemo.Editions.Dto;
using AppFrameworkDemo.Localization.Dto;
using AppFrameworkDemo.MultiTenancy.Dto;
using AppFrameworkDemo.Organizations.Dto;
using AppFrameworkDemo.Sessions.Dto;
using AppFramework.Shared.Common.Models; 
using AutoMapper;

namespace AppFramework.Shared.Common.Core
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
            CreateMap<FlatPermissionDto, FlatPermissionModel>().ReverseMap();
            CreateMap<GetUserForEditOutput, UserForEditModel>().ReverseMap();
            CreateMap<GetCurrentLoginInformationsOutput, CurrentLoginInformationPersistanceModel>().ReverseMap();
            CreateMap<TenantListModel, CreateTenantInput>().ReverseMap();

            /*
             * 以下可添加更多的实体映射关系
             */
        }
    }
}