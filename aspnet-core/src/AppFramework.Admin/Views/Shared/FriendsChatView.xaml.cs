using System.Windows.Controls; 

namespace AppFramework.Views
{ 
    public partial class FriendsChatView : UserControl
    {
        public FriendsChatView()
        {
            InitializeComponent(); 
            scrollViewer.ScrollToEnd();
        }
    }
}
