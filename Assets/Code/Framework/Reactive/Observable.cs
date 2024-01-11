using System;
using System.Collections.Generic;

namespace Framework.Reactive
{
    public class Observable<T> : IDisposable
    {
        protected T _value = default(T);
        private Action<T> _onChange;

        public event Action<T> OnChange
        {
            add {
                _onChange += value;
                value.Invoke(_value);
            }
            remove => _onChange -= value;
        }
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                    return;

                _value = value;
                OnChangeValue(value);
            }
        }

        public Observable(T value) => Value = value;

        protected virtual void OnChangeValue(T value) => _onChange?.Invoke(value);

        public void Dispose()
        {
            _onChange = null;
        }
    }
}