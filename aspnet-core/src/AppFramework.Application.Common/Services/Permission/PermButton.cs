using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Common.Services.Permission
{
    public class PermButton : BindableBase
    {
        public PermButton(string key, string name, Action ation)
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
