using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace AppFramework.Configuration.Host.Dto
{
    public class SendTestEmailInput
    {
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}