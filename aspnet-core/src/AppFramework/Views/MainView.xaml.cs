using AppFramework.Services;
using AppFramework.ViewModels;
using Syncfusion.Windows.Shared;
using Syncfusion.UI.Xaml.NavigationDrawer;

namespace AppFramework.Views
{ 
    public partial class MainView : ChromelessWindow
    {
        public MainView(IThemeService themeService, IResourceService resourceService)
        {
            AppSettings.OnInitialized();
            InitializeComponent();

            themeService.SetCurrentTheme(this);
            resourceService.UpdateResources(App.Current.Resources, themeService.GetCurrentName());
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