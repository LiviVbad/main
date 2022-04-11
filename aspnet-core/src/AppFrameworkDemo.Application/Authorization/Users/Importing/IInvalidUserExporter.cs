using System.Collections.Generic;
using AppFrameworkDemo.Authorization.Users.Importing.Dto;
using AppFrameworkDemo.Dto;

namespace AppFrameworkDemo.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
