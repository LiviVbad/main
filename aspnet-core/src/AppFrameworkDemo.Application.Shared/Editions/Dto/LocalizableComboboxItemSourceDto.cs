using System.Collections.ObjectModel;

namespace AppFrameworkDemo.Editions.Dto
{
    //Mapped in CustomDtoMapper
    public class LocalizableComboboxItemSourceDto
    {
        public Collection<LocalizableComboboxItemDto> Items { get; set; }
    }
}