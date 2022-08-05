using Abp.Application.Services.Dto;
using System;

namespace AppFramework.Update.Dtos
{
    public class GetAllAbpVersionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}