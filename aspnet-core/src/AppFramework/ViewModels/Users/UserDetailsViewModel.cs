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
                IsUnlockButtonVisible = !isNewUser && permissionService.HasPermission(Permkeys.UserEdit);
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

                  await WebRequest.Execute(async () =>
                          await userAppService.GetUserForEdit(new NullableIdDto<long>(userInfo?.Id)),
                          GetUserForEditSuccessed);
              });

            if (parameters.ContainsKey("Config"))
            {
                PasswordComplexitySetting = parameters.GetValue<GetPasswordComplexitySettingOutput>("Config");
            }
        }

        private async Task GetUserForEditSuccessed(GetUserForEditOutput output)
        {
            Model = Map<UserForEditModel>(output);
            Model.OrganizationUnits = Map<List<OrganizationUnitModel>>(output.AllOrganizationUnits);

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

            await Task.CompletedTask;
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