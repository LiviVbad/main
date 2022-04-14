using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Common.Models
{
    public class FlatPermissionModel : BindableBase
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsGrantedByDefault { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<FlatPermissionModel> items;

        public ObservableCollection<FlatPermissionModel> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}