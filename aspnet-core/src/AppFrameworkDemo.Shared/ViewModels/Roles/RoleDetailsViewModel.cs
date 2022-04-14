using Abp.Application.Services.Dto;
using AppFrameworkDemo.Authorization.Roles;
using AppFrameworkDemo.Authorization.Roles.Dto;
using AppFramework.Application.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class RoleDetailsViewModel : NavigationViewModel
    {
        #region 字段/属性

        private readonly IRoleAppService appService;
        private readonly IMessenger messenger;
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

        public DelegateCommand SaveCommand { get; private set; }

        #endregion 字段/属性

        public RoleDetailsViewModel(IRoleAppService appService,
            IMessenger messenger)
        {
            this.appService = appService;
            this.messenger = messenger;
            SaveCommand = new DelegateCommand(Save);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private async void Save()
        {
            await SetBusyAsync(async () =>
            {
                List<string> GrantedPermissionNames = new List<string>();
                GetSelectedNodes(Permissions, ref GrantedPermissionNames);

                await WebRequestRuner.Execute(async () =>
                {
                    await appService.CreateOrUpdateRole(new CreateOrUpdateRoleInput()
                    {
                        Role = Map<RoleEditDto>(Role),
                        GrantedPermissionNames = GrantedPermissionNames
                    });
                },
                async () => await GoBackAsync());
            });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Role);//通知列表更新
            await base.GoBackAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
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