using Code.Platforms.Helpers;
using UnityEngine;

namespace Code.Platforms.Abstract
{
    public abstract class TurnPlatform : Platform, ITurnPlatform
    {
        [SerializeField] private Transform _firstPartTransform;
        [SerializeField] private Transform _lastPartTransform;
        [SerializeField] private PlatformTriggerZone _triggerZone;
        public Transform firstTransform => _firstPartTransform;
        public Transform lastPartTransform => _lastPartTransform;
        

        private void Awake()
        {
            _triggerZone.OnPlayerEntered += OnPlayerInteraction;
        }
    }
}