using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Common
{
    public interface IPermissionTreesService
    {
        void CreatePermissionTrees(List<FlatPermissionDto> permissions, List<string> grantedPermissionNames);

        List<string> GetSelectedItems();
    }
}
