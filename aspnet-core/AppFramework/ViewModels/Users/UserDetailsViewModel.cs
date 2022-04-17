using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Services.Dialogs;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common.Models;

namespace AppFramework.ViewModels
{
    public class UserDetailsViewModel : HostDialogViewModel
    {
        public UserDetailsViewModel()
        {
            Input = new UserCreateOrUpdateModel();
        }

        private UserCreateOrUpdateModel input;

        public UserCreateOrUpdateModel Input
        {
            get { return input; }
            set { input = value; RaisePropertyChanged(); }
        }

        private GetPasswordComplexitySettingOutput PasswordComplexitySetting;
        public ObservableCollection<UserRoleModel> UserRoles { get; set; }
        private ObservableCollection<OrganizationListModel> organizationUnitModels;

        public ObservableCollection<OrganizationListModel> OrganizationUnitModels
        {
            get { return organizationUnitModels; }
            set { organizationUnitModels = value; RaisePropertyChanged(); }
        }

        protected override void Save()
        {
            if (!Verify(Input.User)||!Verify(Input)) return;

            Input.AssignedRoleNames = UserRoles
                .Where(q => q.IsChecked.Equals(true))
                .Select(t => t.RoleName).ToArray();

            Input.OrganizationUnits = OrganizationUnitModels
                .Where(q => q.IsChecked.Equals(true))
                .Select(t => t.Id).ToList();

            var output = mapper.Map<CreateOrUpdateUserInput>(Input);
            base.Save(output);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                var output = parameters.GetValue<GetUserForEditOutput>("Value");

                Input.User = mapper.Map<UserEditModel>(output.User);
                var organizationUnitModels = mapper.Map<List<OrganizationListModel>>
                   (output.AllOrganizationUnits);

                UserRoles = mapper.Map<ObservableCollection<UserRoleModel>>(output.Roles);
                OrganizationUnitModels = BuildOrganizationTree(organizationUnitModels);
            }

            if (parameters.ContainsKey("Config"))
            {
                PasswordComplexitySetting = parameters.GetValue<GetPasswordComplexitySettingOutput>("Config");
            }
        }

        ObservableCollection<OrganizationListModel> BuildOrganizationTree(
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
    }
}
