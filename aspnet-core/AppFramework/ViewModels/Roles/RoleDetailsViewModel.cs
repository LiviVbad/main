using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppFramework.Common.Services.Permission;
using System.Linq;

namespace AppFramework.ViewModels
{
    public class RoleDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private readonly IRoleAppService appService;
        private RoleEditModel role;
        private ObservableCollection<PermissionModel> permissions;

        public RoleEditModel Role
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<PermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<object> selectedItems;

        public ObservableCollection<object> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
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
                var gps = SelectedItems.Select(t => (t as PermissionModel)?.Name).ToList();

                await WebRequest.Execute(async () =>
                {
                    await appService.CreateOrUpdateRole(new CreateOrUpdateRoleInput()
                    {
                        Role = Map<RoleEditDto>(Role),
                        GrantedPermissionNames = gps
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
                var flats = Map<List<PermissionModel>>(output.Permissions);
                Permissions = flats.CreateTrees(null);
                SelectedItems = Permissions.GetSelectedItems(output.GrantedPermissionNames);
            });
        }
    }
}
