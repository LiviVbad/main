using System.Threading.Tasks;
using AppFramework.Sessions.Dto;

namespace AppFramework.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
