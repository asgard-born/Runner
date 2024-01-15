using Behaviour.Behaviours.Abstract;

namespace Behaviour.Behaviours.Velocity
{
    /// <summary>
    /// Поведение игрока типа Быстрое. Поведению делегируется работа с компонентами и состоянием персонажа
    /// и обработка пользовательского свайпа в разные стороны
    /// </summary>
    public class FastBehaviourPm : CharacterBehaviourPm
    {
        public FastBehaviourPm(Ctx ctx) : base(ctx)
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