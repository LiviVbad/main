using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using System.Threading.Tasks;
using AppFramework.Common.Core;
using AppFramework.Common.Models;
using AppFramework.Common;
using AppFramework.Common.Services.Permission;

namespace AppFramework.Shared.ViewModels
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
                Permissions.GetSelectedNodes(ref GrantedPermissionNames);

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
                Permissions = flats.CreateTrees(null);
                Permissions.UpdateSelectedNodes(output.GrantedPermissionNames);
            });
        }
    }
}