using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AppFramework.Update.Dtos
{
    public class GetAbpVersionForEditOutput
    {
        public CreateOrEditAbpVersionDto AbpVersion { get; set; }

    }
}