using System.Threading.Tasks;
using AppFrameworkDemo.Sessions.Dto;

namespace AppFrameworkDemo.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
