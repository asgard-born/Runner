using Framework;
using Shared;
using UniRx;
using UnityEngine;

namespace UI.Pms
{
    public class VirtualPadPm : BaseDisposable
    {
        private Vector2 _playerInputData;
        private ReactiveCommand<SwipeDirection> _onSwipeDirection;

        public struct Ctx
        {
            public ReactiveCommand<SwipeDirection> onSwipeDirection;
            public ReactiveCommand<(Vector2, Vector2)> onSwipeRaw;
        }

        public VirtualPadPm(Ctx ctx)
        {
            AddUnsafe(ctx.onSwipeRaw.Subscribe(UpdateInput));
            
            _onSwipeDirection = ctx.onSwipeDirection;
        }

        private void UpdateInput((Vector2 previous, Vector2 current) positions)
        {
            _playerInputData = positions.current - positions.previous;

            SwipeDirection direction;

            if (Mathf.Abs(_playerInputData.x) > Mathf.Abs(_playerInputData.y))
            {
                direction = _playerInputData.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {
                direction = _playerInputData.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            }

            _onSwipeDirection?.Execute(direction);
        }
    }
}