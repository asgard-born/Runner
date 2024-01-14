using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public struct CharacterState
    {
        public Vector3 speed;
        public float jumpForce;
        public LinkedListNode<RoadlinePoint> currentRoadline;
    }
}