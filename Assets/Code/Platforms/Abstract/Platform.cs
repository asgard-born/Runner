using System;
using Code.ObjectsPool;
using Code.Platforms.Essences;
using UnityEngine;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject, IDisposable
    {
        [SerializeField] protected PlatformType _platformType;
        protected Type _behaviourType;

        public Action<Type> OnInterractedWithPlayer;
        public PlatformType platformType => _platformType;

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