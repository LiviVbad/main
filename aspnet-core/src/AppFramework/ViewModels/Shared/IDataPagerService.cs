using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    /// <summary>
    /// 数据分页服务
    /// </summary>
    public interface IDataPagerService
    {
        /// <summary>
        /// 当前页索引
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// 选中项
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// 分页按钮数量
        /// </summary>
        int NumericButtonCount { get; set; }

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
