using System;
using Code.ObjectsPool;
using Code.Platforms.Essences;
using Code.Platforms.Helpers;
using UnityEngine;

namespace Code.Platforms.Abstract
{
    public abstract class Platform : PoolObject, IDisposable, IEquatable<Platform>
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

        public bool Equals(Platform other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return _platformType == other._platformType && _passingZone == other._passingZone && _interactionZone == other._interactionZone && _respawnPoint == other.respawnPoint &&
                   _checkingPoint == other._checkingPoint && Equals(OnInterractedWithPlayer, other.OnInterractedWithPlayer) && Equals(OnPassedByPlayer, other.OnPassedByPlayer) && behaviourType == other.behaviourType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            return Equals((Platform)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)_platformType;
                hashCode = (hashCode * 397) ^ (_passingZone != null ? _passingZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_interactionZone != null ? _interactionZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_respawnPoint != null ? _respawnPoint.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_checkingPoint != null ? _checkingPoint.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OnInterractedWithPlayer != null ? OnInterractedWithPlayer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OnPassedByPlayer != null ? OnPassedByPlayer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (behaviourType != null ? behaviourType.GetHashCode() : 0);

                return hashCode;
            }
        }
    }
}