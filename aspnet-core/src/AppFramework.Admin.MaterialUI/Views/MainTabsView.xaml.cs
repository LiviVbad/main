using AppFramework.Shared.Services.App;
using AppFramework.Shared;
using System.Windows;
using AppFramework.Shared.Services;
using System.Windows.Controls;
using AppFramework.Admin.ViewModels;

namespace AppFramework.Admin.MaterialUI.Views
{
    public partial class MainTabsView : Window
    {
        private readonly IHostDialogService dialog;
        private readonly IAppStartService appStartService;

        public MainTabsView(IHostDialogService dialog, IAppStartService appStartService)
        {
            InitializeComponent();

            this.dialog = dialog;
            this.appStartService=appStartService;

            HeaderBorder.MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2) SetWindowState();
            };
            HeaderBorder.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };

            BtnMin.Click += BtnMin_Click;
            BtnMax.Click += BtnMax_Click;
            BtnClose.Click += BtnClose_Click;
        }

        private async void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
                appStartService.Exit();
        }

        private void BtnMax_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetWindowState();
        }

        private void BtnMin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = ((base.WindowState != System.Windows.WindowState.Minimized) ?
               System.Windows.WindowState.Minimized : System.Windows.WindowState.Normal);

            this.Hide();
        }

        private void SetWindowState()
        {
            this.WindowState = ((base.WindowState != System.Windows.WindowState.Maximized) ? System.Windows.WindowState.Maximized : System.Windows.WindowState.Normal);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.Source != null&&e.Source is Button content)
            {
                if (content!=null&&content.CommandParameter is TabItem tabItem)
                {
                    if (this.DataContext is MainTabsViewModel viewModel)
                        viewModel.NavigationService.RemoveView(tabItem.Content);
                }
            }
        } 
    }
}
