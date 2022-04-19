using AppFramework.Common.Models;
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
        private readonly IThemeService themeService;
        private readonly IResourceService resourceService;

        public MainView(IThemeService themeService,
            IResourceService resourceService)
        {
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");
           
            this.themeService = themeService;
            this.resourceService = resourceService;
            themeService.SetDefaultTheme(this);
            resourceService.UpdateCustomResources(App.Current.Resources, "MaterialDark");
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