using AppFramework.Dto;
using AppFramework.Version.Dtos;
using System;
using System.Threading.Tasks;

namespace AppFramework.Version
{
    public interface IAppUpdateService
    {  
        Task<UpdateFileOutput> CheckVersion(CheckVersionInput input);
    }
}
