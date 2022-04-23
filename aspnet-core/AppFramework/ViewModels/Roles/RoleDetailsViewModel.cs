using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Services.Dialogs; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq; 

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
                GetSelectedNodes(Permissions, ref GrantedPermissionNames);

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
                Permissions = CreateTrees(flats, null);

                UpdateSelectedNodes(Permissions, output.GrantedPermissionNames);
            });
        }

        #region 内部方法

        /// <summary>
        /// 创建权限节点目录树
        /// </summary>
        /// <param name="flats"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        private ObservableCollection<FlatPermissionModel> CreateTrees(List<FlatPermissionModel> flats, string parentName)
        {
            var trees = new ObservableCollection<FlatPermissionModel>();

            var nodes = flats.Where(q => q.ParentName == parentName).ToArray();

            foreach (var node in nodes)
            {
                node.Items = CreateTrees(flats, node.Name);
                trees.Add(node);
            }

            return trees;
        }

        /// <summary>
        /// 获取选中的权限节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="GrantedPermissionNames"></param>
        private void GetSelectedNodes(ObservableCollection<FlatPermissionModel> nodes, ref List<string> GrantedPermissionNames)
        {
            foreach (var flat in nodes)
            {
                if (flat.IsChecked) GrantedPermissionNames.Add(flat.Name);

                GetSelectedNodes(flat.Items, ref GrantedPermissionNames);
            }
        }

        /// <summary>
        /// 更新选中权限节点
        /// </summary>
        /// <param name="GrantedPermissionNames"></param>
        private void UpdateSelectedNodes(ObservableCollection<FlatPermissionModel> Flats, List<string> GrantedPermissionNames)
        {
            GrantedPermissionNames.ForEach(item =>
            {
                UpdateSelected(Flats, item);
            });
        }

        /// <summary>
        /// 设置选中权限节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeName"></param>
        private void UpdateSelected(ObservableCollection<FlatPermissionModel> nodes, string nodeName)
        {
            foreach (var flat in nodes)
            {
                if (flat.Name.Equals(nodeName))
                {
                    flat.IsChecked = true;
                    return;
                }

                UpdateSelected(flat.Items, nodeName);
            }
        }

        #endregion 内部方法
    }
}
