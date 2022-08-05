 using System.Threading.Tasks;

namespace AppFramework.Services.Update
{
    public interface IUpdateService
    {
        Task CheckVersion();
    }
}
