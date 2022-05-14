using AppFramework.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace AppFramework.Services
{
    public interface IThemeService
    {
        ObservableCollection<ThemeItem> GetThemes();

        string GetCurrent();

        void SetTheme(string themeName);

        void SetDefaultTheme(DependencyObject dependency);
    }
}