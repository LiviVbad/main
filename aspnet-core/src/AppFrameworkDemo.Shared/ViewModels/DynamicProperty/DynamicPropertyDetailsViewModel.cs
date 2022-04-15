using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto; 
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using AppFramework.Common.Core;
using AppFramework.Common.Models;

namespace AppFramework.Shared.ViewModels
{
    public class DynamicPropertyDetailsViewModel : NavigationViewModel
    {
        private readonly IMessenger messenger;
        private readonly IDynamicPropertyAppService appService;

        public DelegateCommand SaveCommand { get; private set; }

        private DynamicPropertyModel model;

        public DynamicPropertyModel Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        public DynamicPropertyDetailsViewModel(IMessenger messenger,
            IDynamicPropertyAppService appService)
        {
            this.messenger = messenger;
            this.appService = appService;

            Model = new DynamicPropertyModel();
            SaveCommand = new DelegateCommand(Save);
        }

        private async void Save()
        {
            await SetBusyAsync(async () =>
             {
                 var input = Map<DynamicPropertyDto>(Model);

                 if (input.Id > 0)
                 {
                     await WebRequestRuner.Execute(async () =>
                                await appService.Update(input),
                                async () => await GoBackAsync());
                 }
                 else
                 {
                     await WebRequestRuner.Execute(async () =>
                                await appService.Add(input),
                                async () => await GoBackAsync());
                 }
             });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Dynamic);
            await base.GoBackAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Model = parameters.GetValue<DynamicPropertyModel>("Value");
            }
        }
    }
}