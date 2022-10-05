﻿using AppFramework.Shared.Services.Layer;
using AppFramework.Shared.ViewModels;
using Prism.Regions.Navigation;
using System;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Navigation;

namespace AppFramework.Shared.ViewModels
{
    public class RegionViewModel : ViewModelBase, IRegionAware
    {
        private readonly IApplayerService applayer;
        public readonly IDialogService dialogService;
        public readonly INavigationService navigationService;

        public RegionViewModel()
        {
            applayer = ContainerLocator.Container.Resolve<IApplayerService>();
            dialogService = ContainerLocator.Container.Resolve<IDialogService>();
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();
        }

        public virtual bool IsNavigationTarget(INavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(INavigationContext navigationContext)
        { }

        public virtual async void OnNavigatedTo(INavigationContext navigationContext)
        {
            await RefreshAsync();
        }

        public virtual async Task RefreshAsync() => await Task.CompletedTask;

        public override async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            try
            {
                applayer.Show(loadingMessage);
                await func();
            }
            finally
            {
                IsBusy = false;
                applayer.Hide();
            }
        }
    }
}