﻿using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class AddUsersViewModel : HostDialogViewModel
    {
        #region 字段/属性

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

        public DelegateCommand QueryCommand { get; private set; }

        #endregion

        public AddUsersViewModel(IOrganizationUnitAppService appService)
        {
            QueryCommand = new DelegateCommand(Query);
            this.appService = appService;
        }

        protected override async void Save()
        {
            var userIds = Values.Where(q => q.IsSelected)?
                .Select(t => Convert.ToInt64(t.Value.Value))
                .ToArray();

            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.AddUsersToOrganizationUnit(
                    new UsersToOrganizationUnitInput()
                    {
                        OrganizationUnitId = Id,
                        UserIds = userIds
                    }), () =>
                    {
                        base.Save();
                        return Task.CompletedTask;
                    });
            });
        }

        private async void Query()
        {
            await WebRequest.Execute(() =>
             appService.FindUsers(new FindOrganizationUnitUsersInput()
             {
                 OrganizationUnitId = Id,
                 Filter = Filter
             }),
             result => FindUsersSuccessed(result));
        }

        private async Task FindUsersSuccessed(PagedResultDto<NameValueDto> pagedResult)
        {
            if (pagedResult == null) return;

            Values.Clear();

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
    }
}