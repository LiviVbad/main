using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Ioc;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class OrganizationsViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        private OrganizationListModel SelectedOrganizationUnit;

        private readonly IOrganizationUnitAppService appService;
        public IDataPagerService roledataPager { get; private set; }
        public IDataPagerService memberdataPager { get; private set; }

        //选中组织、添加跟组织、修改、删除组织
        public DelegateCommand<OrganizationListModel> SelectedCommand { get; }
        public DelegateCommand<OrganizationListModel> AddRootUnitCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> ChangeCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> RemoveCommand { get; private set; }

        public DelegateCommand<string> ExecuteItemCommand { get; private set; }

        //删除成员、角色
        public DelegateCommand<OrganizationUnitUserListDto> DeleteMemberCommand { get; private set; }
        public DelegateCommand<OrganizationUnitRoleListDto> DeleteRoleCommand { get; private set; }

        #endregion

        public OrganizationsViewModel(IOrganizationUnitAppService userAppService)
        {
            this.appService = userAppService;
            SelectedCommand = new DelegateCommand<OrganizationListModel>(Selected);

            AddRootUnitCommand = new DelegateCommand<OrganizationListModel>(AddOrganizationUnit);
            ChangeCommand = new DelegateCommand<OrganizationListModel>(EditOrganizationUnit);
            RemoveCommand = new DelegateCommand<OrganizationListModel>(DeleteOrganizationUnit);

            DeleteRoleCommand = new DelegateCommand<OrganizationUnitRoleListDto>(DeleteRole);
            DeleteMemberCommand = new DelegateCommand<OrganizationUnitUserListDto>(DeleteMember);

            roledataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
            memberdataPager = ContainerLocator.Container.Resolve<IDataPagerService>();

            ExecuteItemCommand = new DelegateCommand<string>(ExecuteItem);
        }

        /// <summary>
        /// 选中组织机构-更新成员和角色信息
        /// </summary>
        /// <param name="organizationUnit"></param>
        private async void Selected(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            SelectedOrganizationUnit = organizationUnit;

            await RefreshUsers(organizationUnit.Id);
            await RefreshRoles(organizationUnit.Id);
        }

        public async void ExecuteItem(string arg)
        {
            switch (arg)
            {
                case "AddOrganizationUnit": AddOrganizationUnit(); break;
                case "AddMember": await AddMember(SelectedOrganizationUnit); break;
                case "AddRole": await AddRole(SelectedOrganizationUnit); break;
                case "Refresh": await RefreshAsync(); break;
            }
        }

        #region 组织机构

        /// <summary>
        /// 刷新组织结构树
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(
                      () => appService.GetOrganizationUnits(),
                      async result =>
                      {
                          var items = BuildOrganizationTree(Map<List<OrganizationListModel>>(result.Items));

                          foreach (var item in items)
                              dataPager.GridModelList.Add(item);

                          await Task.CompletedTask;
                      });
             });
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="organization"></param>
        public async void DeleteOrganizationUnit(OrganizationListModel organization)
        {
            if (await dialog.Question(Local.Localize("OrganizationUnitDeleteWarningMessage", organization.DisplayName)))
            {
                await WebRequest.Execute(() => appService.DeleteOrganizationUnit(new EntityDto<long>()
                {
                    Id = organization.Id
                }), RefreshAsync);
            }
        }

        /// <summary>
        /// 编辑组织机构
        /// </summary>
        /// <param name="organization"></param>
        public async void EditOrganizationUnit(OrganizationListModel organization)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", organization);
            var dialogResult = await dialog.ShowDialogAsync("OrganizationsAddView", param);
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        /// <summary>
        /// 新增组织机构
        /// </summary>
        /// <param name="organization"></param>
        public async void AddOrganizationUnit(OrganizationListModel organization = null)
        {
            DialogParameters param = new DialogParameters();
            if (organization != null) param.Add("ParentId", organization.Id);

            var dialogResult = await dialog.ShowDialogAsync("OrganizationsAddView", param);
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        private void RefreshOrganizationUnit(long id)
        {
            var organizationUnit = dataPager.GridModelList
                .FirstOrDefault(t => t is OrganizationListModel q && q.Id.Equals(id)) as OrganizationListModel;

            if (organizationUnit != null)
            {
                organizationUnit.MemberCount = memberdataPager.GridModelList.Count;
                organizationUnit.RoleCount = roledataPager.GridModelList.Count;
            }
        }

        public ObservableCollection<OrganizationListModel> BuildOrganizationTree(
            List<OrganizationListModel> organizationUnits, long? parentId = null)
        {
            var masters = organizationUnits
                .Where(x => x.ParentId == parentId).ToList();

            var childs = organizationUnits
                .Where(x => x.ParentId != parentId).ToList();

            foreach (OrganizationListModel dpt in masters)
                dpt.Items = BuildOrganizationTree(childs, dpt.Id);

            return new ObservableCollection<OrganizationListModel>(masters);
        }

        #endregion OrganizationUnit

        #region 角色

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <returns></returns>
        private async Task AddRole(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await WebRequest.Execute(() =>
                       appService.FindRoles(new FindOrganizationUnitRolesInput()
                       {
                           OrganizationUnitId = Id
                       }),
                       async result =>
                       {
                           DialogParameters param = new DialogParameters();
                           param.Add("Id", Id);
                           param.Add("Value", result);
                           var dialogResult = await dialog.ShowDialogAsync(AppViewManager.AddRoles, param);
                           if (dialogResult.Result == ButtonResult.OK)
                               await RefreshRoles(Id);
                       });
        }

        /// <summary>
        /// 刷新角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task RefreshRoles(long Id)
        {
            await SetBusyAsync(async () =>
              {
                  var pagedResult = await appService.GetOrganizationUnitRoles(new GetOrganizationUnitRolesInput() { Id = Id });
                  if (pagedResult != null)
                      roledataPager.SetList(pagedResult);

                  RefreshOrganizationUnit(Id);
              });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="obj"></param>
        private async void DeleteRole(OrganizationUnitRoleListDto obj)
        {
            if (await dialog.Question(Local.Localize("RemoveRoleFromOuWarningMessage",
                SelectedOrganizationUnit.DisplayName, obj.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() =>
                    appService.RemoveRoleFromOrganizationUnit(new RoleToOrganizationUnitInput()
                    {
                        RoleId = (int)obj.Id,
                        OrganizationUnitId = SelectedOrganizationUnit.Id,
                    }), async () =>
                    {
                        await RefreshRoles(SelectedOrganizationUnit.Id);
                    });
                });
            }
        }

        #endregion Roles

        #region 成员

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <returns></returns>
        private async Task AddMember(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await WebRequest.Execute(() =>
                        appService.FindUsers(new FindOrganizationUnitUsersInput()
                        {
                            OrganizationUnitId = Id
                        }),
                        async result =>
                        {
                            DialogParameters param = new DialogParameters();
                            param.Add("Id", Id);
                            param.Add("Value", result);
                            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.AddUsers, param);
                            if (dialogResult.Result == ButtonResult.OK)
                                await RefreshUsers(Id);
                        });
        }

        /// <summary>
        /// 刷新成员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task RefreshUsers(long Id)
        {
            await SetBusyAsync(async () =>
            {
                var pagedResult = await appService.GetOrganizationUnitUsers(new GetOrganizationUnitUsersInput() { Id = Id });
                if (pagedResult != null)
                    memberdataPager.SetList(pagedResult);

                RefreshOrganizationUnit(Id);
            });
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="obj"></param>
        private async void DeleteMember(OrganizationUnitUserListDto obj)
        {
            if (await dialog.Question(Local.Localize("RemoveUserFromOuWarningMessage",
                SelectedOrganizationUnit.DisplayName, obj.UserName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() =>
                    appService.RemoveUserFromOrganizationUnit(new UserToOrganizationUnitInput()
                    {
                        OrganizationUnitId = SelectedOrganizationUnit.Id,
                        UserId = obj.Id
                    }), async () =>
                    {
                        await RefreshUsers(SelectedOrganizationUnit.Id);
                    });
                });
            }
        }
        #endregion Users 
    }
}