using AppFramework.Common.Models;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class OrganizationsAddViewModel : HostDialogViewModel
    {
        private OrganizationListModel input;

        public OrganizationListModel Input
        {
            get { return input; }
            set { input = value; RaisePropertyChanged(); }
        }

        protected override void Save()
        {
            base.Save(Input);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
                Input = parameters.GetValue<OrganizationListModel>("Value");
            else
                Input = new OrganizationListModel();
        }
    }
}
