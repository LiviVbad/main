using System.ComponentModel.DataAnnotations;

namespace AppFrameworkDemo.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}