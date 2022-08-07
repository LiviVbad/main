﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace AppFramework.Update.Dtos
{
    public class CreateOrEditAbpVersionDto : EntityDto<int?>
    { 
        [Required]
        public string Name { get; set; }

        [Required]
        public string Version { get; set; }

        public string DownloadUrl { get; set; }

        public string ChangelogUrl { get; set; }

        public bool IsEnable { get; set; }

        public bool IsForced { get; set; } 
    }
}