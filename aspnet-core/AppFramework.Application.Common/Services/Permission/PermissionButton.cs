using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Common.Services.Permission
{
    public class PermissionButton : BindableBase
    {
        public PermissionButton(string key, string name)
        {
            Key = key;
            Name = name;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string Key { get; set; }
    }
}
