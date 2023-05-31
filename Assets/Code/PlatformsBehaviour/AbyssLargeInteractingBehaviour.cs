using System;
using Code.PlatformsBehaviour.Abstract;

namespace Code.PlatformsBehaviour
{
    public class AbyssLargeInteractingBehaviour : PlatformInteractingBehaviour
    {
        private Action _callback;

        public AbyssLargeInteractingBehaviour(Action callback)
        {
            _callback = callback;
        }

        public override void InteractWithPlayer()
        {
            _callback?.Invoke();
        }
    }
}