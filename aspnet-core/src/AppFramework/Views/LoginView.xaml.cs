using AppFramework.Services;
using Prism.Services.Dialogs;
using Syncfusion.Windows.Shared;

namespace AppFramework.Views
{ 
    public partial class LoginView : ChromelessWindow, IDialogWindow
    {
        public LoginView(IThemeService service)
        {
            InitializeComponent();
            service.SetDefaultTheme(this);
        }

        public IDialogResult Result { get; set; }
    }
}