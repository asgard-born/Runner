using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class AbyssLargePlatform : Platform
    {
        protected override void Awake()
        {
            base.Awake();

            _behaviourType = typeof(AbyssLargeInteractingBehaviour);
        }
    }
}