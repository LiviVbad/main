using System.Collections.Generic;
using AppFrameworkDemo.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace AppFrameworkDemo.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
