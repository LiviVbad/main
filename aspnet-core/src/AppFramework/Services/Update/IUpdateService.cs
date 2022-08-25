 using System.Threading.Tasks;

namespace AppFramework.Services
{
    public interface IUpdateService
    {
        Task CheckVersion();
    }
}
