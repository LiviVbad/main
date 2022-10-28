using AppFramework.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;

namespace AppFramework.Views
{
    public partial class FriendsChatView : UserControl
    {
        public FriendsChatView()
        {
            InitializeComponent(); 
            sfInputText.KeyDown+=SfInputText_KeyDown;
        }

        private void SfInputText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key== System.Windows.Input.Key.Enter)
            {
                (this.DataContext as FriendsChatViewModel)?.Send();
                scrollViewer.ScrollToEnd();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var url = e.Uri.AbsoluteUri.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
