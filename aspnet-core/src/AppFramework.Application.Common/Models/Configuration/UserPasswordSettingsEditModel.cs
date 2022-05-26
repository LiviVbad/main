namespace AppFramework.Common.Models.Configuration
{
    public class UserPasswordSettingsEditModel
    {
        public bool EnableCheckingLastXPasswordWhenPasswordChange { get; set; }

        public int CheckingLastXPasswordCount { get; set; }

        public bool EnablePasswordExpiration { get; set; }

        public int PasswordExpirationDayCount { get; set; }
    }
}