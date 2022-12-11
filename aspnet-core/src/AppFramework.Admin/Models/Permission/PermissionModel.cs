using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Admin.Models
{
    public class PermissionModel : BindableBase
    {
        public PermissionModel? Parent { get; set; }

        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsGrantedByDefault { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetIsChecked(value, true, true);
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<PermissionModel> items;

        public ObservableCollection<PermissionModel> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        private void SetIsChecked(bool value, bool checkedChildren, bool checkedParent)
        {
            if (isChecked == value) return;
            isChecked = value;
             
            if (checkedChildren && Items != null)
            {
                for (int i = 0; i < Items.Count; i++)
                    Items[i].SetIsChecked(value, true, false);
            }
             
            if (checkedParent && this.Parent!=null)
                this.Parent.CheckParentIsCheckedState();

            RaisePropertyChanged(nameof(IsChecked));
        }
         
        private void CheckParentIsCheckedState()
        {
            List<PermissionModel> checkedItems = new List<PermissionModel>();
            string checkedNames = string.Empty;
            bool currentState = this.IsChecked;
            bool firstState = false;
            for (int i = 0; i < this.Items.Count; i++)
            {
                bool childrenState = this.Items[i].IsChecked;
                if (i == 0)
                    firstState = childrenState;
                else if (firstState != childrenState)
                    firstState = false;
            }

            if (!firstState) currentState = firstState;
            SetIsChecked(firstState, false, true);
        }
    }
}