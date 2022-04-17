using AppFramework.Models; 
using System.Collections.ObjectModel;
using System.Windows;

namespace AppFramework.Services
{
    public delegate void ThemeEventHandler(string themeName);

    public interface IThemeService
    {
        event ThemeEventHandler ThemeChanged;

        ObservableCollection<ThemeItem> GetThemes();

        void SetTheme(string themeName);

        void SetTheme(DependencyObject dependency, string themeName);

        void SetDefaultTheme(DependencyObject dependency);
    }
}
