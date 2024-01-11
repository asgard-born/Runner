using System;
using System.Threading.Tasks;
using UniRx;

namespace Framework.Reactive
{
    public static class PropertyObservableExtension
    {
        public static async Task<TSelector> WhenValue<TObj, TSelector>(this TObj container,
            Func<TObj, TSelector> selector, Func<TObj, bool> predicate) where TObj : class
        {
            bool predicateResult = predicate.Invoke(container);
            if (predicateResult)
            {
                return selector.Invoke(container);
            }

            return await container.ObserveEveryValueChanged(selector)
                .Where(_ => predicate.Invoke(container))
                .First()
                .ToTask();
        }
    }
}