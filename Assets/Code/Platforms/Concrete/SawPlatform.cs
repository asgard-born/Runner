using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class SawPlatform : Platform
    {
        protected override void Awake()
        {
            base.Awake();

            behaviourType = typeof(SawInteractingBehaviour);
        }
    }
}