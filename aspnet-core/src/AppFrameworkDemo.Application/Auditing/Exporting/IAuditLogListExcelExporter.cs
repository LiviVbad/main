using System.Collections.Generic;
using AppFrameworkDemo.Auditing.Dto;
using AppFrameworkDemo.Dto;

namespace AppFrameworkDemo.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
