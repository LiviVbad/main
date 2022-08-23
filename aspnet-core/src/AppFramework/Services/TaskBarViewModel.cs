using AppFramework.Services;
using AppFramework.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Ioc;
using System;

namespace AppFramework
{
    public class TaskBarViewModel : BindableBase
    {
        public DelegateCommand ShowViewCommand { get; private set; }
        public DelegateCommand ExitCommand { get; set; }
        private readonly IHostDialogService dialog;

        public TaskBarViewModel()
        {
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
            ShowViewCommand = new DelegateCommand(ShowView);
            ExitCommand = new DelegateCommand(Exit);
        }

        private async void Exit()
        {
            ShowView();

            if (await dialog.Question(Local.Localize("AreYouSure")))
            {
                Environment.Exit(0);
            }
        }

        private void ShowView()
        {
            if (!App.Current.MainWindow.IsVisible)
            {
                App.Current.MainWindow.Show();
                App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
