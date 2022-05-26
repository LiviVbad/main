using AppFramework.Security;

namespace AppFramework.Common.Models.Configuration
{
    public class SecuritySettingsEditModel
    {
        public bool AllowOneConcurrentLoginPerUser { get; set; }

        public bool UseDefaultPasswordComplexitySettings { get; set; }

        public PasswordComplexitySetting PasswordComplexity { get; set; }

        public PasswordComplexitySetting DefaultPasswordComplexity { get; set; }

        public UserLockOutSettingsEditModel UserLockOut { get; set; }

        public TwoFactorLoginSettingsEditModel TwoFactorLogin { get; set; }
    }
}