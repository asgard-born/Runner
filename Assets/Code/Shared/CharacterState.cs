using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Shared
{
    public class CharacterState
    {
        public Vector3 speed;
        public float jumpForce;
        public Transform currentSaveZone;
        public LinkedListNode<RoadlinePoint> currentRoadline;
        public ReactiveProperty<int> lives;
        public ReactiveProperty<int> coins;
    }
}