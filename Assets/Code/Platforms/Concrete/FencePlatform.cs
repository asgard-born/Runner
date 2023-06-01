using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class FencePlatform : Platform
    {
        protected override void Awake()
        {
            base.Awake();

            behaviourType = typeof(FenceInteractingBehaviour);
        }
    }
}