using System;
using Code.ObjectsPool;
using Code.Platforms.Essences;
using Code.Platforms.Helpers;
using UnityEngine;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject, IDisposable
    {
        [SerializeField] protected PlatformType _platformType;
        [SerializeField] private PlatformTriggerZone _passingZone;
        [SerializeField] private PlatformTriggerZone _interactionZone;

        protected virtual void Awake()
        {
            if (_passingZone != null)
            {
                _passingZone.OnPlayerEntered += OnPlayerPassed;
            }

            if (_interactionZone != null)
            {
                _interactionZone.OnPlayerEntered += OnPlayerInteraction;
            }
        }

        protected Type _behaviourType;

        public Action<Type> OnInterractedWithPlayer;
        public Action<PlatformType> OnPassedByPlayer;

        public PlatformType platformType => _platformType;

        protected virtual void OnPlayerPassed()
        {
            OnPassedByPlayer?.Invoke(_platformType);
        }

        protected virtual void OnPlayerInteraction()
        {
            if (_behaviourType != null)
            {
                OnInterractedWithPlayer?.Invoke(_behaviourType);
            }
        }

        public void Dispose()
        {
            OnInterractedWithPlayer = null;
        }
    }
}