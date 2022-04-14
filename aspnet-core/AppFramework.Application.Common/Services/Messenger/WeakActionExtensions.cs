using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Common.Core
{
    internal static class WeakActionExtensions
    {
        /// <summary>
        /// 发送事件消息不带参数
        /// </summary>
        /// <param name="weakMessages"></param>
        internal static void SendMessage(this IWeakAction[] weakMessages)
        {
            if (weakMessages == null) return;

            foreach (var weakMessage in weakMessages)
            {
                if (weakMessage != null && weakMessage.Type.Equals(string.Empty))
                {
                    weakMessage.Execute(null);
                } 
            }
        }

        /// <summary>
        /// 发送事件消息带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="weakMessages"></param>
        /// <param name="message"></param>
        internal static void SendMessage<T>(this IWeakAction[] weakMessages, T message)
        {
            if (weakMessages == null) return;

            foreach (var weakMessage in weakMessages)
            {
                if (weakMessage != null && weakMessage.Type.Equals(typeof(T).FullName))
                {
                    weakMessage.Execute(message);
                }
            }
        }
    }
}
