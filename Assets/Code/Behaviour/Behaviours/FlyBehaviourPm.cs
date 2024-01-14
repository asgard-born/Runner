﻿using Shared;

namespace Behaviour.Behaviours
{
    public class FlyBehaviourPm : CharacterBehaviourPm
    {
        public FlyBehaviourPm(Ctx ctx) : base(ctx)
        {
            
        }

        protected override void Behave()
        {
        }

        protected override void OnSwipeDirection(SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.Left:
                    break;

                case SwipeDirection.Right:
                    break;
            }
        }
    }
}