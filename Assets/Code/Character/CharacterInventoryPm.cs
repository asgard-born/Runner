using Framework;
using Framework.Reactive;
using UniRx;

namespace Character
{
    /// <summary>
    /// Отвечает за хранение монет и других возможных ресурсов: кристаллов и тд
    /// </summary>
    public class CharacterInventoryPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public ReactiveProperty<int> coins;
            public ReactiveTrigger onCoinTaken;
        }

        public CharacterInventoryPm(Ctx ctx)
        {
            _ctx = ctx;
            
            AddUnsafe(ctx.onCoinTaken.Subscribe(OnCoinTaken));
        }

        private void OnCoinTaken()
        {
            _ctx.coins.Value += 1;
        }
    }
}