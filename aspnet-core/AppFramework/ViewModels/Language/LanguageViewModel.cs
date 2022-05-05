using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Localization; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : NavigationCurdViewModel<LanguageListModel>
    {
        private readonly ILanguageAppService appService;

        public LanguageViewModel(ILanguageAppService languageAppService)
        {
            this.appService = languageAppService;
        }

        public override async Task RefreshAsync()
        {
            await WebRequest.Execute(() => appService.GetLanguages(),
                      async result =>
                      {
                          GridModelList.Clear();

                          foreach (var item in Map<List<LanguageListModel>>(result.Items))
                              GridModelList.Add(item);

                          await Task.CompletedTask;
                      });
        }
    }
}