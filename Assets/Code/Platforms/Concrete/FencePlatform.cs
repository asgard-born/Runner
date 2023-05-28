using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class FencePlatform : Platform
    {
        private void Awake()
        {
            _behaviourType = typeof(FenceInteractingBehaviour);
        }
    }
}