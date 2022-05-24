using AppFramework.Services;
using Syncfusion.Windows.Shared;
using System.Windows.Input;

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

            this.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };

            BtnMin.Click += BtnMin_Click;
            BtnMax.Click += BtnMax_Click;
            BtnClose.Click += BtnClose_Click;
        }

        private void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMax_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = ((base.WindowState != System.Windows.WindowState.Maximized) ? System.Windows.WindowState.Maximized : System.Windows.WindowState.Normal);
            SystemButtonsUpdate();
        }

        private void BtnMin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = ((base.WindowState != System.Windows.WindowState.Minimized) ?
               System.Windows.WindowState.Minimized : System.Windows.WindowState.Normal);
        }
    }
}