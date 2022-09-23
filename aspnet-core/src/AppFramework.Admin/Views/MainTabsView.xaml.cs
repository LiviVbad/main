using AppFramework.Services;
using AppFramework.Shared;
using AppFramework.ViewModels;
using Syncfusion.Windows.Shared;
using System;
using System.Windows;

namespace AppFramework.Views
{
    public partial class MainTabsView : ChromelessWindow
    {
        private readonly ILocaleCulture localeCulture;
        private readonly IHostDialogService dialog;

        public MainTabsView( 
            IThemeService themeService,
            IHostDialogService dialog)
        {
            InitializeComponent();
            this.localeCulture = localeCulture;
            this.dialog = dialog; 
            themeService.SetCurrentTheme(this);
            HeaderBorder.MouseDown += (s, e) =>
            {
                if (e.ClickCount == 2) SetWindowState();
            };
            HeaderBorder.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
            this.MouseDown += (s, e) => toggleShowPanel.IsChecked = false;
            BtnMin.Click += BtnMin_Click;
            BtnMax.Click += BtnMax_Click;
            BtnClose.Click += BtnClose_Click;
            BtnDoubleLeft.Click += BtnDoubleLeft_Click;

            treeViewItems.NodeExpanded += TreeViewItems_NodeExpanded;
        }

        private void TreeViewItems_NodeExpanded(object? sender, Syncfusion.UI.Xaml.TreeView.NodeExpandedCollapsedEventArgs e)
        {
            if (treeViewItems.ExpanderWidth == 0) CollapseMenu();
        }

        private void BtnDoubleLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (treeViewItems.ExpanderWidth > 0)
            {
                TitltStack.Visibility = Visibility.Collapsed;
                leftGridColumn.Width = new GridLength(50);
                TxtDoubleTitle.Text = "\ue74d";

                treeViewItems.ExpanderWidth = 0;
                treeViewItems.CollapseAll();
            }
            else
            {
                TitltStack.Visibility = Visibility.Visible;
                leftGridColumn.Width = new GridLength(220);
                TxtDoubleTitle.Text = "\ue74c";
                treeViewItems.ExpanderWidth = 15;
            }
        }

        private async void BtnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await dialog.Question(Local.Localize("AreYouSure")))
                Environment.Exit(0);
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
            SystemButtonsUpdate();
        }

        private void TabControlExt_OnCloseButtonClick(object sender, Syncfusion.Windows.Tools.Controls.CloseTabEventArgs e)
        {
            if (e.TargetTabItem != null)
            {
                (this.DataContext as MainTabsViewModel)?.NavigationService.RemoveView(e.TargetTabItem.Content);
            }
        }

        private void TabControlExt_OnCloseAllTabs(object sender, Syncfusion.Windows.Tools.Controls.CloseTabEventArgs e)
        {
            if (e.ClosingTabItems != null)
            {
                var conext = this.DataContext as MainTabsViewModel;
                foreach (var item in e.ClosingTabItems)
                    conext?.NavigationService.RemoveView(item);
            }
        }

        private void TabControlExt_OnCloseOtherTabs(object sender, Syncfusion.Windows.Tools.Controls.CloseTabEventArgs e)
        {
            if (e.ClosingTabItems != null)
            {
                var conext = this.DataContext as MainTabsViewModel;
                foreach (var item in e.ClosingTabItems)
                {
                    if (item != e.TargetTabItem.Content)
                    {
                        conext?.NavigationService.RemoveView(item);
                    }
                }
            }
        }
    }
}