using Abp.Application.Services.Dto;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using System.Linq;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using AppFramework.Common.Models;
using AppFramework.Common;

namespace AppFramework.ViewModels
{
    public class RoleChooseViewModel : HostDialogViewModel
    {
        public DelegateCommand QueryCommand { get; }

        private long Id;

        private string filter;

        public string Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ChooseItem> values;
        private readonly IOrganizationUnitAppService appService;

        public ObservableCollection<ChooseItem> Values
        {
            get { return values; }
            set { values = value; RaisePropertyChanged(); }
        }

        public RoleChooseViewModel(IOrganizationUnitAppService appService)
        {
            QueryCommand = new DelegateCommand(Query);
            this.appService = appService;
        }

        private async void Query()
        {
            await WebRequest.Execute(() =>
             appService.FindRoles(new FindOrganizationUnitRolesInput()
             {
                 OrganizationUnitId = Id,
                 Filter = Filter
             }),
             result => FindUsersSuccessed(result));
        }

        async Task FindUsersSuccessed(PagedResultDto<NameValueDto> pagedResult)
        {
            if (pagedResult == null) return;

            Values?.Clear();

            foreach (var item in pagedResult.Items)
                Values.Add(new ChooseItem(item));

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Id = parameters.GetValue<long>("Id");
                var pagedResult = parameters.GetValue<PagedResultDto<NameValueDto>>("Value");

                Values = new ObservableCollection<ChooseItem>();
                foreach (var item in pagedResult.Items)
                    Values.Add(new ChooseItem(item));
            }
            else
                Values = new ObservableCollection<ChooseItem>();
        }

        protected override void Save()
        {
            var roleIds = Values.Where(q => q.IsSelected)?
                .Select(t => Convert.ToInt32(t.Value.Value))
                .ToArray();

            base.Save(new RolesToOrganizationUnitInput()
            {
                OrganizationUnitId = Id,
                RoleIds = roleIds
            });
        }
    }
}
