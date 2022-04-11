using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace AppFrameworkDemo.Net.Emailing
{
    public class AppFrameworkDemoSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public AppFrameworkDemoSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}