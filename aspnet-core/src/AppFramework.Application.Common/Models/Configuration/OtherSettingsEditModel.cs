using Prism.Mvvm;

namespace AppFramework.Common.Models.Configuration
{
    public class OtherSettingsEditModel : BindableBase
    {
        private bool isQuickThemeSelectEnabled;

        public bool IsQuickThemeSelectEnabled
        {
            get { return isQuickThemeSelectEnabled; }
            set { isQuickThemeSelectEnabled = value; RaisePropertyChanged(); }
        }
    }
}