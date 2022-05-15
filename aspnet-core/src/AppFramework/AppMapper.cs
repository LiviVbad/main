using AppFramework.Auditing.Dto;
using AppFramework.Models.Auditlogs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<GetAuditLogsFilter, GetAuditLogsInput>().ReverseMap();
        }
    }
}
