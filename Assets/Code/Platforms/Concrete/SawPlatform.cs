using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class SawPlatform : Platform
    {
        private void Awake()
        {
            _behaviourType = typeof(SawInteractingBehaviour);
        }
    }
}