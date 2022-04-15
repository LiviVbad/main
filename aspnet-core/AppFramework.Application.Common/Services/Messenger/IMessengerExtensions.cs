using System;
using System.Threading.Tasks;

namespace AppFramework.Common.Core
{
    public static class IMessengerExtensions
    {
        /// <summary>
        /// 发送消息通知(不带参数)
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        public static void Send(this IMessenger messenger,
            string subscriberKey)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Send(body.subscriber, body.key);
        }

        /// <summary>
        /// 发送消息通知(带泛型参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        /// <param name="message"></param>
        public static void Send<T>(this IMessenger messenger,
           string subscriberKey, T message)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Send(body.subscriber, body.key, message);
        }

        /// <summary>
        /// 订阅消息(无参数返回)
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        /// <param name="action"></param>
        public static void Sub(this IMessenger messenger,
            string subscriberKey,
            Action action)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Subscribe(body.subscriber, body.key, action);
        }

        /// <summary>
        /// 订阅消息(异步无参数消息)
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        /// <param name="func"></param>
        public static void Sub(this IMessenger messenger,
            string subscriberKey,
            Func<Task> func)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Subscribe(body.subscriber, body.key, async () => await func());
        }

        /// <summary>
        /// 订阅消息(无参数返回)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        /// <param name="action"></param>
        public static void Sub<T>(this IMessenger messenger,
            string subscriberKey,
            Action<T> action)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Subscribe(body.subscriber, body.key, action);
        }

        /// <summary>
        /// 订阅消息(异步带参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        /// <param name="func"></param>
        public static void Sub<T>(this IMessenger messenger,
            string subscriberKey,
            Func<Task<T>> func)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Subscribe(body.subscriber, body.key, async () => await func());
        }

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="subscriberKey">订阅者密钥</param>
        public static void UnSub(this IMessenger messenger, string subscriberKey)
        {
            var body = GetSubscriberAndToken(subscriberKey);
            messenger.Unsubscribe(body.subscriber, body.key);
        }

        private static MessageInfo GetSubscriberAndToken(string subscriberKey)
        {
            var array = subscriberKey.Split("/");

            if (array.Length!=2)
                throw new ArgumentException("The input format is wrong, subscriberKey format is: subscriber/token");

            return new MessageInfo() { subscriber=array[0], key=array[1] };
        }
         
        private struct MessageInfo
        {
            public string subscriber { get; set; }
            public string key { get; set; }
        }
    }
}
