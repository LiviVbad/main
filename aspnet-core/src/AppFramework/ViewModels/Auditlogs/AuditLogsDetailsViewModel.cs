using AppFramework.Common.Models;
using Prism.Services.Dialogs;
 
namespace AppFramework.ViewModels
{
    public class AuditLogsDetailsViewModel : HostDialogViewModel
    {
        private AuditLogListModel auditLog;

        public AuditLogListModel AuditLog
        {
            get { return auditLog; }
            set { auditLog = value; RaisePropertyChanged(); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                AuditLog = parameters.GetValue<AuditLogListModel>("Value");
            }
        }
    }
}
