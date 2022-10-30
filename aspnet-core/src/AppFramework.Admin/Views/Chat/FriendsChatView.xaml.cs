﻿using AppFramework.Admin.Events;
using AppFramework.Admin.ViewModels.Chat;
using AppFramework.Shared;
using Prism.Events;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppFramework.Views
{
    public partial class FriendsChatView : UserControl
    {
        public FriendsChatView(IEventAggregator aggregator)
        {
            InitializeComponent();
            sfInputText.KeyDown+=SfInputText_KeyDown;
            this.KeyDown+=(s, e) =>
            {
                if (e.KeyStates== Keyboard.GetKeyStates(Key.C)&&Keyboard.Modifiers==ModifierKeys.Alt)
                {
                    (this.DataContext as HostDialogViewModel)?.Cancel();
                }
            };
            aggregator.GetEvent<ScrollEvent>().Subscribe(result => scrollViewer.ScrollToEnd());
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