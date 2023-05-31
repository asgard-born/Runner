using Code.Platforms.Abstract;
using Code.PlatformsBehaviour;

namespace Code.Platforms.Concrete
{
    public class AbyssPlatfrom : Platform
    {
        protected override void Awake()
        {
            base.Awake();

            _behaviourType = typeof(AbyssInteractingBehaviour);
        }
    }
}