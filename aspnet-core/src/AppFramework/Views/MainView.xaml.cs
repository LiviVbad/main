using AppFramework.Services;
using AppFramework.ViewModels;
using Syncfusion.Windows.Shared;
using Syncfusion.UI.Xaml.NavigationDrawer;

namespace AppFramework.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : ChromelessWindow
    {
        public MainView(IThemeService themeService, IResourceService resourceService)
        {
            AppSettings.OnInitialized();
            InitializeComponent();

            themeService.SetDefaultTheme(this);
            resourceService.UpdateResources(App.Current.Resources, themeService.GetCurrent());
            SfNavigationDrawer.ItemClicked += SfNavigationDrawer_ItemClicked;
        }

        private void SfNavigationDrawer_ItemClicked(object? sender, NavigationItemClickedEventArgs e)
        {
            if (e.Item == null) return;
            var item = e.Item.DataContext as Common.Models.NavigationItem;
            if (item == null) return;
            (this.DataContext as MainViewModel)?.Navigate(item);
        }
    }
}