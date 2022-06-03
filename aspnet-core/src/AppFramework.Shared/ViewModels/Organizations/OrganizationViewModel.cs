namespace AppFramework.Shared.ViewModels
{
    using Abp.Application.Services.Dto;
    using AppFramework.Organizations;
    using AppFramework.Organizations.Dto;
    using Prism.Commands;
    using Prism.Navigation;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AppFramework.Common.Models;
    using AppFramework.Common.Core;
    using AppFramework.Common;

    public class OrganizationViewModel : RegionCurdViewModel
    {
        private readonly IOrganizationUnitAppService appService;

        public DelegateCommand<OrganizationListModel> AddRoleCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> AddUserCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> AddSubUnitCommand { get; private set; }

        public OrganizationViewModel(IOrganizationUnitAppService appService, IMessenger messenger)
        {
            this.appService = appService;
            messenger.Sub(AppMessengerKeys.Organization, async () => await RefreshAsync());
            AddSubUnitCommand = new DelegateCommand<OrganizationListModel>(AddSubUnit);
        }

        private async void AddSubUnit(OrganizationListModel obj)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Id", obj.Id);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public override async void Delete(object selectedItem)
        {
            if (selectedItem is OrganizationListModel item)
            {
                if (!await dialogService.DeleteConfirm()) return;

                await appService.DeleteOrganizationUnit(new EntityDto<long>()
                {
                    Id = item.Id
                });
                await RefreshAsync();
            }
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(() => appService.GetOrganizationUnits(), RefreshSuccessed);
            });
        }

        protected virtual async Task RefreshSuccessed(ListResultDto<OrganizationUnitDto> pagedResult)
        {
            var organizationUnits = pagedResult.Items?.Select(t => new OrganizationListModel
            {
                Id = t.Id,
                Code = t.Code,
                ParentId = t.ParentId,
                RoleCount = t.RoleCount,
                DisplayName = t.DisplayName,
                MemberCount = t.MemberCount,
            }).ToList();

            GridModelList = BuildOrganizationTree(organizationUnits);

            await Task.CompletedTask;
        }

        public ObservableCollection<object> BuildOrganizationTree(
           List<OrganizationListModel> organizationUnits, long? parentId = null)
        {
            var masters = organizationUnits
                .Where(x => x.ParentId == parentId).ToList();

            var childs = organizationUnits
                .Where(x => x.ParentId != parentId).ToList();

            foreach (OrganizationListModel dpt in masters)
                dpt.Items = BuildOrganizationTree(childs, dpt.Id);

            return new ObservableCollection<object>(masters);
        }
    }
}