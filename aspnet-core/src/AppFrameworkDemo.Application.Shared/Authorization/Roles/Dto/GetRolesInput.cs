using System.Collections.Generic;

namespace AppFrameworkDemo.Authorization.Roles.Dto
{
    public class GetRolesInput
    {
        public List<string> Permissions { get; set; }
    }
}