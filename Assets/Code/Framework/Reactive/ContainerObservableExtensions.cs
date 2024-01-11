using System.Threading.Tasks;
using UniRx;

namespace Framework.Reactive
{
    public static class ContainerObservableExtension
    {
        public static async Task<T> WhenValueNotNull<T>(this Container<T> container)
        {
            if (container.Value != null)
            {
                return container.Value;
            }

            return await container.ObserveEveryValueChanged(c => c.Value)
                .Where(v => v != null)
                .First()
                .ToTask();
        }
    }
}