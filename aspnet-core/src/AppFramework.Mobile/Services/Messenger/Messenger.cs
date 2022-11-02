﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppFramework.Shared.Core
{
    public class Messenger : IMessenger
    {
        private Dictionary<string, IWeakActionCollection> _weakEvents;

        public void SendByToken(string token)
        {
            Send(string.Empty, token);
        }

        public void SendByToken<T>(string token, T message)
        {
            Send(token, message);
        }

        public void Send(string token)
        {
            var weakMessages = GetWeakEvents(token);
            if (weakMessages.Length > 0)
                weakMessages.SendMessage();
        }

        public void Send<T>( string token, T message)
        {
            var weakMessages = GetWeakEvents(token);
            if (weakMessages.Length > 0)
                weakMessages.SendMessage(message);
        }

        public void Subscribe(string token, Action action)
        {
            var weakEventCollection = GetWeakEventCollection(token);
            weakEventCollection.Add(new WeakAction(token, action));

            RefreshSubscribes(_weakEvents);
        }

        public void Subscribe<T>(string token, Action<T> action)
        {
            var weakEventCollection = GetWeakEventCollection(token);
            weakEventCollection.Add(new WeakAction<T>(token, action));

            RefreshSubscribes(_weakEvents);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subscriber">订阅者</param>
        /// <param name="token">令牌</param>
        public void Unsubscribe( string token)
        {
            if (string.IsNullOrWhiteSpace(token) ||
               _weakEvents == null ||
               _weakEvents.Count == 0 ||
               !_weakEvents.ContainsKey(token))
            {
                return;
            }

            if (_weakEvents.ContainsKey(token))
            {
                var weakMessages = _weakEvents[token];

                foreach (var weakMessage in weakMessages)
                {
                    if (weakMessage.Token.Equals(token))
                        weakMessage?.Dispose();
                }
            }

            RefreshSubscribes(_weakEvents);
        }

        /// <summary>
        /// 根据订阅者及令牌获取匹配事件数组
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private IWeakAction[] GetWeakEvents(string token)
        {
            IWeakAction[] weakMessages = new IWeakAction[0];
            foreach (var item in _weakEvents)
            {
                weakMessages = weakMessages
                    .Union(item.Value
                    .Where(t => t.IsAlive && t.Token.Equals(token)))
                    .ToArray();
            }
            return weakMessages;
        }

        /// <summary>
        /// 根据订阅者及令牌获取事件集合源
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private IWeakActionCollection GetWeakEventCollection(string token)
        {
            if (_weakEvents == null)
                _weakEvents = new Dictionary<string, IWeakActionCollection>();

            Dictionary<string, IWeakActionCollection> temps = _weakEvents;
            IWeakActionCollection weakMessages;

            if (!_weakEvents.ContainsKey(token))
            {
                weakMessages = new WeakEventCollection();
                temps[token] = weakMessages;
            }
            else
                weakMessages = temps[token];

            return weakMessages;
        }
         
        private void RefreshSubscribes(IDictionary<string, IWeakActionCollection> subscriberCollection)
        {
            if (subscriberCollection == null) return;

            var removeKeys = new List<string>();

            foreach (var subscriber in subscriberCollection)
            {
                var deletedSubscribers = subscriber.Value.Where(q => q.IsAlive == false).ToList();

                foreach (var weakEvent in deletedSubscribers)
                {
                    subscriber.Value.Remove(weakEvent.Token);
                }

                if (subscriber.Value.Count == 0)
                    removeKeys.Add(subscriber.Key);
            }

            foreach (var key in removeKeys)
            {
                subscriberCollection.Remove(key);
            }
        }

        private class WeakEventCollection : IWeakActionCollection
        {
            public WeakEventCollection()
            {
                Subscribers = new List<IWeakAction>();
            }

            public int Count => Subscribers.Count;

            private List<IWeakAction> Subscribers;

            public void Add(IWeakAction weakEvent)
            {
                var wk = Subscribers.FirstOrDefault(t => t.Token == weakEvent.Token);
                if (wk != null) 
                    wk = weakEvent;
                else
                    Subscribers.Add(weakEvent);
            }

            public IEnumerator<IWeakAction> GetEnumerator()
            {
                return Subscribers.GetEnumerator();
            }

            public bool Remove(string token)
            {
                var weakEvent = GetWeakEvent(token);
                if (weakEvent != null)
                {
                    Subscribers.Remove(weakEvent);
                    return true;
                }
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private IWeakAction GetWeakEvent(string token)
            {
                return this.Subscribers.FirstOrDefault(r => r.Token == token);
            }
        } 
    }
}
