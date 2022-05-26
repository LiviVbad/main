namespace AppFramework.Common.Models.Configuration
{
    public class TenantManagementSettingsEditModel
    {
        public bool AllowSelfRegistration { get; set; }

        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        public bool UseCaptchaOnRegistration { get; set; }

        public int? DefaultEditionId { get; set; }
    }
}