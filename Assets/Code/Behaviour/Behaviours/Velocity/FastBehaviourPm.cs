using Behaviour.Behaviours.Abstract;
using UnityEngine;

namespace Behaviour.Behaviours.Velocity
{
    /// <summary>
    /// Поведение игрока типа Быстрое. Данному поведению делегируется исключительно
    /// работа с состоянием персонажа, а именно с его скоростью
    /// </summary>
    public class FastBehaviourPm : CharacterBehaviourPm
    {
        private Vector3 _cachedSpeed;

        public FastBehaviourPm(Ctx ctx) : base(ctx)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            _ctx.state.speed = new Vector3(_ctx.configs.speed.x, _ctx.state.speed.y, _ctx.configs.speed.z);
            _canTiming = true;
        }

        protected override void OnTimeOver()
        {
            Reset();
            
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.key);
        }

        protected override void Reset()
        {
            _ctx.state.speed = new Vector3(_ctx.state.initialSpeed.x, _ctx.state.speed.y, _ctx.state.initialSpeed.z);
        }
    }
}