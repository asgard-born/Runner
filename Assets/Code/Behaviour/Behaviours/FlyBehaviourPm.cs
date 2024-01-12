using Shared;

namespace Behaviour.Behaviours
{
    public class FlyBehaviourPm : CharacterBehaviourPm
    {
        public FlyBehaviourPm(Ctx ctx) : base(ctx)
        {
            
        }

        protected override void DoBehave()
        {
        }

        protected override void OnSwipe(SwipeDirection swipeDirection)
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