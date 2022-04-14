using Prism.Commands; 
using System; 
using System.Collections.ObjectModel;
using System.Linq; 
using System.Threading.Tasks; 
using Prism.Services.Dialogs;
using AppFrameworkDemo.Organizations;
using AppFrameworkDemo.Organizations.Dto;
using Abp.Application.Services.Dto;
using AppFramework.Application.Common.Models;

namespace AppFramework.ViewModels
{
    public class UserChooseViewModel : HostDialogViewModel
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

        public UserChooseViewModel(IOrganizationUnitAppService appService)
        {
            QueryCommand = new DelegateCommand(Query);
            this.appService = appService;
        }

        private async void Query()
        {
            await WebRuner.Execute(() =>
             appService.FindUsers(new FindOrganizationUnitUsersInput()
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
            var userIds = Values.Where(q => q.IsSelected)?
                .Select(t => Convert.ToInt64(t.Value.Value))
                .ToArray();

            base.Save(new UsersToOrganizationUnitInput()
            {
                OrganizationUnitId = Id,
                UserIds = userIds
            });
        }
    }
}
