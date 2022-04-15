using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using Prism.Ioc;
using AutoMapper;
using AppFramework.Services.Dialog; 
using AppFramework.Common.Core;

namespace AppFramework.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        public ViewModelBase()
        {
            mapper=ContainerLocator.Container.Resolve<IMapper>();
            validator=ContainerLocator.Container.Resolve<IGlobalValidator>();
            applayer =ContainerLocator.Container.Resolve<IAppDialogService>();
        }

        public readonly IMapper mapper;
        private readonly IGlobalValidator validator;
        private readonly IAppDialogService applayer;

        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsNotBusy));
            }
        }

        public async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            try
            {
                applayer.ShowLoading(loadingMessage);
                await func();
            }
            finally
            {
                IsBusy = false;
                applayer.HideLoading();
            }
        }

        public bool Verify<T>(T model)
        {
            return validator.Validate(model).IsValid;
        }

        public T Map<T>(object model)
        {
            return mapper.Map<T>(model);
        }
    }
}
