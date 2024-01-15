using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Shared
{
    public class CharacterState
    {
        public Vector3 initialSpeed;
        public Vector3 speed;
        public float height;
        public CharacterAction currentAction;
        public Transform currentSavePoint;
        public LinkedListNode<RoadlinePoint> currentRoadline;
        public ReactiveProperty<int> lives;
    }
}