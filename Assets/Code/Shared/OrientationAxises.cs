using System;
using UnityEngine;

namespace Shared
{
    [Serializable]
    public struct OrientationAxises
    {
        [SerializeField] private Transform _leftAxis;
        [SerializeField] private Transform _rightAxis;

        public Transform leftAxis => _leftAxis;
        public Transform rightAxis => _rightAxis;
    }
}