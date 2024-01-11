using System;
using System.Collections.Generic;
using UniRx;

namespace Framework.Reactive
{
    public interface IMessagePublisher<TMsgBase>
    {
        /// <summary>
        /// Send Message to all receiver.
        /// </summary>
        void Publish<TMsg>(TMsg message) where TMsg : TMsgBase;
    }

    public interface IMessageReceiver<TMsgBase>
    {
        /// <summary>
        /// Subscribe typed message.
        /// </summary>
        IObservable<TMsg> Receive<TMsg>() where TMsg : TMsgBase;
    }

    public interface IMessageBroker<T> : IMessagePublisher<T>, IMessageReceiver<T>
    {
    }

    public class MessageBroker<TMsgBase> : IMessageBroker<TMsgBase>, IDisposable
    {
        private class Notifier
        {
            public Type type;
            public object subset;
            public Action<TMsgBase> note;
        }

        private readonly LinkedList<Notifier> notifiers = new LinkedList<Notifier>();
        private bool isDisposed = false;

        public void Publish<TMsg>(TMsg message) where TMsg : TMsgBase
        {
            Type msgType = message.GetType();

            LinkedListNode<Notifier> node;
            lock (notifiers)
            {
                if (isDisposed)
                    throw new ObjectDisposedException($"MessageBroker<{msgType.Name}>");

                node = notifiers.First;
            }
            for (; node != null; node = GetNextNode(notifiers, node))
            {
                Notifier notifier = node.Value;
                if (notifier.type.IsAssignableFrom(msgType))
                    notifier.note(message);
            }
        }

        public IObservable<TMsg> Receive<TMsg>() where TMsg : TMsgBase
        {
            Type msgType = typeof(TMsg);
            Notifier notifier = null;

            lock (notifiers)
            {
                if (isDisposed)
                    throw new ObjectDisposedException($"MessageBroker<{msgType.Name}>");

                for (LinkedListNode<Notifier> node = notifiers.First; node != null; node = node.Next)
                    if (node.Value.type == msgType)
                    {
                        notifier = node.Value;
                        break;
                    }

                if (notifier == null)
                {
                    ISubject<TMsg> subset = new Subject<TMsg>().Synchronize();
                    notifier = new Notifier
                    {
                        subset = subset,
                        note = msg => subset.OnNext((TMsg)msg),
                        type = msgType
                    };
                    notifiers.AddLast(notifier);
                }
            }

            return ((IObservable<TMsg>)notifier.subset).AsObservable();
        }

        public void Dispose()
        {
            lock (notifiers)
                if (!isDisposed)
                {
                    isDisposed = true;
                    notifiers.Clear();
                }
        }

        private LinkedListNode<Notifier> GetNextNode(object locker, LinkedListNode<Notifier> node)
        {
            lock (locker)
                return node.Next;
        }
    }
}