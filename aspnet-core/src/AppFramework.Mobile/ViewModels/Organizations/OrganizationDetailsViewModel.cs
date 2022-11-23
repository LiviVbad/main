using Abp.Application.Services.Dto;
using AppFramework.Shared.Core;
using AppFramework.Shared.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Views;

namespace AppFramework.Shared.ViewModels
{
    public class OrganizationDetailsViewModel : NavigationDetailViewModel
    {
        #region 字段/属性

        private bool newOrganizationIsVisible = true;

        public bool NewOrganizationIsVisible
        {
            get { return newOrganizationIsVisible; }
            set { newOrganizationIsVisible = value; RaisePropertyChanged(); }
        }

        private bool IsNewOrganization;
        private long? ParentId;
        private readonly IMessenger messenger;
        private readonly IPermissionService permissionService;
        private readonly IOrganizationUnitAppService appService;
        public DelegateCommand AddUsersCommand { get; private set; }
        public DelegateCommand AddRolesCommand { get; private set; }
        public DelegateCommand<OrganizationUnitUserListDto> DeleteUserCommand { get; private set; }
        public DelegateCommand<OrganizationUnitRoleListDto> DeleteRoleCommand { get; private set; }

        private OrganizationListModel organizationUnit;
        private ObservableCollection<OrganizationUnitRoleListDto> rolesModelList;
        private ObservableCollection<OrganizationUnitUserListDto> userModelList;

