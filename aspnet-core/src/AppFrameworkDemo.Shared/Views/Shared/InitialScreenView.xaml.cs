using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFrameworkDemo.Shared.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialScreenView : ContentPage
    {
        public InitialScreenView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}