using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public struct CharacterState
    {
        public float speed;
        public float jumpForce;
        public float sideSpeed;
        public Vector3 velocity;
        public LinkedListNode<RoadlinePoint> currentRoadline;
    }
}