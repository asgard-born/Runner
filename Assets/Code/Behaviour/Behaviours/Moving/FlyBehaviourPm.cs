using Behaviour.Behaviours.Abstract;
using Shared;
using UnityEngine;

namespace Behaviour.Behaviours.Moving
{
    public class FlyBehaviourPm : MovingBehaviourPm
    {
        protected static readonly int _landing = Animator.StringToHash("Landing");

        public FlyBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        protected override void OnTimesOver()
        {
            _currentAction = CharacterAction.Landing;
        }

        protected override void InitializeState()
        {
        }

        protected override void Behave()
        {
            if (_isMoving)
            {
                Move();
            }

            switch (_currentAction)
            {
                case CharacterAction.Running:
                    _ctx.animator.SetTrigger(_running);

                    break;

                case CharacterAction.Landing:
                    _ctx.animator.SetTrigger(_landing);

                    break;
            }
        }

        protected override void OnSwipeDirection(SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.Left:
                case SwipeDirection.Right:
                    OnChangeSide(swipeDirection);

                    break;
            }
        }
    }
}