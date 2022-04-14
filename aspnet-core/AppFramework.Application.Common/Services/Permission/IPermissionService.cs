namespace AppFramework.Shared.Common.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}