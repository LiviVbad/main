namespace AppFramework.Shared.ViewModels
{
    using AppFramework.Common.Core;
    using AppFramework.Common.Services.Layer;
    using AppFramework.Shared.Localization;
    using AppFramework.Shared.Services.Layer;
    using AutoMapper;
    using FluentValidation.Results;
    using Prism.Ioc;
    using Prism.Mvvm;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class ViewModelBase : BindableBase
    {
        public ViewModelBase()
        {
            mapper = ContainerLocator.Container.Resolve<IMapper>();
            validator = ContainerLocator.Container.Resolve<IGlobalValidator>();
            applayer = ContainerLocator.Container.Resolve<IApplayerService>();
        }

        private bool isBusy;
        private readonly IMapper mapper;
        private readonly IGlobalValidator validator;
        private readonly IApplayerService applayer;

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
                applayer.Show(Local.Localize(loadingMessage));
                await func();
            }
            finally
            {
                IsBusy = false;
                applayer.Hide();
            }
        }

        /// <summary>
        /// 实体验证器方法
        /// </summary>
        /// <typeparam name="T">验证结果</typeparam>
        /// <param name="model">验证实体</param>
        /// <returns></returns>
        public ValidationResult Verify<T>(T model, bool ShowError = true)
        {
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid && ShowError)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in validationResult.Errors)
                {
                    stringBuilder.AppendLine(item.ErrorMessage);
                }
                AppDialogHelper.Warn(stringBuilder.ToString());
            }
            return validationResult;
        }

        /// <summary>
        /// 实体映射方法
        /// </summary>
        /// <typeparam name="T">最终类型</typeparam>
        /// <param name="model">映射实体</param>
        /// <returns></returns>
        public T Map<T>(object model)
        {
            return mapper.Map<T>(model);
        }
    }
}