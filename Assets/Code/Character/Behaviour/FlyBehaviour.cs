using Shared;

namespace Character.Behaviour
{
    public class FlyBehaviour : CharacterBehaviour
    {
        public FlyBehaviour(Ctx ctx) : base(ctx)
        {
        }

        public override void DoBehave()
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