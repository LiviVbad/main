using AppFramework.Common;
using AppFramework.Models;
using AppFramework.Services;
using Prism.Commands;

namespace AppFramework.ViewModels.Shared
{
    public class VisualViewModel : NavigationViewModel
    {
        public VisualViewModel(IThemeService themeService)
        {
            Title = Local.Localize("VisualSettings");
            this.themeService = themeService;
             
            SetThemeCommand = new DelegateCommand<ThemeItem>(arg => themeService.SetTheme(arg.DisplayName));
        }
         
        public DelegateCommand<ThemeItem> SetThemeCommand { get; } 

        public IThemeService themeService { get; set; }
    }
}
