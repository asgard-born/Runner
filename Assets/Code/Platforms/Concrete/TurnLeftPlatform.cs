using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class TurnLeftPlatform : TurnPlatform
    {
        private void Start()
        {
            _behaviourType = typeof(TurnLeftInteractingBehaviour);
        }
    }
}