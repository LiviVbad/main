using AppFramework.ViewModels.Shared;
using Prism.Services.Dialogs;
using System; 

namespace AppFramework.ViewModels
{
    public class SelectDateRangeViewModel : HostDialogViewModel
    {
        private DateTime? startDate;

        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; RaisePropertyChanged(); }
        }

        private DateTime? endDate;

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; RaisePropertyChanged(); }
        }

        protected override void Save()
        {
            if (StartDate == null || EndDate == null) return;

            DialogParameters param = new DialogParameters();
            param.Add("StartDate", StartDate);
            param.Add("EndDate", EndDate);
            base.Save(param);
        }

        public override void OnDialogOpened(IDialogParameters parameters) { }
    }
}
