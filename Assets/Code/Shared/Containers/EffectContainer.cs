using System;
using UnityEngine;

namespace Shared.Containers
{
    [Serializable]
    public struct EffectContainer
    {
        [SerializeField] private GameObject _effect;
        [SerializeField] private Transform _point;
        
        public GameObject effect => _effect;
        public Transform point => _point;
    }
}