using AppFramework.Dto;
using AppFramework.Update.Dtos;
using System;
using System.Threading.Tasks;

namespace AppFramework.Update
{
    public interface IAppUpdateService
    {  
        Task<UpdateFileOutput> CheckVersion(CheckVersionInput input);
    }
}
