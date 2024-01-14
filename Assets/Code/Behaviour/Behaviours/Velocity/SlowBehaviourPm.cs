using Behaviour.Behaviours.Abstract;
using Shared;

namespace Behaviour.Behaviours.Velocity
{
    public class SlowBehaviourPm : CharacterBehaviourPm
    {
        public SlowBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        protected override void Initialize()
        {
        }

        protected override void Behave()
        {
        }

        protected override void OnTimesOver()
        {
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        protected override void OnSwipeDirection(Direction direction)
        {
        }
    }
}