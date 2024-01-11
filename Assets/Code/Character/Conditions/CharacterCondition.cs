using Framework;
using Shared;

namespace Character.Conditions
{
    public abstract class CharacterCondition : BaseDisposable
    {
        protected readonly Ctx _ctx;

        public struct Ctx
        {
            public float time;
            public EffectContainer[] effects;
            public CharacterAnimatorView animatorView;
            public CharacterStats stats;
        }

        public abstract void DoConditioning();
    }
}