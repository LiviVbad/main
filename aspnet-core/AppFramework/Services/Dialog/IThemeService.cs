using AppFramework.Models; 
using System.Collections.ObjectModel; 

namespace AppFramework.Services
{
    public delegate void ThemeEventHandler(string themeName);

    public interface IThemeService
    {
        event ThemeEventHandler ThemeChanged;

        ObservableCollection<ThemeItem> GetThemes();

        void SetTheme(string themeName);

        void SetTheme(object dependency, string themeName);

        void SetDefaultTheme(object dependency);
    }
}
