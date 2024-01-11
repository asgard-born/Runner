using System;

namespace Framework.Reactive
{
    public interface IReadOnlyReactiveTrigger
    {
        IDisposable Subscribe(Action action);
    }
    public interface IReadOnlyReactiveTrigger<out T>
    {
        IDisposable Subscribe(Action<T> action);
    }
    public interface IReadOnlyReactiveTrigger<out T1, out T2>
    {
        IDisposable Subscribe(Action<T1, T2> action);
    } 
    public interface IReadOnlyReactiveTrigger<out T1, out T2, out T3>
    {
        IDisposable Subscribe(Action<T1, T2, T3> action);
    }
    public interface IReadOnlyReactiveTrigger<out T1, out T2, out T3, out T4>
    {
        IDisposable Subscribe(Action<T1, T2, T3, T4> action);
    }
}