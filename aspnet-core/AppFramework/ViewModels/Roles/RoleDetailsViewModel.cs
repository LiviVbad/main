using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppFramework.Common.Services.Permission; 

namespace AppFramework.ViewModels
{
    public class RoleDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private readonly IRoleAppService appService;
        private RoleEditModel role;
        private ObservableCollection<FlatPermissionModel> permissions;

        public RoleEditModel Role
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<FlatPermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        #endregion 字段/属性

        public RoleDetailsViewModel(IRoleAppService appService)
        {
            this.appService = appService;
        }

        /// <summary>
        /// 保存
        /// </summary>
        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                List<string> GrantedPermissionNames = new List<string>();
                Permissions.GetSelectedNodes(ref GrantedPermissionNames);

                await WebRequest.Execute(async () =>
                {
                    await appService.CreateOrUpdateRole(new CreateOrUpdateRoleInput()
                    {
                        Role = Map<RoleEditDto>(Role),
                        GrantedPermissionNames = GrantedPermissionNames
                    });
                });
                base.Save();
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                int? id = null;

                if (parameters != null && parameters.ContainsKey("Value"))
                    id = parameters.GetValue<RoleListModel>("Value").Id;

                var output = await appService.GetRoleForEdit(new NullableIdDto(id));

                Role = Map<RoleEditModel>(output.Role);
                var flats = Map<List<FlatPermissionModel>>(output.Permissions);
                Permissions = flats.CreateTrees(null);
                Permissions.UpdateSelectedNodes(output.GrantedPermissionNames);
            });
        }
    }
}
