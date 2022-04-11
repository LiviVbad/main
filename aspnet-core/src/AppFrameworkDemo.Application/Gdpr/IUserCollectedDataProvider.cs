﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using AppFrameworkDemo.Dto;

namespace AppFrameworkDemo.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
