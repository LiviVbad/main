﻿using Abp.Application.Services.Dto;
using AppFrameworkDemo.Organizations;
using AppFrameworkDemo.Organizations.Dto;
using AppFramework.Application.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class AddUsersViewModel : NavigationViewModel
    {
        private readonly IOrganizationUnitAppService appService;
        public DelegateCommand SaveCommand { get; private set; }

        private OrganizationListModel organizationUnit;

        private ObservableCollection<ChooseItem> values;

        public ObservableCollection<ChooseItem> Values
        {
            get { return values; }
            set { values = value; RaisePropertyChanged(); }
        }

        public AddUsersViewModel(IOrganizationUnitAppService appService)
        {
            this.appService = appService;
            Values = new ObservableCollection<ChooseItem>();

            SaveCommand = new DelegateCommand(Save);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    organizationUnit = parameters.GetValue<OrganizationListModel>("Value");

                    await WebRequestRuner.Execute(async () =>
                    {
                        return await appService.FindUsers(new FindOrganizationUnitUsersInput()
                        {
                            OrganizationUnitId = organizationUnit.Id
                        });
                    }, result => FindUsersSuccessed(result));
                }
            });
        }

        public async void Save()
        {
            var userIds = Values.Where(q => q.IsSelected)
                  .Select(t => Convert.ToInt64(t.Value.Value))
                  .ToArray();

            if (userIds.Length == 0) return;

            var navigationParameter = new NavigationParameters();
            navigationParameter.Add("IsRefreshUserOrRoles", true);

            await SetBusyAsync((async () =>
            {
                await WebRequestRuner.Execute(async () =>
                 await appService.AddUsersToOrganizationUnit(new UsersToOrganizationUnitInput()
                 {
                     OrganizationUnitId = organizationUnit.Id,
                     UserIds = userIds,
                 }), async () => await navigationService.GoBackAsync(navigationParameter));
            }));
        }

        private async Task FindUsersSuccessed(PagedResultDto<NameValueDto> pagedResult)
        {
            Values?.Clear();
            foreach (var item in pagedResult.Items)
                Values.Add(new ChooseItem(item));

            await Task.CompletedTask;
        }
    }
}