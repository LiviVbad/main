using AppFramework.Update.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
