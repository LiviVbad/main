using Abp.Localization;
using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Localization;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageChengedTextViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        private readonly ILanguageAppService appService;
        private readonly IApplicationContext context;

        public DelegateCommand QueryCommand { get; private set; }

        private string filter;
        private int targetIndex;
        private GetLanguageTextsInput input;
        private ObservableCollection<string> sources;
        private ObservableCollection<LanguageStruct> baseLanguages;
        private ObservableCollection<LanguageStruct> targetLanguages;
        private LanguageStruct selectedBaseLanguage;
        private LanguageStruct selectedTargetLanguage;

        public string Filter
        {
            get { return filter; }
            set { filter = value; RaisePropertyChanged(); }
        }

        public int TargetIndex
        {
            get { return targetIndex; }
            set { targetIndex = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<LanguageStruct> BaseLanguages
        {
            get { return baseLanguages; }
            set { baseLanguages = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<LanguageStruct> TargetLanguages
        {
            get { return targetLanguages; }
            set { targetLanguages = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> Sources
        {
            get { return sources; }
            set { sources = value; RaisePropertyChanged(); }
        }

        public LanguageStruct SelectedBaseLanguage
        {
            get { return selectedBaseLanguage; }
            set
            {
                selectedBaseLanguage = value;
                input.BaseLanguageName = value.Name;
                RaisePropertyChanged();
            }
        }

        public LanguageStruct SelectedTargetLanguage
        {
            get { return selectedTargetLanguage; }
            set
            {
                selectedTargetLanguage = value;
                input.TargetLanguageName = value.Name;
                RaisePropertyChanged();
            }
        }

        public string SelectedSource
        {
            get { return input.SourceName; }
            set
            {
                input.SourceName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public LanguageChengedTextViewModel(ILanguageAppService appService, IApplicationContext context)
        {
            sources = new ObservableCollection<string>();
            baseLanguages = new ObservableCollection<LanguageStruct>();
            targetLanguages = new ObservableCollection<LanguageStruct>();

            input = new GetLanguageTextsInput()
            {
                FilterText = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService = appService;
            this.context = context;
            QueryCommand = new DelegateCommand(Query);
        }

        private void Query() { }

        public override async Task RefreshAsync()
        {
            await WebRequest.Execute(() => appService.GetLanguageTexts(input),
                      async result =>
                      {
                          GridModelList.Clear();

                          foreach (var item in Map<List<LanguageTextListModel>>(result.Items))
                              GridModelList.Add(item);

                          TotalCount = result.TotalCount;

                          await Task.CompletedTask;
                      });
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            foreach (var item in context.Configuration.Localization.Sources)
            {
                Sources.Add(item.Name);
            }

            foreach (var item in context.Configuration.Localization.Languages)
            {
                BaseLanguages.Add(new LanguageStruct(item.Name, item.DisplayName));
                TargetLanguages.Add(new LanguageStruct(item.Name, item.DisplayName));
            }

            if (navigationContext.Parameters.ContainsKey("Name"))
            {
                var Name = navigationContext.Parameters.GetValue<string>("Name");

                var lang = context.Configuration.Localization.Languages.FirstOrDefault(t => t.Name.Equals(Name));
                if (lang != null)
                {
                    SelectedBaseLanguage = new LanguageStruct(lang.Name, lang.DisplayName);
                    SelectedTargetLanguage = new LanguageStruct(lang.Name, lang.DisplayName);
                }

                input.TargetValueFilter = "ALL";
                input.SourceName = Sources.Last();
            }

            await RefreshAsync();
        }
    }
}
