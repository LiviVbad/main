namespace AppFramework.Shared
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public interface ICurdViewModel<T>
    {
        /// <summary>
        /// 实体列表
        /// </summary>
        ObservableCollection<T> GridModelList { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        void Add();

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="selectedItem">选中编辑项</param>
        void Edit(T selectedItem);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="selectedItem">选中删除项</param>
        void Delete(T selectedItem);

        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        Task RefreshAsync();
    }
}