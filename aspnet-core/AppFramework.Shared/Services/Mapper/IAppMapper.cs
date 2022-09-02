using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Mapper
{
    public interface IAppMapper
    {
        IMapper Current { get; }

        TDestination Map<TDestination>(object source);
    }
}
