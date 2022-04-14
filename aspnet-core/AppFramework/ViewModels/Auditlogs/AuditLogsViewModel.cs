using AppFramework.ViewModels;
using Prism.Regions;
using System;
using AppFrameworkDemo.Auditing.Dto;

namespace AppFramework.ViewModels
{
    public class AuditLogsViewModel 
    {
        public GetAuditLogsInput input;

        public AuditLogsViewModel()
        {
            input = new GetAuditLogsInput()
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };
        }
    }
}
