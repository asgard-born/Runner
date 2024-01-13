using System;
using UnityEngine;

namespace Shared
{
    [Serializable]
    public class RoadlinePoint
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private RoadlineType _type;

        public Transform transform => _transform;
        public RoadlineType type => _type;
    }
}