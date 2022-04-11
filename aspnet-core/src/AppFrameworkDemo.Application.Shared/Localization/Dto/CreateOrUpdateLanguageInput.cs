using System.ComponentModel.DataAnnotations;

namespace AppFrameworkDemo.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}