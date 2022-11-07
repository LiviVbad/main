using System.Windows;

namespace AppFramework.Shared.Services.App
{
    public interface IAppStartService
    {
        Window CreateShell(System.Windows.Application app);

        void Logout();

        void Exit();
    }
}
