using System.Collections.Generic;

namespace Shared
{
    public struct CharacterState
    {
        public float initialVelocity;
        public float velocity;
        public float jumpForce;
        public LinkedListNode<RoadlinePoint> currentRoadline;
    }
}