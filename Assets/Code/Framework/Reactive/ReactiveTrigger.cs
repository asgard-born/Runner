using System;
using UniRx;

namespace Framework.Reactive
{
    public class ReactiveTrigger : IReadOnlyReactiveTrigger, IDisposable
    {
        private readonly Subject<bool> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<bool>();
        }

        public void Dispose()
        {
            _subject.Dispose();
        }

        public IDisposable Subscribe(Action action)
        {
            return _subject.Subscribe(b => action?.Invoke());
        }

        public void Notify()
        {
            _subject.OnNext(true);
        }
    }
    
    public class ReactiveTrigger<T> : IReadOnlyReactiveTrigger<T>, IDisposable
    {
        private readonly Subject<T> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<T>();
        }

        public void Dispose()
        {
            _subject.Dispose();
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return _subject.Subscribe(action);
        }

        public void Notify(T value)
        {
            _subject.OnNext(value);
        }
    } 
    
    public class ReactiveTrigger<T1, T2> : IReadOnlyReactiveTrigger<T1, T2>, IDisposable
    {
        private struct Entry
        {
            public T1 arg1;
            public T2 arg2;
        }
        
        private readonly Subject<Entry> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<Entry>();
        }

        public void Dispose()
        {
            _subject.Dispose();
        }

        public IDisposable Subscribe(Action<T1, T2> action)
        {
            return _subject.Subscribe(e => action?.Invoke(e.arg1, e.arg2));
        }

        public void Notify(T1 arg1, T2 arg2)
        {
            _subject.OnNext(new Entry { arg1 = arg1, arg2 = arg2});
        }
    }
    
    public class ReactiveTrigger<T1, T2, T3> : IReadOnlyReactiveTrigger<T1, T2, T3>, IDisposable
//        , IObservable<ReactiveTrigger<T1, T2, T3>.Entry>
    {
        public struct Entry
        {
            public T1 arg1;
            public T2 arg2;
            public T3 arg3;
        }

        public class WhereObservable : IReadOnlyReactiveTrigger<T1, T2, T3>
        {
            private readonly IObservable<Entry> _observable;
            private readonly Subject<Entry> _subject;
            private ReactiveTrigger<T1, T2, T3> _trigger;

            public WhereObservable(IObservable<Entry> observable)
            {
                _observable = observable;
            }
            public IDisposable Subscribe(Action<T1, T2, T3> action)
            {
                return _observable.Subscribe(e => action?.Invoke(e.arg1, e.arg2, e.arg3));
            }
        }

        private readonly Subject<Entry> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<Entry>();
        }

//       How to Pack ReactiveTrigger to IObservable<T1, T2, T3>
        public IReadOnlyReactiveTrigger<T1, T2, T3> Where(Func<T1, T2, T3, bool> selector)
        {
            // return _subject.Where(e => selector(e.arg1, e.arg2, e.arg3) );
            IObservable<Entry> observable = _subject.Where(e => selector(e.arg1, e.arg2, e.arg3) );
            return new WhereObservable( observable );
        }
        
        // TODO : Select support

        public void Dispose()
        {
            _subject.Dispose();
        }

        public IDisposable Subscribe(Action<T1, T2, T3> action)
        {
            return _subject.Subscribe(e => action?.Invoke(e.arg1, e.arg2, e.arg3));
        }

        public void Notify(T1 arg1, T2 arg2, T3 arg3)
        {
            _subject.OnNext(new Entry { arg1 = arg1, arg2 = arg2, arg3 = arg3});
        }
    }

    public class ReactiveTrigger<T1, T2, T3, T4> : IReadOnlyReactiveTrigger<T1, T2, T3, T4>, IDisposable
    {
        private struct Entry
        {
            public T1 arg1;
            public T2 arg2;
            public T3 arg3;
            public T4 arg4;
        }

        private readonly Subject<Entry> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<Entry>();
        }

        public void Dispose()
        {
            _subject.Dispose();
        }

        public IDisposable Subscribe(Action<T1, T2, T3, T4> action)
        {
            return _subject.Subscribe(e => action?.Invoke(e.arg1, e.arg2, e.arg3, e.arg4));
        }

        public void Notify(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _subject.OnNext(new Entry { arg1 = arg1, arg2 = arg2, arg3 = arg3, arg4 = arg4 });
        }
    }
}