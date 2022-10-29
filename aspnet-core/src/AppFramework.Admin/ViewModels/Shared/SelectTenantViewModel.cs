﻿using AppFramework.Shared;
using Prism.Services.Dialogs; 

namespace AppFramework.ViewModels.Shared
{
    public class SelectTenantViewModel : HostDialogViewModel
    {
        private string tenancyName = string.Empty;
        private bool isLoginForTenants;

        public string TenancyName
        {
            get { return tenancyName; }
            set { tenancyName = value; RaisePropertyChanged(); }
        }

        public bool IsLoginForTenants
        {
            get { return isLoginForTenants; }
            set
            {
                isLoginForTenants = value;
                if (!value) TenancyName = string.Empty;
                RaisePropertyChanged();
            }
        }

        public override void Save()
        {
            base.Save(IsLoginForTenants ? TenancyName : "");
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}
