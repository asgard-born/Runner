using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class StandardPlatform : Platform
    {
        private void Awake()
        {
            _behaviourType = typeof(StandardInteractingBehaviour);
        }
    }
}