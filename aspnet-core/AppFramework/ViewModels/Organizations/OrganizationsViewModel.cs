using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Localization;
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
        private OrganizationUnitModel SelectedOrganizationUnit;

        private readonly IOrganizationUnitAppService appService;

        public DelegateCommand<OrganizationUnitModel> SelectedCommand { get; }

        public OrganizationsViewModel(IOrganizationUnitAppService userAppService)
        {
            this.appService = userAppService;
            SelectedCommand = new DelegateCommand<OrganizationUnitModel>(Selected);
            UserModelList = new ObservableCollection<OrganizationUnitUserListDto>();
            RolesModelList = new ObservableCollection<OrganizationUnitRoleListDto>();
        }

        private async void Selected(OrganizationUnitModel organizationUnit)
        {
            if (organizationUnit == null) return;

            SelectedOrganizationUnit = organizationUnit;

            await RefreshUsers(organizationUnit.Id);
            await RefreshRoles(organizationUnit.Id);
        }

        protected async void Execute(string arg)
        {
            switch (arg)
            {
                case "AddOrganizationUnit": await AddOrganizationUnit(); break;
                case "AddMember": await AddMember(SelectedOrganizationUnit); break;
                case "AddRole": await AddRole(SelectedOrganizationUnit); break;
                case "Refresh": await RefreshAsync(); break;
                case "Add": await dialogHostService.ShowDialogAsync("UserAddView"); break;
                case "Exprot": break;
                case "Filter": break;
            }
        }

        #region OrganizationUnit

        public override async Task RefreshAsync()
        {
            await WebRequest.Execute(
                      () => appService.GetOrganizationUnits(),
                      result => Successed(result),
                      ex => Task.CompletedTask);
        }

        public async Task DeleteOrganizationUnit(OrganizationUnitModel organizationUnit)
        {
            var result = await dialogHostService
                .Question(Local.Localize("OrganizationUnitDeleteWarningMessage", organizationUnit.DisplayName));
            if (result)
            {
                await WebRequest.Execute(
                    () => appService.DeleteOrganizationUnit(new EntityDto<long>()
                    {
                        Id = organizationUnit.Id
                    }),
                    () => RefreshAsync(),
                    ex => Task.CompletedTask);
            }
        }

        public async Task UpdateRootUnit(OrganizationUnitModel organizationUnit)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", organizationUnit);

            var dialogResult = await dialogHostService.ShowDialogAsync("", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var input = dialogResult.Parameters.GetValue<OrganizationUnitModel>("Value");

                await WebRequest.Execute(
                       () =>
                       appService.UpdateOrganizationUnit(new UpdateOrganizationUnitInput()
                       {
                           DisplayName = input.DisplayName
                       }),
                       result => RefreshAsync(),
                       ex => Task.CompletedTask);
            }
        }

        public async Task AddOrganizationUnit(long? parentId = null)
        {
            var dialogResult = await dialogHostService.ShowDialogAsync("");
            if (dialogResult.Result == ButtonResult.OK)
            {
                var input = dialogResult.Parameters.GetValue<OrganizationUnitModel>("Value");
                await WebRequest.Execute(
                       () =>
                       appService.CreateOrganizationUnit(new CreateOrganizationUnitInput()
                       {
                           DisplayName = input.DisplayName,
                           ParentId = parentId
                       }),
                       result => RefreshAsync(),
                       ex => Task.CompletedTask);
            }
        }

        private async Task Successed(ListResultDto<OrganizationUnitDto> pagedResult)
        {
            GridModelList.Clear();

            var items = Map<List<OrganizationListModel>>(pagedResult.Items);
            foreach (var item in BuildOrganizationTree(items))
                GridModelList.Add(item);

            await Task.CompletedTask;
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

        #region Roles

        private ObservableCollection<OrganizationUnitRoleListDto> rolesModelList;

        public ObservableCollection<OrganizationUnitRoleListDto> RolesModelList
        {
            get { return rolesModelList; }
            set { rolesModelList = value; }
        }

        private async Task AddRole(OrganizationUnitModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await WebRequest.Execute(
                       () => appService.FindRoles(new FindOrganizationUnitRolesInput()
                       {
                           OrganizationUnitId = Id
                       }),
                       result => FinRolesSuccessed(Id, result),
                       ex => Task.CompletedTask);
        }

        private async Task FinRolesSuccessed(long Id, PagedResultDto<NameValueDto> pagedResult)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Id", Id);
            param.Add("Value", pagedResult);
            var dialogResult = await dialogHostService.ShowDialogAsync("", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var input = dialogResult.Parameters.GetValue<RolesToOrganizationUnitInput>("Value");
                await WebRequest.Execute(
                    () => appService.AddRolesToOrganizationUnit(input),
                    () => RefreshRoles(Id));

                RefreshOrganizationUnit(Id);
            }
        }

        private async Task RefreshRoles(long Id)
        {
            await SetBusyAsync(
              async () =>
              {
                  var pagedResult = await appService
                  .GetOrganizationUnitRoles(new GetOrganizationUnitRolesInput() { Id = Id });
                  if (pagedResult != null)
                  {
                      RolesModelList?.Clear();
                      foreach (var item in pagedResult.Items)
                          RolesModelList.Add(item);
                  }
              });
        }

        #endregion Roles

        #region Users

        private ObservableCollection<OrganizationUnitUserListDto> userModelList;

        public ObservableCollection<OrganizationUnitUserListDto> UserModelList
        {
            get { return userModelList; }
            set { userModelList = value; }
        }

        private async Task AddMember(OrganizationUnitModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await WebRequest.Execute(
                       () => appService.FindUsers(new FindOrganizationUnitUsersInput()
                       {
                           OrganizationUnitId = Id
                       }),
                       result => FinUsersSuccessed(Id, result));
        }

        private async Task FinUsersSuccessed(long Id, PagedResultDto<NameValueDto> pagedResult)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Id", Id);
            param.Add("Value", pagedResult);
            var dialogResult = await dialogHostService.ShowDialogAsync("", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var input = dialogResult.Parameters.GetValue<UsersToOrganizationUnitInput>("Value");
                await WebRequest.Execute(
                    () => appService.AddUsersToOrganizationUnit(input),
                    () => RefreshUsers(Id));

                RefreshOrganizationUnit(Id);
            }
        }

        private async Task RefreshUsers(long Id)
        {
            await SetBusyAsync(async () =>
            {
                var pagedResult = await appService
                    .GetOrganizationUnitUsers(new GetOrganizationUnitUsersInput() { Id = Id });
                if (pagedResult != null)
                {
                    UserModelList?.Clear();
                    foreach (var item in pagedResult.Items)
                        UserModelList.Add(item);
                }
            });
        }

        #endregion Users

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await RefreshAsync();
        }
    }
}