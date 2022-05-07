using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class OrganizationsViewModel : NavigationCurdViewModel<OrganizationListModel>
    {
        #region 字段/属性

        private OrganizationListModel SelectedOrganizationUnit;

        private readonly IOrganizationUnitAppService appService;

        private ObservableCollection<OrganizationUnitRoleListDto> rolesModelList;

        public ObservableCollection<OrganizationUnitRoleListDto> RolesModelList
        {
            get { return rolesModelList; }
            set { rolesModelList = value; }
        }

        private ObservableCollection<OrganizationUnitUserListDto> userModelList;

        public ObservableCollection<OrganizationUnitUserListDto> UserModelList
        {
            get { return userModelList; }
            set { userModelList = value; }
        }

        public DelegateCommand<OrganizationListModel> SelectedCommand { get; }
        public DelegateCommand<OrganizationListModel> AddRootUnitCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> ChangeCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> RemoveCommand { get; private set; }

        #endregion

        public OrganizationsViewModel(IOrganizationUnitAppService userAppService)
        {
            this.appService = userAppService;
            SelectedCommand = new DelegateCommand<OrganizationListModel>(Selected);
            UserModelList = new ObservableCollection<OrganizationUnitUserListDto>();
            RolesModelList = new ObservableCollection<OrganizationUnitRoleListDto>();

            AddRootUnitCommand = new DelegateCommand<OrganizationListModel>(AddOrganizationUnit);
            ChangeCommand = new DelegateCommand<OrganizationListModel>(EditOrganizationUnit);
            RemoveCommand = new DelegateCommand<OrganizationListModel>(DeleteOrganizationUnit);
        }

        private async void Selected(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            SelectedOrganizationUnit = organizationUnit;

            await RefreshUsers(organizationUnit.Id);
            await RefreshRoles(organizationUnit.Id);
        }

        public override async void Execute(string arg)
        {
            switch (arg)
            {
                case "AddOrganizationUnit": AddOrganizationUnit(null); break;
                case "AddMember": await AddMember(SelectedOrganizationUnit); break;
                case "AddRole": await AddRole(SelectedOrganizationUnit); break;
                case "Refresh": await RefreshAsync(); break;
            }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        #region 组织机构

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(
                      () => appService.GetOrganizationUnits(),
                      async result =>
                      {
                          GridModelList.Clear();

                          var items = BuildOrganizationTree(Map<List<OrganizationListModel>>(result.Items));

                          foreach (var item in items)
                          {
                              GridModelList.Add(item);
                          }

                          await Task.CompletedTask;
                      });
             });
        }

        public async void DeleteOrganizationUnit(OrganizationListModel organization)
        {
            var result = await dialog.Question(Local.Localize("OrganizationUnitDeleteWarningMessage", organization.DisplayName));
            if (result)
            {
                await WebRequest.Execute(
                    () => appService.DeleteOrganizationUnit(new EntityDto<long>()
                    {
                        Id = organization.Id
                    }), () => RefreshAsync());
            }
        }

        public async void EditOrganizationUnit(OrganizationListModel organization)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", organization);
            var dialogResult = await dialog.ShowDialogAsync("OrganizationsAddView", param);
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        public async void AddOrganizationUnit(OrganizationListModel organization)
        {
            DialogParameters param = new DialogParameters();
            if (organization != null) param.Add("ParentId", organization.Id);

            var dialogResult = await dialog.ShowDialogAsync("OrganizationsAddView", param);
            if (dialogResult.Result == ButtonResult.OK)
                await RefreshAsync();
        }

        private void RefreshOrganizationUnit(long id)
        {
            var organizationUnit = GridModelList.FirstOrDefault(t => t.Id.Equals(id));

            if (organizationUnit != null)
            {
                organizationUnit.MemberCount = UserModelList.Count;
                organizationUnit.RoleCount = RolesModelList.Count;
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

        private async Task AddRole(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await WebRequest.Execute(() =>
                       appService.FindRoles(
                       new FindOrganizationUnitRolesInput()
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
                           {
                               await RefreshRoles(Id);
                               RefreshOrganizationUnit(Id);
                           }
                       });
        }

        private async Task RefreshRoles(long Id)
        {
            await SetBusyAsync(async () =>
              {
                  var pagedResult = await appService.GetOrganizationUnitRoles(
                      new GetOrganizationUnitRolesInput() { Id = Id });
                  if (pagedResult != null)
                  {
                      RolesModelList?.Clear();
                      foreach (var item in pagedResult.Items)
                          RolesModelList.Add(item);
                  }
              });
        }

        #endregion Roles

        #region 用户

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
                {
                    await RefreshUsers(Id);
                    RefreshOrganizationUnit(Id);
                }
            });
        }

        private async Task RefreshUsers(long Id)
        {
            await SetBusyAsync(async () =>
            {
                var pagedResult = await appService.GetOrganizationUnitUsers(
                    new GetOrganizationUnitUsersInput() { Id = Id });
                if (pagedResult != null)
                {
                    UserModelList?.Clear();
                    foreach (var item in pagedResult.Items)
                        UserModelList.Add(item);
                }
            });
        }

        #endregion Users 
    }
}