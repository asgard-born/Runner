using ObjectsPool;
using Shared.Containers;
using UnityEngine;

namespace Items
{
    public class Item : PoolObject
    {
        [SerializeField] protected EffectContainer[] _effects;
        [SerializeField] protected float _timeSec;

        public EffectContainer[] effects => _effects;
        public float timeSec => _timeSec;
    }
}