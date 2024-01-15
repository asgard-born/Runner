using Behaviour.Behaviours.Abstract;
using UnityEngine;

namespace Behaviour.Behaviours.Velocity
{
    public class SlowBehaviourPm : CharacterBehaviourPm
    {
        public SlowBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        protected override void Initialize()
        {
            _ctx.state.speed = new Vector3(_ctx.configs.speed.x, _ctx.state.speed.y, _ctx.configs.speed.z);
            _hasStarted = true;
        }

        protected override void OnTimeOver()
        {
            _ctx.state.speed = _ctx.state.speed = new Vector3(_ctx.state.initialSpeed.x, _ctx.state.speed.y, _ctx.state.initialSpeed.z);
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }
    }
}