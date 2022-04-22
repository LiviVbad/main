using AppFramework.Common.Core;
using AppFramework.Services.Dialog;
using AutoMapper;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        public ViewModelBase()
        {
            mapper = ContainerLocator.Container.Resolve<IMapper>();
            validator = ContainerLocator.Container.Resolve<IGlobalValidator>(); 
        }

        public readonly IMapper mapper;
        private readonly IGlobalValidator validator; 

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
                await func();
            } 
            finally
            {
                IsBusy = false;
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