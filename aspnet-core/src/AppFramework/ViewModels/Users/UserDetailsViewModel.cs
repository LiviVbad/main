using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class UserDetailsViewModel : HostDialogViewModel
    {
        public UserDetailsViewModel(IUserAppService userAppService,
            IPermissionService permissionService)
        {
            Input = new UserCreateOrUpdateModel();
            this.userAppService = userAppService;
            this.permissionService = permissionService;
        }

        private bool isNewUser;
        private bool isUnlockButtonVisible;
        private UserForEditModel model;

        private UserCreateOrUpdateModel input;

        public UserCreateOrUpdateModel Input
        {
            get { return input; }
            set { input = value; RaisePropertyChanged(); }
        }

        private GetPasswordComplexitySettingOutput PasswordComplexitySetting;
        private readonly IUserAppService userAppService;
        private readonly IPermissionService permissionService;

        public bool IsUnlockButtonVisible
        {
            get => isUnlockButtonVisible;
            set
            {
                isUnlockButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNewUser
        {
            get => isNewUser;
            set
            {
                isNewUser = value;
                IsUnlockButtonVisible = !isNewUser && permissionService.HasPermission(PermissionKey.UserEdit);
                RaisePropertyChanged();
            }
        }

        public UserForEditModel Model
        {
            get => model;
            set
            {
                model = value;
                RaisePropertyChanged();
            }
        }

        protected override async void Save()
        {
            Input.User = Model.User;
            Input.AssignedRoleNames = Model.Roles.Where(x => !x.IsAssigned).Select(x => x.RoleName).ToArray();
            Input.OrganizationUnits = Model.OrganizationUnits.Where(x => x.IsAssigned).Select(x => x.Id).ToList();

            if (!Verify(Input).IsValid) return;

            await SetBusyAsync(async () =>
            {
                var input = Map<CreateOrUpdateUserInput>(Input);
                await WebRequest.Execute(async () =>
                    await userAppService.CreateOrUpdateUser(input),
                    async () =>
                    {
                        base.Save();
                        await Task.CompletedTask;
                    });
            }, AppLocalizationKeys.SavingWithThreeDot);
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
              {
                  UserListModel userInfo = null;
                  if (parameters.ContainsKey("Value"))
                      userInfo = parameters.GetValue<UserListModel>("Value");

                  IsNewUser = userInfo == null;
                  Input.SetRandomPassword = IsNewUser;
                  Input.SendActivationEmail = IsNewUser;

                  var user = await userAppService.GetUserForEdit(new NullableIdDto<long>(userInfo?.Id));
                  Model = Map<UserForEditModel>(user);
                  Model.OrganizationUnits = Map<List<OrganizationUnitModel>>(user.AllOrganizationUnits);

                  if (IsNewUser)
                  {
                      //Model.Photo = ImageSource.FromResource(AssetsHelper.ProfileImagePlaceholderNamespace);
                      Model.User = new UserEditModel
                      {
                          IsActive = true,
                          IsLockoutEnabled = true,
                          ShouldChangePasswordOnNextLogin = true,
                      };
                  }
              });

            //if (parameters.ContainsKey("Value"))
            //{
            //    var output = parameters.GetValue<GetUserForEditOutput>("Value");

            //    UserInput.User = mapper.Map<UserEditModel>(output.User);
            //    var organizationUnitModels = mapper.Map<List<OrganizationListModel>>
            //       (output.AllOrganizationUnits);

            //    UserRoles = mapper.Map<ObservableCollection<UserRoleModel>>(output.Roles);
            //    OrganizationUnitModels = BuildOrganizationTree(organizationUnitModels);
            //}

            if (parameters.ContainsKey("Config"))
            {
                PasswordComplexitySetting = parameters.GetValue<GetPasswordComplexitySettingOutput>("Config");
            }
        }

        private ObservableCollection<OrganizationListModel> BuildOrganizationTree(
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