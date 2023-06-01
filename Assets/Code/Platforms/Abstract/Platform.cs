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
        [SerializeField] private TriggerZone _passingZone;
        [SerializeField] private TriggerZone _interactionZone;
        [SerializeField] private Transform _respawnPoint;
        [SerializeField] private Transform _checkingPoint;

        public Transform respawnPoint => _respawnPoint;
        public Transform checkingPoint => _checkingPoint;

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

        public Type behaviourType { get; protected set; }

        public Action<Platform> OnInterractedWithPlayer;
        public Action<Platform> OnPassedByPlayer;

        public PlatformType platformType => _platformType;

        protected virtual void OnPlayerPassed()
        {
            _passingZone.SetEnable(false);
            OnPassedByPlayer?.Invoke(this);
        }

        protected virtual void OnPlayerInteraction()
        {
            _interactionZone.SetEnable(false);

            if (behaviourType != null)
            {
                OnInterractedWithPlayer?.Invoke(this);
            }
        }

        public override void Init()
        {
            if (_passingZone != null)
            {
                _passingZone.SetEnable(true);
            }

            if (_interactionZone != null)
            {
                _interactionZone.SetEnable(true);
            }
        }

        public void Dispose()
        {
            ReturnToPool();

            OnInterractedWithPlayer = null;
            OnPassedByPlayer = null;
        }
    }
}