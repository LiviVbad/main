using AppFramework.Authorization.Users.Dto;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using AppFramework.Common.Services.Permission;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class UserChangePermissionViewModel : HostDialogViewModel
    {
        private ObservableCollection<FlatPermissionModel> permissions;

        public ObservableCollection<FlatPermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
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
                await Task.CompletedTask;
            });
        }
    }
}
