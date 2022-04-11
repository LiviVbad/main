using System.ComponentModel.DataAnnotations;

namespace AppFrameworkDemo.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}