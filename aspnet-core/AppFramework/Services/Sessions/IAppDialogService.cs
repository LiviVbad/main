using System.Threading.Tasks;

namespace AppFramework.Services.Dialog
{
    public interface IAppDialogService
    {
        Task Show(string title, string message);

        bool Question(string title, string message);
    }
}