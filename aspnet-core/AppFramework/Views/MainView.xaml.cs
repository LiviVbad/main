using AppFramework.Common.Models;
using AppFramework.ViewModels;
using Syncfusion.Windows.Shared; 

namespace AppFramework.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : ChromelessWindow
    {
        public MainView()
        { 
            Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            InitializeComponent(); 
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");
            SfNavigationDrawer.ItemClicked += SfNavigationDrawer_ItemClicked;
        }

        private void SfNavigationDrawer_ItemClicked(object sender, Syncfusion.UI.Xaml.NavigationDrawer.NavigationItemClickedEventArgs e)
        {
            if (e.Item == null) return;

            var navigationPage = e.Item.DataContext as NavigationItem;
            (this.DataContext as MainViewModel)?.Navigate(navigationPage);
        }
    }
}
