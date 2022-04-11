using System.Collections.Generic;
using Abp;
using AppFrameworkDemo.Chat.Dto;
using AppFrameworkDemo.Dto;

namespace AppFrameworkDemo.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
