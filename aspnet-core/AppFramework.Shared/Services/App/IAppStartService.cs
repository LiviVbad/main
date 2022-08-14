using System.Threading.Tasks;
using System.Windows;

namespace AppFramework.Shared.Services.App
{
    public interface IAppStartService
    {
        Task<Window> CreateShell(System.Windows.Application app);

        void Logout();

        void Exit();
    }
}
