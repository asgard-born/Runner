using System.Collections.Generic;

namespace Shared
{
    public struct CharacterState
    {
        public float initialSpeed;
        public float speed;
        public float jumpForce;
        public LinkedListNode<Roadline> currentRoadline;
    }
}