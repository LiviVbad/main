using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public interface INavigationDataAware 
    {
        object SelectedItem { get; set; }

        int TotalCount { get; set; }

        ObservableCollection<object> GridModelList { get; set; }

        void Add();

        void Edit();

        Task RefreshAsync();
    }
}
