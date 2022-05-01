using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class OrganizationsAddViewModel : HostDialogViewModel
    {
        public OrganizationsAddViewModel(IOrganizationUnitAppService appService)
        {
            this.appService = appService;
        }

        private long? ParentId;
        private OrganizationListModel input;
        private readonly IOrganizationUnitAppService appService;

        public OrganizationListModel Input
        {
            get { return input; }
            set { input = value; RaisePropertyChanged(); }
        }

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(() => appService.CreateOrganizationUnit(
                     new CreateOrganizationUnitInput()
                     {
                         DisplayName = input.DisplayName,
                         ParentId = ParentId
                     }));
             });
            base.Save();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
                Input = parameters.GetValue<OrganizationListModel>("Value");
            else
                Input = new OrganizationListModel();

            if (parameters.ContainsKey("ParentId"))
                ParentId = parameters.GetValue<long>("ParentId");
        }
    }
}