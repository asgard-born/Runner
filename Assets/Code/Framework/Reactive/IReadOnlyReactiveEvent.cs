using System;

namespace Framework.Reactive
{
    public interface IReadOnlyReactiveEvent<out T>
    {
        IDisposable Subscribe(Action<T> action);
        IDisposable SubscribeWithSkip(Action<T> action);
        IDisposable SubscribeOnceWithSkip(Action<T> action);
    }

    public interface IReadOnlyReactiveEvent<out T1, out T2>
    {
        IDisposable Subscribe(Action<T1, T2> action);
        IDisposable SubscribeWithSkip(Action<T1, T2> action);
        IDisposable SubscribeOnceWithSkip(Action<T1, T2> action);
    }

    public interface IReadOnlyReactiveEvent<out T1, out T2, out T3>
    {
        IDisposable Subscribe(Action<T1, T2, T3> action);
        IDisposable SubscribeWithSkip(Action<T1, T2, T3> action);
        IDisposable SubscribeOnceWithSkip(Action<T1, T2, T3> action);
    }

    public interface IReadOnlyReactiveEvent<out T1, out T2, out T3, out T4>
    {
        IDisposable Subscribe(Action<T1, T2, T3, T4> action);
        IDisposable SubscribeWithSkip(Action<T1, T2, T3, T4> action);
        IDisposable SubscribeOnceWithSkip(Action<T1, T2, T3, T4> action);
    }
}