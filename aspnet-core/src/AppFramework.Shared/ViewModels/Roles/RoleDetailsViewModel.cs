using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using Prism.Commands;
using Prism.Navigation; 
using System.Threading.Tasks;
using AppFramework.Common.Core;
using AppFramework.Common.Models;
using AppFramework.Common; 

namespace AppFramework.Shared.ViewModels
{
    public class RoleDetailsViewModel : NavigationViewModel
    {
        #region 字段/属性

        public IPermissionTreesService treesService { get; set; }
        private readonly IRoleAppService appService;
        private readonly IMessenger messenger;
        private RoleEditModel role;

        public RoleEditModel Role
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SaveCommand { get; private set; }
        
        #endregion 字段/属性

        public RoleDetailsViewModel(IRoleAppService appService,
            IMessenger messenger,
            IPermissionTreesService treesService)
        {
            this.appService = appService;
            this.messenger = messenger;
            this.treesService = treesService;
            SaveCommand = new DelegateCommand(Save);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    await appService.CreateOrUpdateRole(new CreateOrUpdateRoleInput()
                    {
                        Role = Map<RoleEditDto>(Role),
                        GrantedPermissionNames = treesService.GetSelectedItems()
                    });
                },
                async () => await GoBackAsync());
            });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Role);//通知列表更新
            await base.GoBackAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                int? id = null;

                if (parameters != null && parameters.ContainsKey("Value"))
                    id = parameters.GetValue<RoleListDto>("Value").Id;
                var output = await appService.GetRoleForEdit(new NullableIdDto(id));
                Role = Map<RoleEditModel>(output.Role);
                treesService.CreatePermissionTrees(output.Permissions, output.GrantedPermissionNames);
            });
        }
    }
}