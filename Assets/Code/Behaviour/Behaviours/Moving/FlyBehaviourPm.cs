using Behaviour.Behaviours.Abstract;
using Shared;

namespace Behaviour.Behaviours.Moving
{
    public class FlyBehaviourPm : MovingBehaviourPm
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
                case SwipeDirection.Right:
                    OnChangeSide(swipeDirection);

                    break;
            }
        }
    }
}