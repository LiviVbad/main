using Prism.Commands;
using System.Collections.ObjectModel; 
using Prism.Regions;
using AppFramework.Services;
using AppFramework.Models;

namespace AppFramework.ViewModels
{
    public class VisualViewModel : ViewModelBase
    {
        private readonly IThemeService themeService;
        public DelegateCommand ReplayCommand { get; }
        public DelegateCommand RestoreCommand { get; }

        private ThemeItem selectedTheme;

        public ThemeItem SelectedTheme
        {
            get { return selectedTheme; }
            set { selectedTheme = value; RaisePropertyChanged(); }
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ThemeItem> themeItems;

        public ObservableCollection<ThemeItem> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; RaisePropertyChanged(); }
        }

        public VisualViewModel(IThemeService themeService)
        {
            this.themeService = themeService;

            ReplayCommand = new DelegateCommand(Replay);
            RestoreCommand = new DelegateCommand(() =>
              {
                  themeService.SetTheme("MaterialDark");
              });
        }

        private void Replay()
        {
            var themeName = SelectedIndex == 0 ?
                SelectedTheme.LightName :
                SelectedTheme.DarkName;

            themeService.SetTheme(themeName);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ThemeItems = themeService.GetThemes();
        }
    }
}
