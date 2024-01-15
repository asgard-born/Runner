using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Shared
{
    public class CharacterState
    {
        public Vector3 speed;
        public float jumpForce;
        public ReactiveProperty<int> lives;
        public Transform currentSaveZone;
        public LinkedListNode<RoadlinePoint> currentRoadline;
    }
}