using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Common.Core
{
    public interface IWeakActionCollection : IEnumerable<IWeakAction>
    {
        /// <summary>
        /// 事件总数
        /// </summary>
        int Count { get; }
         
        /// <summary>
        /// 添加事件消息
        /// </summary>
        /// <param name="weakEvent">事件源</param>
        void Add(IWeakAction weakEvent);

        /// <summary>
        /// 根据令牌移除事件消息
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        bool Remove(string token);
    }
}
