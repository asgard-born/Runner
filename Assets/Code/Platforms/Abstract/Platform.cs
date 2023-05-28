using System;
using Code.ObjectsPool;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject, IDisposable
    {
        public Action<Type> OnInterractedWithPlayer;
        protected Type _behaviourType;

        protected virtual void OnPlayerInteraction()
        {
            OnInterractedWithPlayer?.Invoke(_behaviourType);
        }

        public void Dispose()
        {
            OnInterractedWithPlayer = null;
        }
    }
}