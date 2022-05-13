using AppFramework.Authorization.Users.Dto;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using AppFramework.Common.Services.Permission;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppFramework.Authorization.Users;
using AppFramework.Common;
using System.Linq;

namespace AppFramework.ViewModels
{
    public class UserChangePermissionViewModel : HostDialogViewModel
    {
        public UserChangePermissionViewModel(IUserAppService userAppService)
        {
            this.userAppService = userAppService;
        }

        private long Id;
        private ObservableCollection<PermissionModel> permissions;
        private readonly IUserAppService userAppService;

        public ObservableCollection<PermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<object> selectedItems;

        public ObservableCollection<object> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
        }

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    var gps = SelectedItems.Select(t => (t as PermissionModel)?.Name).ToList();
                    await userAppService.UpdateUserPermissions(new UpdateUserPermissionsInput()
                    {
                        Id = Id,
                        GrantedPermissionNames = gps
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

                    var flats = Map<List<PermissionModel>>(output.Permissions);
                    Permissions = flats.CreateTrees(null);
                    SelectedItems = Permissions.GetSelectedItems(output.GrantedPermissionNames);
                }

                if (parameters.ContainsKey("Id"))
                    Id = parameters.GetValue<long>("Id");

                await Task.CompletedTask;
            });
        }
    }
}
