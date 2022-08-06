using AppFramework.Update.Dtos;
using System; 
using System.Threading.Tasks;

namespace AppFramework.Update
{
    public class AppUpdateService : IAppUpdateService
    {
        public Task<UpdateFileOutput> CheckVersion(CheckVersionInput input)
        {
            throw new NotImplementedException();
        } 
    }
}
