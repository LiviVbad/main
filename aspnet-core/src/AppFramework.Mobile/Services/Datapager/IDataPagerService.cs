using Abp.Application.Services.Dto;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Services.Datapager
{
    public interface IDataPagerService
    { 
        int SkipCount { get; set; }

        /// <summary>
        /// 选中项
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        ObservableCollection<object> GridModelList { get; set; }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        void SetList<T>(IPagedResult<T> pagedResult);
    }
}
