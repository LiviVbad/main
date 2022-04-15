using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AppFramework.Common.Models
{
    public class OrganizationListModel : BindableBase
    {
        private bool isChecked;
        private string displayName;
        private int memberCount;
        private int roleCount;
        private ObservableCollection<OrganizationListModel> items;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<OrganizationListModel> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName=value; RaisePropertyChanged(); }
        }

        public int MemberCount
        {
            get { return memberCount; }
            set { memberCount=value; RaisePropertyChanged(); }
        }

        public int RoleCount
        {
            get { return roleCount; }
            set { roleCount=value; RaisePropertyChanged(); }
        }
    }
}