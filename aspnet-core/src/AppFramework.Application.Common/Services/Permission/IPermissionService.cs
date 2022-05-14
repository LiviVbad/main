namespace AppFramework.Common
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}