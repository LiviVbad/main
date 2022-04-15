namespace AppFramework.Common.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}