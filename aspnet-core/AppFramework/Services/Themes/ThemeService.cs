using AppFramework.Extensions;
using AppFramework.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace AppFramework.Services
{
    public class ThemeService : IThemeService
    {
        public event ThemeEventHandler ThemeChanged;

        private ObservableCollection<ThemeItem> themes = new ObservableCollection<ThemeItem>()
        {
            new ThemeItem(){  DisplayName="Fluent", LightName="FluentLight",DarkName="FluentDark"},
            new ThemeItem(){  DisplayName="Material",LightName="MaterialLight",DarkName="MaterialDark"},
            new ThemeItem(){  DisplayName="MaterialBlue",LightName="MaterialLightBlue",DarkName="MaterialDarkBlue"},
            new ThemeItem(){  DisplayName="Office2019",LightName="Office2019White",DarkName="Office2019Black"},
        };

        public ObservableCollection<ThemeItem> GetThemes()
        { 
            return themes;
        }

        public void SetTheme(DependencyObject dependency, string themeName)
        {
            dependency.SetTheme(themeName);
        }

        public void SetTheme(string themeName)
        {
            ThemeChanged?.Invoke(themeName);
        }

        public void SetDefaultTheme(DependencyObject dependency)
        {
            dependency.SetTheme("MaterialDark");
        }
    }
}