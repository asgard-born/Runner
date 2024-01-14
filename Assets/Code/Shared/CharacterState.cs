using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public struct CharacterState
    {
        public float velocity;
        public float jumpForce;
        public float sideSpeed;
        public Vector3 nextPosition;
        public LinkedListNode<RoadlinePoint> currentRoadline;
    }
}