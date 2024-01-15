using Behaviour.Behaviours.Abstract;

namespace Behaviour.Behaviours.Velocity
{
    public class SlowBehaviourPm : CharacterBehaviourPm
    {
        public SlowBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        protected override void Initialize()
        {
            _hasStarted = true;
            _ctx.state.speed = _ctx.configs.speed;
        }

        protected override void Behave()
        {
        }

        protected override void OnTimesOver()
        {
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }
    }
}