using AppFramework.ViewModels;
using System.Linq;
using System.Windows.Controls;
using Syncfusion.UI.Xaml.Controls.DataPager;

namespace AppFramework.Views
{
    public partial class LanguageChengedTextView : UserControl
    {
        public LanguageChengedTextView()
        {
            InitializeComponent();
        }

        private void sfDataPapger_OnDemandLoading(object sender, OnDemandLoadingEventArgs e)
        {
            var context = this.DataContext as INavigationDataAware;
            sfDataPapger.LoadDynamicItems(e.StartIndex, context.GridModelList.Skip(e.StartIndex).Take(e.PageSize));
        }
    }
}
