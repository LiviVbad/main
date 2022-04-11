using Abp.Authorization;
using AppFrameworkDemo.Authorization.Roles;
using AppFrameworkDemo.Authorization.Users;

namespace AppFrameworkDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
