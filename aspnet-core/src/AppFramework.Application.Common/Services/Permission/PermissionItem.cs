using Prism.Mvvm;
using System; 

namespace AppFramework.Common.Services.Permission
{
    public class PermissionItem : BindableBase
    {
        public PermissionItem(string key, string name, Action ation)
        {
            Key = key;
            Name = name;
            Ation = ation;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public Action Ation { get; }
        public string Key { get; set; }
    }
}
