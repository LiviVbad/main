using Abp.Application.Services.Dto;
using AutoMapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.ViewModels
{
    public class DataPagerService : BindableBase, IDataPagerService
    {
        public DataPagerService(IMapper mapper)
        {
            pageSize = AppConsts.DefaultPageSize;
            numericButtonCount = 10;
            gridModelList = new ObservableCollection<object>();
            this.mapper = mapper;
        }

        private object selectedItem;
        private int pageIndex, pageCount, pageSize, numericButtonCount;
        private ObservableCollection<object> gridModelList;
        private readonly IMapper mapper;

        public int NumericButtonCount
        {
            get { return numericButtonCount; }
            set
            {
                numericButtonCount = value;
                RaisePropertyChanged();
            }
        }

        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                pageIndex = value;
                RaisePropertyChanged();
            }
        }

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; RaisePropertyChanged(); }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; RaisePropertyChanged(); }
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

            if (pagedResult.TotalCount == 0)
                PageCount = 1;
            else
                PageCount = (int)Math.Ceiling(pagedResult.TotalCount / (double)PageSize);
        }
    }
}
