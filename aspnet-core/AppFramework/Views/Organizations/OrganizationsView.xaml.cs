using AppFramework.Common.Models;
using AppFramework.ViewModels;
using Syncfusion.UI.Xaml.TreeView;
using System.Windows;
using System.Windows.Controls;

namespace AppFramework.Views
{
    /// <summary>
    /// OrganizationsView.xaml 的交互逻辑
    /// </summary>
    public partial class OrganizationsView : UserControl
    {
        public OrganizationsView()
        {
            InitializeComponent();

            AddRootMenu.Click += SftreeViewMenuItem_Click;
            ChangeMenu.Click += SftreeViewMenuItem_Click;
            RemoveMenu.Click += SftreeViewMenuItem_Click;
        }

        private async void SftreeViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem.DataContext is TreeViewItemContextMenuInfo info)
            {
                var organizationUnit = info.Node.Content as OrganizationUnitModel;
                var context = this.DataContext as OrganizationsViewModel;
                switch (menuItem.Tag)
                {
                    case "AddRootUnit":
                        await context.AddOrganizationUnit(organizationUnit.Id);
                        break;

                    case "Change":
                        await context.UpdateRootUnit(organizationUnit);
                        break;

                    case "Remove":
                        await context.DeleteOrganizationUnit(organizationUnit);
                        break;
                }
            }
        }
    }
}