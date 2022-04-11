using System.Collections.Generic;
using AppFrameworkDemo.Authorization.Users.Dto;
using AppFrameworkDemo.Dto;

namespace AppFrameworkDemo.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}