using Abp.Application.Services.Dto;
using AutoMapper;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace AppFramework.Shared.Services.Datapager
{
    public class DataPagerService : BindableBase, IDataPagerService
    {
        private readonly IMapper mapper;

        public DataPagerService(IMapper mapper)
        {
            gridModelList = new ObservableCollection<object>();
            this.mapper = mapper;
        }

        private object selectedItem;
        private int skipCount;
        private ObservableCollection<object> gridModelList;
        public int SkipCount
        {
            get { return skipCount; }
            set { skipCount = value; RaisePropertyChanged(); }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<object> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public void SetList<T>(IPagedResult<T> pagedResult)
        {
            GridModelList.Clear();

            foreach (var item in mapper.Map<List<T>>(pagedResult.Items))
                GridModelList.Add(item);
        }
    }
}
