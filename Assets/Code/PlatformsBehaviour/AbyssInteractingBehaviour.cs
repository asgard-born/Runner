using System;
using Code.PlatformsBehaviour.Abstract;

namespace Code.PlatformsBehaviour
{
    public class AbyssInteractingBehaviour : PlatformInteractingBehaviour
    {
        private Action _callback;

        public AbyssInteractingBehaviour(Action callback)
        {
            _callback = callback;
        }

        public override void InteractWithPlayer()
        {
            _callback?.Invoke();
        }
    }
}