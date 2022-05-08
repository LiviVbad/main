using AppFramework.Models;
using AppFramework.Services;
using Prism.Commands;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace AppFramework.ViewModels
{
    public class VisualViewModel : NavigationViewModel
    {
        public VisualViewModel(IThemeService themeService)
        {
            this.themeService = themeService;
            ReplayCommand = new DelegateCommand<ThemeItem>(Replay);
        }

        #region 字段/属性

        private bool initialize;
        private readonly IThemeService themeService;
        public DelegateCommand<ThemeItem> ReplayCommand { get; }
        public DelegateCommand RestoreCommand { get; }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (initialize)
                {
                    AppSettings.Instance.IsDarkTheme = value == 1;
                    themeService.SetTheme(themeService.GetCurrent());
                }

                selectedIndex = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ThemeItem> themeItems;

        public ObservableCollection<ThemeItem> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; RaisePropertyChanged(); }
        }

        #endregion

        private void Replay(ThemeItem themeItem)
        {
            AppSettings.Instance.IsDarkTheme = SelectedIndex == 1;
            AppSettings.Instance.ThemeName = themeItem.DisplayName;
            themeService.SetTheme(themeService.GetCurrent());
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ThemeItems = themeService.GetThemes();
            SelectedIndex = AppSettings.Instance.IsDarkTheme ? 1 : 0;
            initialize = true;
        }
    }
}