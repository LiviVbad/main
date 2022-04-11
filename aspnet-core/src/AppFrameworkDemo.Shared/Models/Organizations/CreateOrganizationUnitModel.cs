﻿using Prism.Mvvm;

namespace AppFrameworkDemo.Shared.Models
{
    public class CreateOrganizationUnitModel : BindableBase
    {
        private string displayName;

        public long? ParentId { get; set; }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; RaisePropertyChanged(); }
        }
    }
}