using Abp.Application.Services.Dto;
using AppFramework.Authorization.Permissions;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Services.Permission;
using AppFramework.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class RoleViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

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

        #endregion

        public RoleViewModel(IRoleAppService appService,
            IPermissionAppService permissionAppService)
        {
            Title = Local.Localize("Roles");
            this.appService = appService;
            this.permissionAppService = permissionAppService;
            input = new GetRolesInput();
            SelectedCommand = new DelegateCommand(SelectedPermission);

            dataPager.OnPageIndexChangedEventhandler += RoleOnPageIndexChangedEventhandler;

            UpdateTitle();
        }

        private async void RoleOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            //filter... 
            await SetBusyAsync(async () =>
            {
                await GetRoles(input);
            });
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
                await OnNavigatedToAsync();
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        public async void Delete()
        {
            if (dataPager.SelectedItem is RoleListDto item)
            {
                if (await dialog.Question(Local.Localize("RoleDeleteWarningMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await WebRequest.Execute(() => appService.DeleteRole(
                            new EntityDto(item.Id)),
                            async()=>await OnNavigatedToAsync());
                    });
                }
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private async Task GetRoles(GetRolesInput filter)
        {
            await WebRequest.Execute(() => appService.GetRoles(filter),
                       async result =>
                       {
                           dataPager.SetList(new PagedResultDto<RoleListDto>
                           {
                               Items = result.Items
                           });
                           await Task.CompletedTask;
                       });
        }

        /// <summary>
        /// 刷新角色模块
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
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

                await GetRoles(input);
            });
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.RoleEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.RoleDelete, Local.Localize("Delete"),Delete)
            };
        }
    }
}