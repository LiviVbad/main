using AppFramework.Version.Dtos;
using System; 
using System.Threading.Tasks;

namespace AppFramework.Version
{
    public class AppUpdateService : IAppUpdateService
    {
        public Task<UpdateFileOutput> CheckVersion(CheckVersionInput input)
        {
            throw new NotImplementedException();
        } 
    }
}
