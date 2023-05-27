using UnityEngine;

namespace Code.Platforms.Abstract
{
    public abstract class TurnPlatform : Platform, ITurnPlatform
    {
        [SerializeField] private Transform _firstPartTransform;
        [SerializeField] private Transform _lastPartTransform;
        public Transform firstTransform => _firstPartTransform;
        public Transform lastPartTransform => _lastPartTransform;
    }
}