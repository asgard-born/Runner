using System;
using Code.ObjectsPool;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject, IDisposable
    {
        public Action<Type> OnInterractedByPlayer;

        protected virtual void OnPlayerInteraction()
        {
            OnInterractedByPlayer?.Invoke(this.GetType());
        }

        public void Dispose()
        {
            OnInterractedByPlayer = null;
        }
    }
}