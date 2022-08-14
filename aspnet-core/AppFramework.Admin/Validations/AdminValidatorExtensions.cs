using AppFramework.Models;
using AppFramework.MultiTenancy.Dto;
using AppFramework.Shared.Models.Configuration;
using AppFramework.Validations;
using FluentValidation;
using Prism.Ioc; 

namespace Validations
{
    public static class AdminValidatorExtensions
    {
        /// <summary>
        ///  注册FluentValidation
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterValidator(this IContainerRegistry services)
        { 
            services.RegisterScoped<IValidator<UserCreateOrUpdateModel>, UserCreateOrUpdateValidator>();
            services.RegisterScoped<IValidator<CreateOrganizationUnitModel>, OrganizationUnitValidator>();
            services.RegisterScoped<IValidator<CreateTenantInput>, CreateTenantValidator>();
            services.RegisterScoped<IValidator<TenantEditDto>, UpdateTenantValidator>();
            services.RegisterScoped<IValidator<EditionCreateModel>, CreateEditionValidator>();
            services.RegisterScoped<IValidator<HostSettingsEditModel>, SettingsValidator>();
            services.RegisterScoped<IValidator<VersionListModel>, VersionValidator>();
        }
    }
}
