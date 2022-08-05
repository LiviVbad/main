﻿using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AppFramework.Update.Dtos;
using AppFramework.Dto;
using Abp.Application.Services.Dto;
using AppFramework.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using AppFramework.Storage;

namespace AppFramework.Update
{
    [AbpAuthorize(AppPermissions.Pages_AbpVersions)]
    public class AbpVersionsAppService : AppFrameworkAppServiceBase, IAbpVersionsAppService
    {
        private readonly IRepository<AbpVersion> _abpVersionRepository;

        public AbpVersionsAppService(IRepository<AbpVersion> abpVersionRepository)
        {
            _abpVersionRepository = abpVersionRepository;

        }

        public async Task<PagedResultDto<GetAbpVersionForViewDto>> GetAll(GetAllAbpVersionsInput input)
        {

            var filteredAbpVersions = _abpVersionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Version.Contains(input.Filter) || e.DownloadUrl.Contains(input.Filter) || e.ChangelogUrl.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredAbpVersions = filteredAbpVersions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var abpVersions = from o in pagedAndFilteredAbpVersions
                              select new
                              {

                                  o.Name,
                                  o.Version,
                                  o.DownloadUrl,
                                  o.ChangelogUrl,
                                  o.IsEnable,
                                  o.IsForced,
                                  Id = o.Id
                              };

            var totalCount = await filteredAbpVersions.CountAsync();

            var dbList = await abpVersions.ToListAsync();
            var results = new List<GetAbpVersionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAbpVersionForViewDto()
                {
                    AbpVersion = new AbpVersionDto
                    {

                        Name = o.Name,
                        Version = o.Version,
                        DownloadUrl = o.DownloadUrl,
                        ChangelogUrl = o.ChangelogUrl,
                        IsEnable = o.IsEnable,
                        IsForced = o.IsForced,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAbpVersionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAbpVersionForViewDto> GetAbpVersionForView(int id)
        {
            var abpVersion = await _abpVersionRepository.GetAsync(id);

            var output = new GetAbpVersionForViewDto { AbpVersion = ObjectMapper.Map<AbpVersionDto>(abpVersion) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AbpVersions_Edit)]
        public async Task<GetAbpVersionForEditOutput> GetAbpVersionForEdit(EntityDto input)
        {
            var abpVersion = await _abpVersionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAbpVersionForEditOutput { AbpVersion = ObjectMapper.Map<CreateOrEditAbpVersionDto>(abpVersion) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAbpVersionDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AbpVersions_Create)]
        protected virtual async Task Create(CreateOrEditAbpVersionDto input)
        {
            var abpVersion = ObjectMapper.Map<AbpVersion>(input);

            if (AbpSession.TenantId != null)
            {
                abpVersion.TenantId = (int?)AbpSession.TenantId;
            }

            await _abpVersionRepository.InsertAsync(abpVersion);

        }

        [AbpAuthorize(AppPermissions.Pages_AbpVersions_Edit)]
        protected virtual async Task Update(CreateOrEditAbpVersionDto input)
        {
            var abpVersion = await _abpVersionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, abpVersion);

        }

        [AbpAuthorize(AppPermissions.Pages_AbpVersions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _abpVersionRepository.DeleteAsync(input.Id);
        }

    }
}