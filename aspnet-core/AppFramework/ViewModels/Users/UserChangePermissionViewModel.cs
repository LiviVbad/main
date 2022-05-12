using AppFramework.Authorization.Users.Dto;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using AppFramework.Common.Services.Permission;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppFramework.Authorization.Users;
using AppFramework.Common;

namespace AppFramework.ViewModels
{
    public class UserChangePermissionViewModel : HostDialogViewModel
    {
        public UserChangePermissionViewModel(IUserAppService userAppService)
        {
            this.userAppService = userAppService;
        }

        private long Id;
        private ObservableCollection<FlatPermissionModel> permissions;
        private readonly IUserAppService userAppService;

        public ObservableCollection<FlatPermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    List<string> GrantedPermissionNames = new List<string>();
                    Permissions.GetSelectedNodes(ref GrantedPermissionNames);
                    await userAppService.UpdateUserPermissions(new UpdateUserPermissionsInput()
                    {
                        Id = Id,
                        GrantedPermissionNames = GrantedPermissionNames
                    });
                }, async () =>
                {
                    base.Save();
                    await Task.CompletedTask;
                });
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    var output = parameters.GetValue<GetUserPermissionsForEditOutput>("Value");

                    var flats = Map<List<FlatPermissionModel>>(output.Permissions);
                    Permissions = flats.CreateTrees(null);
                    Permissions.UpdateSelectedNodes(output.GrantedPermissionNames);
                }

                if (parameters.ContainsKey("Id"))
                    Id = parameters.GetValue<long>("Id");

                await Task.CompletedTask;
            });
        }
    }
}
