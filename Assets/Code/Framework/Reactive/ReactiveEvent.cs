using System;
using UniRx;

namespace Framework.Reactive
{
    public class ReactiveEvent<T> : IReadOnlyReactiveEvent<T>, IDisposable
    {
        private readonly ReactiveProperty<T> _property;

        public ReactiveEvent()
        {
            _property = new ReactiveProperty<T>();
        }

        public ReactiveEvent(T init)
        {
            _property = new ReactiveProperty<T>(init);
        }

        public void Dispose()
        {
            _property?.Dispose();
        }

        public void Notify(T obj)
        {
            _property.SetValueAndForceNotify(obj);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return _property.Subscribe(action);
        }

        public IDisposable SubscribeWithSkip(Action<T> action)
        {
            return _property.SkipLatestValueOnSubscribe().Subscribe(action);
        }

        public IDisposable SubscribeOnceWithSkip(Action<T> action)
        {
            return _property.SkipLatestValueOnSubscribe().First().Subscribe(action);
        }
    }

    public class ReactiveEvent<T1, T2> : IReadOnlyReactiveEvent<T1, T2>, IDisposable
    {
        private struct Entry
        {
            public T1 arg1;
            public T2 arg2;
        }

        private readonly ReactiveProperty<Entry> _property;

        public ReactiveEvent()
        {
            _property = new ReactiveProperty<Entry>();
        }

        public ReactiveEvent(T1 arg1, T2 arg2)
        {
            _property = new ReactiveProperty<Entry>(new Entry { arg1 = arg1, arg2 = arg2 });
        }

        public void Dispose()
        {
            _property?.Dispose();
        }

        public void Notify(T1 arg1, T2 arg2)
        {
            _property.SetValueAndForceNotify(new Entry { arg1 = arg1, arg2 = arg2 });
        }

        public IDisposable Subscribe(Action<T1, T2> action)
        {
            return _property.Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2));
        }

        public IDisposable SubscribeWithSkip(Action<T1, T2> action)
        {
            return _property.SkipLatestValueOnSubscribe().
              Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2));
        }

        public IDisposable SubscribeOnceWithSkip(Action<T1, T2> action)
        {
            return _property.SkipLatestValueOnSubscribe().First().
              Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2));
        }

        private IDisposable Subscripe(Action<Entry> entry)
        {
            return _property.Subscribe(entry);
        }
    }

    public class ReactiveEvent<T1, T2, T3> : IReadOnlyReactiveEvent<T1, T2, T3>, IDisposable
    {
        private struct Entry
        {
            public T1 arg1;
            public T2 arg2;
            public T3 arg3;
        }

        private readonly ReactiveProperty<Entry> _property;

        public ReactiveEvent()
        {
            _property = new ReactiveProperty<Entry>();
        }

        public ReactiveEvent(T1 arg1, T2 arg2, T3 arg3)
        {
            _property = new ReactiveProperty<Entry>(new Entry { arg1 = arg1, arg2 = arg2, arg3 = arg3 });
        }

        public void Dispose()
        {
            _property?.Dispose();
        }

        public void Notify(T1 arg1, T2 arg2, T3 arg3)
        {
            _property.SetValueAndForceNotify(new Entry { arg1 = arg1, arg2 = arg2, arg3 = arg3 });
        }

        public IDisposable Subscribe(Action<T1, T2, T3> action)
        {
            return _property.Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2, entry.arg3));
        }

        public IDisposable SubscribeWithSkip(Action<T1, T2, T3> action)
        {
            return _property.SkipLatestValueOnSubscribe()
                .Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2, entry.arg3));
        }

        public IDisposable SubscribeOnceWithSkip(Action<T1, T2, T3> action)
        {
            return _property.SkipLatestValueOnSubscribe().First().
              Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2, entry.arg3));
        }

        private IDisposable Subscripe(Action<Entry> entry)
        {
            return _property.Subscribe(entry);
        }
    }

    public class ReactiveEvent<T1, T2, T3, T4> : IReadOnlyReactiveEvent<T1, T2, T3, T4>, IDisposable
    {
        private struct Entry
        {
            public T1 arg1;
            public T2 arg2;
            public T3 arg3;
            public T4 arg4;
        }

        private readonly ReactiveProperty<Entry> _property;

        public ReactiveEvent()
        {
            _property = new ReactiveProperty<Entry>();
        }

        public ReactiveEvent(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _property = new ReactiveProperty<Entry>(new Entry { arg1 = arg1, arg2 = arg2, arg3 = arg3, arg4 = arg4 });
        }

        public void Dispose()
        {
            _property?.Dispose();
        }

        public void Notify(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _property.SetValueAndForceNotify(new Entry { arg1 = arg1, arg2 = arg2, arg3 = arg3, arg4 = arg4 });
        }

        public IDisposable Subscribe(Action<T1, T2, T3, T4> action)
        {
            return _property.Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2, entry.arg3, entry.arg4));
        }

        public IDisposable SubscribeWithSkip(Action<T1, T2, T3, T4> action)
        {
            return _property.SkipLatestValueOnSubscribe().
              Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2, entry.arg3, entry.arg4));
        }

        public IDisposable SubscribeOnceWithSkip(Action<T1, T2, T3, T4> action)
        {
            return _property.SkipLatestValueOnSubscribe().First().
              Subscribe(entry => action?.Invoke(entry.arg1, entry.arg2, entry.arg3, entry.arg4));
        }

        private IDisposable Subscripe(Action<Entry> entry)
        {
            return _property.Subscribe(entry);
        }
    }
}