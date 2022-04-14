using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppFrameworkDemo.Localization.Dto;
using AppFrameworkDemo.Localization;
using Prism.Mvvm;

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : BindableBase
    {
        private readonly ILanguageAppService appService;

        private ObservableCollection<ApplicationLanguageListDto> gridModelList;

        public ObservableCollection<ApplicationLanguageListDto> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public LanguageViewModel(ILanguageAppService languageAppService)
        {
            this.appService = languageAppService;

            GridModelList = new ObservableCollection<ApplicationLanguageListDto>();
        }

        public async Task RefreshAsync()
        {
            await WebRuner.Execute(
                       () => appService.GetLanguages(),
                       result => RefreshSuccessed(result));
        }

        protected virtual async Task RefreshSuccessed(GetLanguagesOutput output)
        {
            GridModelList.Clear();

            foreach (var item in output.Items)
                GridModelList.Add(item);

            await Task.CompletedTask;
        }
    }
}
