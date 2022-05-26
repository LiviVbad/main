namespace AppFramework.Common.Models.Configuration
{
    public class UserLockOutSettingsEditModel
    {
        public bool IsEnabled { get; set; }

        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        public int DefaultAccountLockoutSeconds { get; set; }
    }
}