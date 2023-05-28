using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class AbyssLargePlatform : Platform
    {
        private void Awake()
        {
            _behaviourType = typeof(AbyssLargeInteractingBehaviour);
        }
    }
}