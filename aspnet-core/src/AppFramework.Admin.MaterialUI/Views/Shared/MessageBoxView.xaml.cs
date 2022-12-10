using AppFramework.Shared.Services;
using System.Windows.Controls;

namespace AppFramework.Admin.MaterialUI
{
    public partial class MessageBoxView : UserControl
    {
        public MessageBoxView(IThemeService themeService)
        {
            InitializeComponent(); 
            themeService.SetCurrentTheme(this); 
        }
    }
}
