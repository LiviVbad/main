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
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5NjkxQDMyMzAyZTMxMmUzMG4yeGhZNm01STJSdnVKQVJiUHpzM3ZMUEc5K1hZTXd3TVFTbGZ1UERrQlU9");
            InitializeComponent();
             
            themeService.SetDefaultTheme(this);
            resourceService.UpdateResources(App.Current.Resources, "MaterialDark");
            SfNavigationDrawer.ItemClicked += SfNavigationDrawer_ItemClicked;
        }

        private void SfNavigationDrawer_ItemClicked(object sender, NavigationItemClickedEventArgs e)
        {
            if (e.Item == null) return;

            var item = e.Item.DataContext as Common.Models.NavigationItem;
            (this.DataContext as MainViewModel)?.Navigate(item);
        }
    }
}