﻿using Abp.Application.Services.Dto;
using AppFramework.Authorization.Permissions;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class RoleViewModel : NavigationCurdViewModel
    {
        private readonly IRoleAppService appService;
        private readonly IPermissionAppService permissionAppService;
        public GetRolesInput input;

        private string selectPermissions = string.Empty;

        /// <summary>
        /// 已选择权限的文本
        /// </summary>
        public string SelectPermissions
        {
            get { return selectPermissions; }
            set { selectPermissions = value; RaisePropertyChanged(); }
        }

        private ListResultDto<FlatPermissionWithLevelDto> flatPermission;

        public DelegateCommand SelectedCommand { get; private set; }

        public RoleViewModel(IRoleAppService appService,
            IPermissionAppService permissionAppService)
        {
            this.appService = appService;
            this.permissionAppService = permissionAppService;
            input = new GetRolesInput();
            SelectedCommand = new DelegateCommand(SelectedPermission);

            UpdateTitle();
        }

        private void UpdateTitle(int count = 0)
        {
            SelectPermissions = Local.Localize("SelectPermissions") + $"({count})";
        }

        /// <summary>
        /// 选择权限
        /// </summary>
        private async void SelectedPermission()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", flatPermission);
            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.SelectedPermission, param);
            if (dialogResult.Result == Prism.Services.Dialogs.ButtonResult.OK)
            {
                var selectedPermissions = dialogResult.Parameters.GetValue<List<string>>("Value");

                input.Permissions = selectedPermissions;
                UpdateTitle(selectedPermissions.Count);
                await RefreshAsync();
            }
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                if (flatPermission == null)
                {
                    await WebRequest.Execute(() => permissionAppService.GetAllPermissions(), result =>
                       {
                           flatPermission = result;
                           return Task.CompletedTask;
                       });
                }

                await WebRequest.Execute(() => appService.GetRoles(input),
                        async result =>
                        {
                            GridModelList.Clear();

                            foreach (var item in Map<List<RoleListModel>>(result.Items))
                                GridModelList.Add(item);

                            await Task.CompletedTask;
                        });
            });
        }

        public async void Delete()
        {
            if (SelectedItem is RoleListModel item)
            {
                if (await dialog.Question(Local.Localize("RoleDeleteWarningMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() => appService.DeleteRole(
                            new EntityDto(item.Id)),
                            RefreshAsync);
                    });
                }
            }
        }

        public override PermButton[] GeneratePermissionButtons()
        {
            return new PermButton[]
            {
                new PermButton(Permkeys.RoleEdit, Local.Localize("Change"),()=>Edit()),
                new PermButton(Permkeys.RoleDelete, Local.Localize("Delete"),()=>Delete())
            };
        }
    }
}