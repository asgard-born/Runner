using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class TurnRightPlatform : TurnPlatform
    {
        private void Start()
        {
            _behaviourType = typeof(TurnRightInteractingBehaviour);
        }
    }
}