        public OrganizationListModel OrganizationUnit
        {
            get { return organizationUnit; }
            set { organizationUnit = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<OrganizationUnitRoleListDto> RolesModelList
        {
            get { return rolesModelList; }
            set { rolesModelList = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<OrganizationUnitUserListDto> UserModelList
        {
            get { return userModelList; }
            set { userModelList = value; RaisePropertyChanged(); }
        }

        #endregion 字段/属性

        public OrganizationDetailsViewModel(IMessenger messenger,
            IPermissionService permissionService,
            IOrganizationUnitAppService appService)
        {
            this.messenger = messenger;
            this.permissionService = permissionService;
            this.appService = appService;
            AddUsersCommand = new DelegateCommand(AddUsers);
            AddRolesCommand = new DelegateCommand(AddRoles);
            DeleteUserCommand = new DelegateCommand<OrganizationUnitUserListDto>(DeleteUser);
            DeleteRoleCommand = new DelegateCommand<OrganizationUnitRoleListDto>(DeleteRole);
            UserModelList = new ObservableCollection<OrganizationUnitUserListDto>();
            RolesModelList = new ObservableCollection<OrganizationUnitRoleListDto>();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override async void Save()
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(async () =>
                 {
                     OrganizationUnitDto organizationUnit = null;

                     if (IsNewOrganization)
                     {
                         if (OrganizationUnit.Id > 0) return organizationUnit;

                         organizationUnit = await appService.CreateOrganizationUnit(new CreateOrganizationUnitInput()
                         {
                             ParentId = ParentId,
                             DisplayName = OrganizationUnit.DisplayName
                         });
                     }
                     else
                     {
                         await appService.UpdateOrganizationUnit(new UpdateOrganizationUnitInput()
                         {
                             Id = OrganizationUnit.Id,
                             DisplayName = OrganizationUnit.DisplayName
                         });
                     };
                     return organizationUnit;
                 }, result => CreateOrganizationUnitSuccessed(result));
             });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        public async void AddUsers()
        {
            if (!permissionService.HasPermission(AppPermissions.OrganizationUnitsManageMembers))
                return;

            await navigationService.NavigateAsync(AppViews.AddUsers, GetParameter());
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        public async void AddRoles()
        {
            if (!permissionService.HasPermission(AppPermissions.OrganizationUnitsManageRoles))
                return;

            await navigationService.NavigateAsync(AppViews.AddRoles, GetParameter());
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteUser(OrganizationUnitUserListDto organizationUnit)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                 await appService.RemoveUserFromOrganizationUnit(new UserToOrganizationUnitInput()
                 {
                     OrganizationUnitId = OrganizationUnit.Id,
                     UserId = organizationUnit.Id
                 }), async () =>
                 {
                     var user = UserModelList.FirstOrDefault(t => t.Id.Equals(organizationUnit.Id));
                     if (user != null) UserModelList.Remove(user);

                     await Task.CompletedTask;
                 });
            });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteRole(OrganizationUnitRoleListDto unitRoleListDto)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    await appService.RemoveRoleFromOrganizationUnit(new RoleToOrganizationUnitInput()
                    {
                        OrganizationUnitId = OrganizationUnit.Id,
                        RoleId = (int)unitRoleListDto.Id
                    });
                }, async () =>
                {
                    var role = RolesModelList.FirstOrDefault(t => t.Id.Equals(unitRoleListDto.Id));
                    if (role != null) RolesModelList.Remove(role);
                    await Task.CompletedTask;
                });
            });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Organization);//通知列表更新
            await base.GoBackAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await SetBusyAsync(async () =>
             {
                 if (parameters.ContainsKey("IsRefreshUserOrRoles"))
                 {
                     //当从其它导航页返回到当前页面时,判断是否刷新当前页面的数据
                     await GetOrganizationUnitRolesAndUsers(OrganizationUnit.Id);
                 }
                 else if (parameters.ContainsKey("Value"))
                 {
                     //编辑组织,获取组织对应的用户以及角色列表 
                     OrganizationUnit = parameters.GetValue<OrganizationListModel>("Value");
                     await GetOrganizationUnitRolesAndUsers(OrganizationUnit.Id);
                 }
                 else
                 {
                     if (parameters.ContainsKey("Id"))
                     {
                         //新建子组织时, 记录父组织ID, 用于记录子组织所在级别
                         ParentId = parameters.GetValue<long>("Id");
                     }

                     IsNewOrganization = true;

                     /*
                      * 如果是属于新增组织,禁用 用户角色\添加角色\用户按钮, 因为这两个功能需要确保组织已经被新建成功
                      * 这样方便与添加对应的角色和用户记录对应的ID
                      */
                     NewOrganizationIsVisible = false;
                     OrganizationUnit = new OrganizationListModel();
                 }
             });
        }

        #region 内部方法

        /// <summary>
        /// 获取导航参数
        /// </summary>
        /// <returns></returns>
        private INavigationParameters GetParameter()
        {
            return new NavigationParameters()
            {
                { "Value",OrganizationUnit }
            };
        }

        /// <summary>
        /// 获取组织机构的角色以及用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task GetOrganizationUnitRolesAndUsers(long id)
        {
            await WebRequest.Execute(async () =>
            {
                return await appService.GetOrganizationUnitRoles(new
                    GetOrganizationUnitRolesInput()
                { Id = id });
            }, result => GetOrganizationUnitRolesSuccessed(result));

            await WebRequest.Execute(async () =>
            {
                return await appService.GetOrganizationUnitUsers(new
                    GetOrganizationUnitUsersInput()
                { Id = id });
            }, result => GetOrganizationUnitUsersSuccessed(result));
        }

        /// <summary>
        /// 创建组织机构完成后
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <returns></returns>
        private async Task CreateOrganizationUnitSuccessed(OrganizationUnitDto organizationUnit)
        {
            if (organizationUnit == null)
            {
                await GoBackAsync();
                return;
            }

            //当组织新增完成之后,允许为该组织添加用户及角色
            OrganizationUnit.Id = organizationUnit.Id;
            OrganizationUnit.ParentId = ParentId;
            //显示角色和用户面板及相应操作按钮。
            NewOrganizationIsVisible = true;
        }

        /// <summary>
        /// 获取组织机构角色
        /// </summary>
        /// <param name="pagedResult"></param>
        /// <returns></returns>
        private async Task GetOrganizationUnitRolesSuccessed(PagedResultDto<OrganizationUnitRoleListDto> pagedResult)
        {
            if (pagedResult != null)
            {
                RolesModelList?.Clear();
                foreach (var item in pagedResult.Items)
                    RolesModelList.Add(item);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取组织结构用户
        /// </summary>
        /// <param name="pagedResult"></param>
        /// <returns></returns>
        private async Task GetOrganizationUnitUsersSuccessed(PagedResultDto<OrganizationUnitUserListDto> pagedResult)
        {
            if (pagedResult != null)
            {
                UserModelList?.Clear();
                foreach (var item in pagedResult.Items)
                    UserModelList.Add(item);
            }

            await Task.CompletedTask;
        }

        #endregion 内部方法
    }
}