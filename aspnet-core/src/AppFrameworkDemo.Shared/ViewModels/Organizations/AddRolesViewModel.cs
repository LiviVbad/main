﻿using Abp.Application.Services.Dto;
using AppFrameworkDemo.Organizations;
using AppFrameworkDemo.Organizations.Dto;
using AppFrameworkDemo.Shared.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.ViewModels
{
    public class AddRolesViewModel : NavigationViewModel
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

        public AddRolesViewModel(IOrganizationUnitAppService appService)
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
                        return await appService.FindRoles(new FindOrganizationUnitRolesInput()
                        {
                            OrganizationUnitId = organizationUnit.Id
                        });
                    }, result => FindRolesSuccessed(result));
                }
            });
        }

        public async void Save()
        {
            var roleIds = Values.Where(q => q.IsSelected)
                  .Select(t => Convert.ToInt32(t.Value.Value))
                  .ToArray();

            if (roleIds.Length == 0) return;

            var navigationParameter = new NavigationParameters();
            navigationParameter.Add("IsRefreshUserOrRoles", true);

            await SetBusyAsync(async () =>
            {
                await WebRequestRuner.Execute(async () =>
                 await appService.AddRolesToOrganizationUnit(new RolesToOrganizationUnitInput()
                 {
                     OrganizationUnitId = organizationUnit.Id,
                     RoleIds = roleIds,
                 }), async () => await navigationService.GoBackAsync(navigationParameter));
            });
        }

        private async Task FindRolesSuccessed(PagedResultDto<NameValueDto> pagedResult)
        {
            Values?.Clear();
            foreach (var item in pagedResult.Items)
                Values.Add(new ChooseItem(item));

            await Task.CompletedTask;
        }
    }
}