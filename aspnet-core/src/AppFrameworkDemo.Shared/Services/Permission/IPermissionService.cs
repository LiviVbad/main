namespace AppFrameworkDemo.Shared.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}