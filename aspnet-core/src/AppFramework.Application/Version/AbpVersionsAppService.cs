using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using AppFramework.Version.Dtos;
using Abp.Application.Services.Dto;
using AppFramework.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Nito.AsyncEx;
using System.Runtime.Versioning;

namespace AppFramework.Version
{
    [AbpAuthorize(AppPermissions.Pages_AbpVersions)]
    public class AbpVersionsAppService : AppFrameworkAppServiceBase, IAbpVersionsAppService
    {
        private readonly IRepository<AbpVersion> _abpVersionRepository;

        public AbpVersionsAppService(
            IHttpContextAccessor httpContext,
             IRepository<AbpVersion> abpVersionRepository)
        {
            _abpVersionRepository = abpVersionRepository;

        }

        public async Task<PagedResultDto<AbpVersionDto>> GetAll(GetAllAbpVersionsInput input)
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
            var results = new List<AbpVersionDto>();

            foreach (var o in dbList)
            {
                var res = new AbpVersionDto
                {

                    Name = o.Name,
                    Version = o.Version,
                    DownloadUrl = o.DownloadUrl,
                    ChangelogUrl = o.ChangelogUrl,
                    IsEnable = o.IsEnable,
                    IsForced = o.IsForced,
                    Id = o.Id,
                };

                results.Add(res);
            }

            return new PagedResultDto<AbpVersionDto>(
                totalCount,
                results
            );

        }

        public async Task<AbpVersionDto> GetAbpVersionForView(int id)
        {
            var abpVersion = await _abpVersionRepository.GetAsync(id);

            return ObjectMapper.Map<AbpVersionDto>(abpVersion);
        }

        [AbpAuthorize(AppPermissions.Pages_AbpVersions_Edit)]
        public async Task<AbpVersionDto> GetAbpVersionForEdit(EntityDto input)
        {
            var abpVersion = await _abpVersionRepository.FirstOrDefaultAsync(input.Id);

            return ObjectMapper.Map<AbpVersionDto>(abpVersion);
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

        public async Task<UpdateFileOutput> CheckVersion(CheckVersionInput input)
        {
            var abpVersion = await _abpVersionRepository.FirstOrDefaultAsync(t => t.Name.Equals(input.ApplicationName) &&
            new System.Version(t.Version) > new System.Version(input.Version));

            if (abpVersion != null)
            {
                return new UpdateFileOutput()
                {
                    IsNewVersion = true,
                    Version = abpVersion.Version,
                    IsForced = abpVersion.IsForced,
                    DownloadURL = abpVersion.DownloadUrl,
                    ChangelogURL = abpVersion.ChangelogUrl
                };
            }
            else
                return new UpdateFileOutput();
        }
    }
}