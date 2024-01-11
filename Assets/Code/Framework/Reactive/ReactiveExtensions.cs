using System;
using System.Collections.Generic;
using UniRx;

namespace Framework.Reactive
{
    public static class ReactiveExtensions
    {
        public static IDisposable DelayedCall(Action action, float delaySec)
          => DelayedCall(action, TimeSpan.FromSeconds(delaySec));

        public static IDisposable DelayedCall(Action action, TimeSpan delay)
        {
            if (delay <= TimeSpan.Zero)
            {
                action?.Invoke();
                return null;
            }
            return Observable.Timer(delay).First().Subscribe(_ => action?.Invoke());
        }

        public static (Dictionary<TKey, TValue>, IDisposable, IDisposable) CreateCachedDictionary<TKey, TValue>(
          this IReadOnlyReactiveDictionary<TKey, TValue> reactiveDictionary)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(reactiveDictionary.Count);
            foreach (KeyValuePair<TKey, TValue> pair in reactiveDictionary)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
            IDisposable onAdd = reactiveDictionary.ObserveAdd().Subscribe(added => dictionary.Add(added.Key, added.Value));
            IDisposable onRemove = reactiveDictionary.ObserveRemove().Subscribe(removed => dictionary.Remove(removed.Key));
            return (dictionary, onAdd, onRemove);
        }
    }
}