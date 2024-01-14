using Behaviour.Behaviours.Abstract;
using Shared;

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

        protected override void Behave()
        {
            
        }

        protected override void OnSwipeDirection(SwipeDirection swipeDirection)
        {
            
        }
    }
}