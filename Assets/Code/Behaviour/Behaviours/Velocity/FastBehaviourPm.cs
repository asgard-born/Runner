using Behaviour.Behaviours.Abstract;
using UnityEngine;

namespace Behaviour.Behaviours.Velocity
{
    /// <summary>
    /// Поведение игрока типа Быстрое. Поведению делегируется работа с компонентами и состоянием персонажа
    /// и обработка пользовательского свайпа в разные стороны
    /// </summary>
    public class FastBehaviourPm : CharacterBehaviourPm
    {
        private Vector3 _cachedSpeed;

        public FastBehaviourPm(Ctx ctx) : base(ctx)
        {
        }

        protected override void Initialize()
        {
            _ctx.state.speed = new Vector3(_ctx.configs.speed.x, _ctx.state.speed.y, _ctx.configs.speed.z);
            _hasStarted = true;
        }

        protected override void OnTimeOver()
        {
            Reset();
            
            _ctx.onBehaviourFinished?.Execute(_ctx.configs.type);
        }

        protected override void Reset()
        {
            _ctx.state.speed = new Vector3(_ctx.state.initialSpeed.x, _ctx.state.speed.y, _ctx.state.initialSpeed.z);
        }
    }
}