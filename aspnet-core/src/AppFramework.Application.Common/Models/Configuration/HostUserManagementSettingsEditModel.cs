namespace AppFramework.Common.Models.Configuration
{
    public class HostUserManagementSettingsEditModel
    {
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        public bool SmsVerificationEnabled { get; set; }

        public bool IsCookieConsentEnabled { get; set; }

        public bool IsQuickThemeSelectEnabled { get; set; }

        public bool UseCaptchaOnLogin { get; set; }

        public bool AllowUsingGravatarProfilePicture { get; set; }

        public SessionTimeOutSettingsEditModel SessionTimeOutSettings { get; set; }

        public UserPasswordSettingsEditModel UserPasswordSettings { get; set; }
    }
}